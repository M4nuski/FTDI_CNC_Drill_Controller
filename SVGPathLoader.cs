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

        private float dist(PointF a, PointF b)
        {
            return (float)Math.Sqrt(((a.X - b.X) * (a.X - b.X)) + ((a.Y - b.Y) * (a.Y - b.Y)));
        }
        private float dist(float aX, float aY, float bX, float bY)
        {
            return (float)Math.Sqrt(((aX - bX) * (aX - bX)) + ((aY - bY) * (aY - bY)));
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

                var pts = new List<PointF>();// new PointF[circlesList[i].PathData.Count*100];
                var pte = new List<PointF>();//new PointF[circlesList[i].PathData.Count*100];
                var cx = 0.0f;
                var cy = 0.0f;
                var fcx = 0.0f;
                var fcy = 0.0f;
                var segCount = 0;
              //  for (var j = 0; j < circlesList[i].PathData.Count; ++j)
              //  {

                foreach (var pds in circlesList[i].PathData)
                {
                    var pstr = pds.ToString();
                    if (pstr[0] == 'M')
                    {
                        var mi = pstr.IndexOf(' ');
                        cx = float.Parse(pstr.Substring(1, mi));
                        cy = float.Parse(pstr.Substring(mi+1));
                        fcx = cx;
                        fcy = cy;
                    }
                    if (pstr[0] == 'm')
                    {
                        var mi = pstr.IndexOf(' ');
                        cx += float.Parse(pstr.Substring(1, mi));
                        cy += float.Parse(pstr.Substring(mi + 1));
                        fcx = cx;
                        fcy = cy;
                    }
                    if (pstr[0] == 'L')
                    {
                        var mi = pstr.IndexOf(' ');
                        var ncx = float.Parse(pstr.Substring(1, mi));
                        var ncy = float.Parse(pstr.Substring(mi + 1));

                        pts.Add(new PointF(cx, cy));
                        pte.Add(new PointF(ncx, ncy));
                        
                        cx = ncx;
                        cy = ncy;
                        segCount++;
                    }
                    if ( pstr.ToUpperInvariant()[0] == 'Z')
                    {
                        pts.Add(new PointF(cx, cy));
                        pte.Add(new PointF(fcx, fcy));
                        segCount++;
                    }
                    if (pstr.ToUpperInvariant()[0] == 'C') // absolute cubic bezier
                    {
                        var abmove = pstr[0] == 'C';

                        var mi = pstr.IndexOf(' ');
                        var p1x = float.Parse(pstr.Substring(1, mi));
                        pstr = pstr.Substring(mi + 1);
                        mi = pstr.IndexOf(' ');
                        var p1y = float.Parse(pstr.Substring(0, mi));
                        pstr = pstr.Substring(mi + 1);

                        mi = pstr.IndexOf(' ');
                        var p2x = float.Parse(pstr.Substring(0, mi));
                        pstr = pstr.Substring(mi + 1);
                        mi = pstr.IndexOf(' ');
                        var p2y = float.Parse(pstr.Substring(0, mi));
                        pstr = pstr.Substring(mi + 1);

                        mi = pstr.IndexOf(' ');
                        var p3x = float.Parse(pstr.Substring(0, mi));
                        pstr = pstr.Substring(mi + 1);
                     //   mi = pstr.IndexOf(' ');
                        var p3y = float.Parse(pstr);

                        if (!abmove)
                        {
                            p1x += cx;
                            p1y += cy;
                            p2x += cx;
                            p2y += cy;
                            p3x += cx;
                            p3y += cy;
                        }

                        var d = dist(cx, cy, p3x, p3y);
                        var minstep = 0.2f * svgReader.Ppi;
                        if (d > minstep)
                        {
                            var chunks = (int)Math.Ceiling(d / minstep);
                            var incre = 1.0f / chunks;
                            for (var t = 0.0f; t < 1.0f; t += incre)
                            {
                                //Pt = (1 - t) ^ 3 * P0 + 3(1 - t) ^ 2 * t * P1 + 3(1 - t) * t ^ 2 * P2 + t ^ 3 * P3
                                var ncx = (float)((cx * Math.Pow(1.0f - t, 3)) + (3 * Math.Pow(1.0f - t, 2) * t * p1x) + (3 * (1.0f - t) * t * t * p2x) + (Math.Pow(t, 3) * p3x));
                                var ncy = (float)((cy * Math.Pow(1.0f - t, 3)) + (3 * Math.Pow(1.0f - t, 2) * t * p1y) + (3 * (1.0f - t) * t * t * p2y) + (Math.Pow(t, 3) * p3y));
                                pts.Add(new PointF(cx, cy));
                                pte.Add(new PointF(ncx, ncy));
                                cx = ncx;
                                cy = ncy;
                                segCount++;
                            }
                        } else
                        {
                            pts.Add(new PointF(cx, cy));
                            pte.Add(new PointF(p3x, p3y));
                            cx = p3x;
                            cy = p3y;
                            segCount++;
                        }
                    }
                }



                //  pts[j].X = circlesList[i].PathData[j]
                //    pts[j].X = circlesList[i].PathData[j].Start.X;
                //    pts[j].Y = circlesList[i].PathData[j].Start.Y;

                //   pte[j].X = circlesList[i].PathData[j].End.X;
                //   pte[j].Y = circlesList[i].PathData[j].End.Y;
                // }

                var ptsa = pts.ToArray();
                var ptea = pte.ToArray();
                applyParentsTransforms(ptsa, circlesList[i].Parent);
                applyParentsTransforms(ptea, circlesList[i].Parent);

                for (var k = 0; k < ptsa.Length; ++k)
                {
                    ptsa[k].X /= svgReader.Ppi;
                    ptsa[k].Y /= svgReader.Ppi;
                    ptea[k].X /= svgReader.Ppi;
                    ptea[k].Y /= svgReader.Ppi;
                }

                s.pstart.AddRange(ptsa);
                s.pend.AddRange(ptea);

               // for (var k = s.pstart.Count-1; k >= 0; --k)
              //  {
               //     if ((Math.Abs(s.pstart[k].X - s.pend[k].X) <= GlobalProperties.NodeEpsilon) && (Math.Abs(s.pstart[k].Y - s.pend[k].Y) <= GlobalProperties.NodeEpsilon)) {
                //        s.pstart.RemoveAt(k);
                //        s.pend.RemoveAt(k);
                //    }
               // }  

                r.Add(s);
            }
            ExtLog.AddLine($"Loaded { r.Sum((s) => s.pstart.Count) } segments");

            float dsum = 0.0f;
            for (var i = 0; i < r.Count-1; ++i) dsum += dist(r[i].pend.Last(), r[i + 1].pstart.First());
            ExtLog.AddLine($"interpath travel distance: {dsum}");
            
            for (var i = 0; i < r.Count - 1; ++i)
            {
                var bestindex = i + 1;
                var bestdist = float.PositiveInfinity;
                for (var j = i+1; j < r.Count; ++j)
                {
                    var newdist = dist(r[i].pend.Last(), r[j].pstart.First());
                    if (newdist < bestdist)
                    {
                        bestdist = newdist;
                        bestindex = j;
                    }
                }
                if (bestindex != (i+1))
                {
                    r.Insert(i+1, r[bestindex]);
                    r.RemoveAt(bestindex+1);
                }
            }

            dsum = 0.0f;
            for (var i = 0; i < r.Count - 1; ++i) dsum += dist(r[i].pend.Last(), r[i + 1].pstart.First());
            ExtLog.AddLine($"interpath travel distance after opt: {dsum}");
            ExtLog.AddLine($"Optimized { r.Sum((s) => s.pstart.Count) } segments");
            
            return r;
        }
          
    }

}
