using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Svg;

namespace CNC_Drill_Controller1
{
    internal class SVGLoader : INodeLoader
    {
        public float PageWidth { get; set; }
        public float PageHeight { get; set; }
        public List<DrillNode> DrillNodes { get; set; }

        private static void applyParentsTransforms(PointF[] pt, SvgElement node)
        {
            node.Transforms.GetMatrix().TransformPoints(pt);
            if (node.Parent != null) applyParentsTransforms(pt, node.Parent);
        }

        private void readCircles(string filename)
        {
            var svgReader = SvgDocument.Open(filename);

            PageWidth = svgReader.Width.Value;
            PageHeight = svgReader.Height.Value;

            var circlesEnum = svgReader.Children.FindSvgElementsOf<SvgCircle>();
            var circlesList = circlesEnum as IList<SvgCircle> ?? circlesEnum.ToList();

            for (var i = 0; i < circlesList.Count; i++)
            {
                var pt = new[] { new PointF(circlesList[i].CenterX.Value, circlesList[i].CenterY.Value) };

                applyParentsTransforms(pt, circlesList[i].Parent);

                pt[0].X /= 72;//svgReader.Ppi;
                pt[0].Y /= 72;//svgReader.Ppi;

                DrillNodes.Add(new DrillNode(pt[0]));
            }

            ExtLog.AddLine(DrillNodes.Count.ToString("D") + " Shapes");
        }


        public void Load(string Filename, DrawingTypeDialog.DrawingConfigStruct DrawingConfig)
        {
            readCircles(Filename);
            removeZeros();
            removeDuplicates();
            if (DrawingConfig.Inverted) flipNodes();
        }

        private void removeZeros()
        {
            DrillNodes.RemoveAll(
                dn => (Math.Sqrt(Math.Pow(dn.location.X, 2) + Math.Pow(dn.location.Y, 2)) < GlobalProperties.NodeEpsilon));

            ExtLog.AddLine(DrillNodes.Count.ToString("D") + " Non-Zeros");
        }
        private void removeDuplicates()
        {
            var duplicates = new bool[DrillNodes.Count];

            for (var i = 0; i < DrillNodes.Count; i++)
            {
                if (!duplicates[i])
                    for (var j = 0; j < DrillNodes.Count; j++)
                    {
                        if ((i != j) && (!duplicates[j]) && (sameLocation(DrillNodes[i], DrillNodes[j]))) duplicates[j] = true;
                    }
            }

            for (var i = duplicates.Length - 1; i >= 0; i--)
            {
                if (duplicates[i]) DrillNodes.RemoveAt(i);
            }

            ExtLog.AddLine(DrillNodes.Count.ToString("D") + " Uniques");
        }

        private bool sameLocation(DrillNode drillNode1, DrillNode drillNode2)
        {
            return ((Math.Sqrt(Math.Pow(drillNode1.location.X - drillNode2.location.X, 2) + Math.Pow(drillNode1.location.Y - drillNode2.location.Y, 2))) < GlobalProperties.NodeEpsilon);
        }

        private void flipNodes()
        {
            for (var i = 0; i < DrillNodes.Count; i++)
            {
                DrillNodes[i].location.X = PageWidth - DrillNodes[i].location.X;
            }
        }

    }
}
