using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CNC_Drill_Controller1
{
    class VDXLoader : INodeLoader
    {
        private class rawShapeData
        {
            public float x, y;
            public float w, h;
            public bool isEllipse;
            public bool isDuplicate;
            public bool isZero;

        }
        private List<rawShapeData> Shapes;

        public float PageWidth { get; set; }
        public float PageHeight { get; set; }
        public List<DrillNode> DrillNodes { get; set; }

        private void RemoveNonEllipses()
        {
            Shapes.RemoveAll(d => !d.isEllipse);
            ExtLog.AddLine($"{Shapes.Count} Ellipses");
        }

        private void RemoveZero()
        {
            for (var i = 0; i < Shapes.Count; i++)
            {
                Shapes[i].isZero = (Math.Abs(Shapes[i].x) < float.Epsilon) && (Math.Abs(Shapes[i].y) < float.Epsilon);
            }
            Shapes.RemoveAll(d => d.isZero);
            ExtLog.AddLine($"{Shapes.Count} Non-zeros");
        }

        private void RemoveDuplicates()
        {
            var dup = new bool[Shapes.Count];
            for (var i = 0; i < Shapes.Count; i++)
            {
                if (!dup[i]) for (var j = 0; j < Shapes.Count; j++)
                    {
                        if ((i != j) && !dup[j])
                        {
                            var dist =
                                Math.Sqrt(Math.Pow(Shapes[i].x - Shapes[j].x, 2) + Math.Pow(Shapes[i].y - Shapes[j].y, 2));
                            dup[j] = dist < GlobalProperties.NodeEpsilon;
                        }
                    }
            }
            for (var i = 0; i < Shapes.Count; i++)
            {
                Shapes[i].isDuplicate = dup[i];
            }

            Shapes.RemoveAll(d => d.isDuplicate);
            ExtLog.AddLine($"{Shapes.Count} Uniques");
        }

        private void FlipXShapes()
        {
            foreach (var rawShapeData in Shapes)
            {
                rawShapeData.x = PageWidth - rawShapeData.x;
            }
        }

        private void FlipYShapes()
        {
            foreach (var rawShapeData in Shapes)
            {
                rawShapeData.y = PageHeight - rawShapeData.y;
            }
        }

        public void Load(string Filename, DrawingTypeDialog.DrawingConfigStruct DrawingConfig)
        {
           // PageWidth = 11.0f;
           // PageHeight = 11.0f;
           // DrillNodes = new List<DrillNode>();

            ReadNodes(Filename);

            RemoveZero();
            ConvertSmallSquaresToEllipses();
            RemoveNonEllipses();
            RemoveDuplicates();
            if (DrawingConfig.Inverted) FlipXShapes();
            if (DrawingConfig.vdx_vertical_flip) FlipYShapes(); 

            for (var i = 0; i < Shapes.Count; i++)
            {
                DrillNodes.Add(new DrillNode(new PointF(Shapes[i].x, Shapes[i].y)));
            }
        }

        private void ReadNodes(string filename)
        {
            Shapes = new List<rawShapeData>();
            try
            {
                var f = File.OpenText(filename);
                SeekShape(f);
                f.Close();
            }
            catch (IOException ex)
            {
                ExtLog.AddLine(ex.Message);
                DrillNodes.Clear();
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
                        PageHeight = SafeFloatParse(TrimString(l, "<PageHeight Unit='IN'>", "</PageHeight>"), PageHeight);
                    }
                    //<PageWidth Unit='IN'>8.5</PageWidth>
                    //<PageHeight Unit='IN'>11</PageHeight>
                }
            }

            ExtLog.AddLine($"{Shapes.Count} Shapes");
        }

        private rawShapeData readUntilEndOfShape(StreamReader reader)
        {
            var endFound = false;
            var newShape = new rawShapeData();
            while (!reader.EndOfStream & !endFound)
            {
                var l = reader.ReadLine();
                if (l != null)
                {
                    if (l == "</Shape>") endFound = true;
                    if (l.StartsWith("<Shape ID="))
                    {
                        Shapes.Add(readUntilEndOfShape(reader));
                    }

                    if (l.StartsWith("<PinX>"))
                    {
                        var pin = TrimString(l, "<PinX>", "</PinX>");
                        newShape.x = SafeFloatParse(pin, 0.0f);
                    }
                    if (l.StartsWith("<PinY>"))
                    {
                        var pin = TrimString(l, "<PinY>", "</PinY>");
                        newShape.y = SafeFloatParse(pin, 0.0f);

                    }
                    if (l.StartsWith("<Width>"))
                    {
                        var w = TrimString(l, "<Width>", "</Width>");
                        newShape.w = SafeFloatParse(w, 0.0f);

                    }
                    if (l.StartsWith("<Height>"))
                    {
                        var h = TrimString(l, "<Height>", "</Height>");
                        newShape.h = SafeFloatParse(h, 0.0f);

                    }
                    if (l.StartsWith("<Ellipse IX=")) newShape.isEllipse = true;
                }
                //012345678901234567
                //<PinX>4.125</PinX>
                //<PinY>9.875</PinY>
                //012345     0123456
                //<Width>0.05</Width>
                //<Height>0.049999999999997 </ Height >
            }
            return newShape;
        }

        private void ConvertSmallSquaresToEllipses() 
        {
            var numSq = 0;
            foreach (var s in Shapes)
            {
                if ((!s.isEllipse) && (Math.Abs(s.w - 0.050f) < GlobalProperties.NodeEpsilon) && (Math.Abs(s.h - 0.050f) < GlobalProperties.NodeEpsilon) ) 
                {

                    s.isEllipse = true;
                    numSq++;
                }
            }
            if (numSq > 0) ExtLog.AddLine($"Converted {numSq} square(s) to ellipe(s)");
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
