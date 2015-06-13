using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace CNC_Drill_Controller1
{
    class VDXLoader
    {
        private class rawShapeData
        {
            public float x, y;
            public bool isEllipse;
            public bool isDuplicate;
            public bool isZero;

        }
        private List<rawShapeData> Shapes;

        public float PageWidth, PageHeight;
        public List<DrillNode> DrillNodes; 
        public float NodeEpsilon = 0.010f;
        public string log;



        private void AddLine(string s)
        {
            if (log != "") log = log + " / " + s;
            else log = s;
        }

        private void cleanList()
        {
            for (var i = 0; i < Shapes.Count; i++)
            {
                for (var j = 0; j < Shapes.Count; j++)
                {
                    if (i != j)
                    {
                        Shapes[i].isZero = (Math.Abs(Shapes[i].x) < float.Epsilon) && (Math.Abs(Shapes[i].y) < float.Epsilon);

                        Shapes[i].isDuplicate =
                            (Math.Sqrt(Math.Pow(Shapes[i].x - Shapes[j].x, 2) + Math.Pow(Shapes[i].y - Shapes[j].y, 2)) <
                             NodeEpsilon) && (!Shapes[j].isDuplicate);
                    }
                }
            }
        }

        public VDXLoader(string FileName, bool flipX)
        {
            if (File.Exists(FileName))
            {
                //load and parse data;
                log = "Loading:";
                Shapes = new List<rawShapeData>();
                PageWidth = 11.0f;
                PageHeight = 11.0f; 
                DrillNodes = new List<DrillNode>();

                try
                {
                    var f = File.OpenText(FileName);
                    SeekShape(f);
                    f.Close();
                }
                catch (IOException ex)
                {
                    AddLine(ex.Message);
                    DrillNodes.Clear();
                    return;
                }

                AddLine(Shapes.Count.ToString("D") + " Shapes");
                AddLine(Shapes.Sum(d => (d.isEllipse) ? 1 : 0).ToString("D") + " Ellipses");
                cleanList();
                AddLine(Shapes.Sum(d => (d.isZero) ? 1 : 0).ToString("D") + " Zeros");
                AddLine(Shapes.Sum(d => (d.isDuplicate) ? 1 : 0).ToString("D") + " Duplicates");

                AddLine("Page Width: " + PageWidth.ToString("F1"));
                AddLine("Page Height: " + PageHeight.ToString("F1"));
                
                foreach (var rawShape in Shapes)
                {
                    if (rawShape.isEllipse && !rawShape.isDuplicate && !rawShape.isZero)
                    {
                        DrillNodes.Add(new DrillNode(new PointF(flipX? (PageWidth-rawShape.x) : rawShape.x, PageHeight - rawShape.y),-1));
                    }
                }

            }
        }

        private void SeekShape(StreamReader reader)
        {

            while (!reader.EndOfStream)
            {
                var l = reader.ReadLine();
                if (l != null)
                {
                    if (l.StartsWith("<Shape ID="))
                    {
                        Shapes.Add(readUntilEndOfShape(reader));
                    }
                    if (l.StartsWith("<PageWidth Unit='IN'>"))
                    {
                        PageWidth = SafeFloatParse(TrimString(l, "<PageWidth Unit='IN'>", "</PageWidth>"), PageWidth);
                    }
                    if (l.StartsWith("<PageHeight Unit='IN'>"))
                    {
                        PageHeight = SafeFloatParse(TrimString(l, "<PageHeight Unit='IN'>", "</PageHeight>"), PageWidth);
                    }
                    //<PageWidth Unit='IN'>8.5</PageWidth>
                    //<PageHeight Unit='IN'>11</PageHeight>
                }
            }
        }

        private rawShapeData readUntilEndOfShape(StreamReader reader)
        {
            var endFound = false;
            var newShape = new rawShapeData();
            while (!reader.EndOfStream & !endFound)
            {
                var l = reader.ReadLine();
                if (l == "</Shape>") endFound = true;
                if (l != null && l.StartsWith("<Shape ID="))
                {
                    Shapes.Add(readUntilEndOfShape(reader));
                }

                if (l != null && l.StartsWith("<PinX>"))
                {
                    var pin = TrimString(l, "<PinX>", "</PinX>");
                    newShape.x = SafeFloatParse(pin, 0.0f);
                }
                if (l != null && l.StartsWith("<PinY>"))
                {
                    var pin = TrimString(l, "<PinY>", "</PinY>");
                    newShape.y = SafeFloatParse(pin, 0.0f);

                }
                if (l != null && l.StartsWith("<Ellipse IX=")) newShape.isEllipse = true;
                //012345678901234567
                //<PinX>4.125</PinX>
                //<PinY>9.875</PinY>
                //012345     0123456
            }
            return newShape;
        }

        private static string TrimString(string source, string Start, string End)
        {
            return source.Substring(Start.Length, source.Length - End.Length - Start.Length);
        }

        private static float SafeFloatParse(string s, float fallback)
        {
            float result;
            return float.TryParse(s, out result) ? result : fallback;
        }
    }
}
