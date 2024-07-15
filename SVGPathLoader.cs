using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Svg;
using System.Drawing;
using System.IO;

namespace CNC_Drill_Controller1
{


    class SVGPathLoader
    {
        public class seg
        {
            public List<PointF> pstart = new List<PointF>();
            public List<PointF> pend = new List<PointF>();
        }
        private static void applyParentsTransforms(PointF[] pt, SvgElement node)
        {
            node.Transforms.GetMatrix().TransformPoints(pt);
            if (node.Parent != null) applyParentsTransforms(pt, node.Parent);
        }

        public List<seg> loadSVGPaths(string filename)
        {

            var svgReader = SvgDocument.Open(filename);

            //PageWidth = svgReader.Width.Value;
            //PageHeight = svgReader.Height.Value;

            var circlesEnum = svgReader.Children.FindSvgElementsOf<SvgPath>();
            var circlesList = circlesEnum as IList<SvgPath> ?? circlesEnum.ToList();

            var r = new List<seg>();

            for (var i = 0; i < circlesList.Count; i++)
            {
                seg s = new seg();

               // var d = circlesList[i].Path(new SvgColourServer(Color.DarkGreen));

                var pts = new PointF[circlesList[i].PathData.Count];
                var pte = new PointF[circlesList[i].PathData.Count];

                for (var j = 0; j < circlesList[i].PathData.Count; ++j)
                {
                    pts[j].X = circlesList[i].PathData[j].Start.X;
                    pts[j].Y = circlesList[i].PathData[j].Start.Y;

                    pte[j].X = circlesList[i].PathData[j].End.X;
                    pte[j].Y = circlesList[i].PathData[j].End.Y;
                }

                applyParentsTransforms(pts, circlesList[i].Parent);
                applyParentsTransforms(pte, circlesList[i].Parent);

                for (var k = 0; k < pts.Length; ++k)
                {
                    pts[k].X /= svgReader.Ppi;
                    pts[k].Y /= svgReader.Ppi;
                    pte[k].X /= svgReader.Ppi;
                    pte[k].Y /= svgReader.Ppi;
                }

                s.pstart.AddRange(pts);
                s.pend.AddRange(pte);

                for (var k = s.pstart.Count-1; k >= 0; --k)
                {
                    if ((Math.Abs(s.pstart[k].X - s.pend[k].X) <= GlobalProperties.NodeEpsilon) && (Math.Abs(s.pstart[k].Y - s.pend[k].Y) <= GlobalProperties.NodeEpsilon)) {
                        s.pstart.RemoveAt(k);
                        s.pend.RemoveAt(k);
                    }

                }  

                r.Add(s);
            }
            return r;
        }
               // ExtLog.AddLine(DrillNodes.Count.ToString("D") + " Shapes");
    }

}
