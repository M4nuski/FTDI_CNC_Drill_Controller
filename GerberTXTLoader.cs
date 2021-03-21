using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace CNC_Drill_Controller1
{
    class GerberTXTLoader : INodeLoader
    {
        // load and parse Gerber TXT drill coordinates files
        public float PageWidth { get; set; }
        public float PageHeight { get; set; }
        public List<DrillNode> DrillNodes { get; set; }

        public void Load(string Filename, DrawingTypeDialog.DrawingConfigStruct DrawingConfig)
        {
            try
            {
                var f = File.OpenText(Filename);

                var numFormat = GetNumFormat(f);
                if (numFormat == "") throw new Exception("Cannot find position encoding mode in header.");

                ReadNodes(f, numFormat, DrawingConfig);
                f.Close();

                AdjustNodesToOrigin();
                GetPageSize();

            }
            catch (IOException ex)
            {
                ExtLog.AddLine(ex.Message);
                DrillNodes.Clear();
            }

        }

        private void AdjustNodesToOrigin()
        {
            var minPosX = float.MaxValue;
            var minPosY = float.MaxValue;

            foreach (var node in DrillNodes)
            {
                minPosX = Math.Min(node.location.X, minPosX);
                minPosY = Math.Min(node.location.Y, minPosY);
            }

            foreach (var node in DrillNodes)
            {
                node.location.X -= minPosX;
                node.location.Y -= minPosY;
            }
        }

        private void GetPageSize()
        {
            var PageWidth = float.MinValue;
            var PageHeight = float.MinValue;

            foreach (var node in DrillNodes)
            {
                PageWidth = Math.Max(node.location.X, PageWidth);
                PageHeight = Math.Max(node.location.Y, PageHeight);
            }
        }

        private void ReadNodes(StreamReader f, string numFormat, DrawingTypeDialog.DrawingConfigStruct drawingConfig)
        {
            var numLen = drawingConfig.gerber_intLen + drawingConfig.gerber_fractLen;

            while (!f.EndOfStream)
            {
                var line = f.ReadLine();
                line = line.Trim();

                // X032174Y008630
                // 01234567890123

                if ((line.Length > 0) && (line[0] == 'X'))
                {
                    var yindex = line.IndexOf('Y');
                    if (yindex > -1)
                    {
                        var xText = line.Substring(1, yindex - 1);
                        var yText = line.Substring(yindex + 1);

                        if (numFormat == "INCH")
                        {
                            // leading and trailing 0 not suppressed
                            // XnnmmmmYnnmmmm
                            if (xText.Length != numLen) throw new Exception($"Unexpected number format - length is {xText.Length} but config expected {numLen}");
                            if (yText.Length != numLen) throw new Exception($"Unexpected number format - length is {yText.Length} but config expected {numLen}");

                            float x = parseLocation(xText, drawingConfig);
                            float y = parseLocation(yText, drawingConfig);

                            DrillNodes.Add(new DrillNode(new PointF(x, y)));

                        } else if (numFormat == "INCH,LZ")
                        {
                            // leading 0 only
                            // XiiifYiiif 0.620 = 0062__
                            // trailing 0 are removed

                            xText = xText.PadRight(numLen, '0');
                            yText = yText.PadRight(numLen, '0');

                            float x = parseLocation(xText, drawingConfig);
                            float y = parseLocation(yText, drawingConfig);

                            DrillNodes.Add(new DrillNode(new PointF(x, y)));
                        } else if (numFormat == "INCH,TZ")
                        {
                            // trailing 0 only
                            // XiiifYiiif 0.620 = __6200
                            // leading 0 are removed
                            xText = xText.PadLeft(numLen, '0');
                            yText = yText.PadLeft(numLen, '0');

                            float x = parseLocation(xText, drawingConfig);
                            float y = parseLocation(yText, drawingConfig);

                            DrillNodes.Add(new DrillNode(new PointF(x, y)));
                        }
                    } 
                }
            }
        }

        private float parseLocation(string text, DrawingTypeDialog.DrawingConfigStruct drawingConfig)
        {
            var Int = int.Parse(text.Substring(0, drawingConfig.gerber_intLen));
            var Fract = int.Parse(text.Substring(drawingConfig.gerber_intLen, drawingConfig.gerber_fractLen));
            var denum = (float)Math.Pow(10, drawingConfig.gerber_fractLen);
            return (float)Int + ((float)Fract / denum);
        }

        private string GetNumFormat(StreamReader f)
        {
            var mode = "";
            while (!f.EndOfStream)
            {
                var line = f.ReadLine();
                if (line == "INCH") mode = "INCH";
                if (line == "INCH,TZ") mode = "INCH,TZ";
                if (line == "INCH,LZ") mode = "INCH,LZ";
                if (line == "%") return mode;
            }

            return mode;
        }
    }
}
