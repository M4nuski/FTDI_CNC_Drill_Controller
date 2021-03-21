using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CNC_Drill_Controller1
{
    public class DrillNode
    {
        public Color Color { get { return nodeStatusColors[status]; } }

        public enum DrillNodeStatus
        {
            Idle,
            Next,
            Drilled
        }

        public override string ToString()
        {
            return string.Format("{0,6:F3} {1,6:F3}", location.X, location.Y);
        }

        public PointF location;

        public DrillNodeStatus status;

        //public int ID;

        private Dictionary<DrillNodeStatus, Color> nodeStatusColors = new Dictionary<DrillNodeStatus, Color>
        {
            {DrillNodeStatus.Idle, Color.Black}, 
            {DrillNodeStatus.Next, Color.Red}, 
            {DrillNodeStatus.Drilled, Color.Yellow}            
        };

        static public Color nodeSelectedColor = Color.Blue;

        public int _originalIndex = -1;

        public DrillNode()
        {
            location = new PointF(0, 0);
            status = DrillNodeStatus.Idle;
        }

        public DrillNode(PointF Location)
        {
            location = Location;
            status = DrillNodeStatus.Idle;
        }
    }

    static class DrillNodeHelper
    {
        public static float OptimizationEpsilon = 0.020f;

        public static int X_Sort_Ascending_Predicate(DrillNode A, DrillNode B)
        {
            return (int)(10000 * (A.location.X - B.location.X));
        }
        public static int X_Sort_Descending_Predicate(DrillNode A, DrillNode B)
        {
            return (int)(10000 * (B.location.X - A.location.X));
        }
        public static int Y_Sort_Ascending_Predicate(DrillNode A, DrillNode B)
        {
            return (int)(10000 * (A.location.Y - B.location.Y));
        }
        public static int Y_Sort_Descending_Predicate(DrillNode A, DrillNode B)
        {
            return (int)(10000 * (B.location.Y - A.location.Y));
        }

        public static double EuclidianLength(PointF A, PointF B)
        {
            return Math.Sqrt(Sqr(A.X - B.X) + Sqr(A.Y - B.Y));
        }

        public static double CNCLength(PointF A, PointF B)
        {
            var x = Math.Abs(A.X - B.X);
            var y = Math.Abs(A.Y - B.Y);
            return Math.Max(x, y);
        }

        public static double Sqr(float X)
        {
            return X * X;
        }

        public static double getPathLength(List<DrillNode> nodes, PointF StartLocation)
        {
            var result = 0d;
            for (var i = 0; i < nodes.Count; i++)
            {
                result += CNCLength(StartLocation, nodes[i].location);
                StartLocation = nodes[i].location;
            }
            return result;
        }

        private static double[] GetNodeDistances(List<DrillNode> nodes, PointF Origin)
        {
            var result = new double[nodes.Count];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = EuclidianLength(Origin, nodes[i].location);
            }
            return result;
        }


        public static void ResetNodeStatus_Predicate(DrillNode x)
        {
            x.status = DrillNode.DrillNodeStatus.Idle;
        }

        public static List<DrillNode> OptimizeNodesNN(List<DrillNode> Nodes, PointF StartLocation)
        {
            if (Nodes.Count > 2)
            {
                ExtLog.AddLine("Optimizing node sequence (nearest neighbor)");

                var oldPathLength = getPathLength(Nodes, StartLocation);
                var oldNodesCount = Nodes.Count;

                var nodes = new List<DrillNode>(Nodes);
                var CurrentPosition = StartLocation;
                var newNodes = new List<DrillNode>();

                var dists = GetNodeDistances(nodes, CurrentPosition);
                var Closest = 0;
                for (var i = 0; i < dists.Length; i++)
                {
                    if (dists[i] < dists[Closest]) Closest = i;
                }
                newNodes.Add(nodes[Closest]);
                nodes.Remove(nodes[Closest]);

                for (var j = 0; j < oldNodesCount - 1; j++)
                {
                    CurrentPosition = newNodes[newNodes.Count - 1].location;
                    dists = GetNodeDistances(nodes, CurrentPosition);
                    Closest = 0;
                    for (var i = 0; i < dists.Length; i++)
                    {
                        if (dists[i] < dists[Closest]) Closest = i;
                    }
                    newNodes.Add(nodes[Closest]);
                    nodes.Remove(nodes[Closest]);
                }

                var newPathLength = getPathLength(newNodes, StartLocation);
                var newNodesCount = newNodes.Count;

                if (newPathLength <= oldPathLength)
                {
                    if (newNodesCount == oldNodesCount)
                    {
                        newNodes.ForEach(ResetNodeStatus_Predicate);

                        ExtLog.AddLine("Path of " + newPathLength.ToString("F4") + " / " + oldPathLength.ToString("F4"));
                        return newNodes;
                    }
                    ExtLog.AddLine("Optimize Failed (nodes count mismatch)");
                }
                else ExtLog.AddLine("Optimize Failed (new path length is longer)");
            }
            else ExtLog.AddLine("Not enough nodes to optimize path.");
            return Nodes;
        }

        public static List<DrillNode> OptimizeNodesHScanLine(List<DrillNode> Nodes, PointF StartLocation)
        {
            if (Nodes.Count > 2)
            {
                ExtLog.AddLine("Optimizing node sequence (H-scanlines)");
                var oldPathLength = getPathLength(Nodes, StartLocation);
                var oldNodesCount = Nodes.Count;

                var newYNodes = new List<DrillNode>(Nodes);
                newYNodes.Sort(Y_Sort_Ascending_Predicate);
                var outputNodes = new List<DrillNode>();
                var itteration = 0;
                for (var i = 0; i < newYNodes.Count; i++)
                {
                    var target_Y = newYNodes[i].location.Y;
                    var Y_list = newYNodes.Where(n => Math.Abs(n.location.Y - target_Y) < OptimizationEpsilon).ToList();

                    if ((itteration % 2) == 0)
                    {
                        Y_list.Sort(X_Sort_Ascending_Predicate);
                    }
                    else
                    {
                        Y_list.Sort(X_Sort_Descending_Predicate);
                    }

                    outputNodes.AddRange(Y_list);
                    i += Y_list.Count - 1;
                    itteration++;
                }

                if (outputNodes.Count == oldNodesCount)
                {
                    var Y_Length = getPathLength(outputNodes, StartLocation);
                    if (Y_Length < oldPathLength)
                    {
                        outputNodes.ForEach(ResetNodeStatus_Predicate);
                        ExtLog.AddLine("Path of " + Y_Length.ToString("F4") + " / " + oldPathLength.ToString("F4"));
                        return outputNodes;
                    }
                    ExtLog.AddLine("Optimize Failed (new path length is longer)");
                }
                else ExtLog.AddLine("Optimize Failed (nodes count mismatch)");
            }
            else ExtLog.AddLine("Not enough nodes to optimize path.");
            return Nodes;
        }

        public static List<DrillNode> OptimizeNodesVScanLine(List<DrillNode> Nodes, PointF StartLocation)
        {
            if (Nodes.Count > 2)
            {
                ExtLog.AddLine("Optimizing node sequence (V-scanlines)");
                var oldPathLength = getPathLength(Nodes, StartLocation);
                var oldNodesCount = Nodes.Count;

                var newXNodes = new List<DrillNode>(Nodes);
                newXNodes.Sort(X_Sort_Ascending_Predicate);
                var outputNodes = new List<DrillNode>();
                var itteration = 0;
                for (var i = 0; i < newXNodes.Count; i++)
                {
                    var target_X = newXNodes[i].location.X;
                    var X_list = newXNodes.Where(n => Math.Abs(n.location.X - target_X) < OptimizationEpsilon).ToList();

                    if ((itteration % 2) == 0)
                    {
                        X_list.Sort(Y_Sort_Ascending_Predicate);
                    }
                    else
                    {
                        X_list.Sort(Y_Sort_Descending_Predicate);
                    }

                    outputNodes.AddRange(X_list);
                    i += X_list.Count - 1;
                    itteration++;
                }

                if (outputNodes.Count == oldNodesCount)
                {
                    var X_Length = getPathLength(outputNodes, StartLocation);

                    if (X_Length < oldPathLength)
                    {
                        outputNodes.ForEach(ResetNodeStatus_Predicate);
                        ExtLog.AddLine("Path of " + X_Length.ToString("F4") + " / " + oldPathLength.ToString("F4"));
                        return outputNodes;
                    }
                    ExtLog.AddLine("Optimize Failed (new path length is longer)");

                }
                else ExtLog.AddLine("Optimize Failed (nodes count mismatch)");

            }
            else ExtLog.AddLine("Not enough nodes to optimize path.");
            return Nodes;
        }


    }
}
