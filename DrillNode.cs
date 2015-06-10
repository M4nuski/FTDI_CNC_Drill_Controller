using System.Collections.Generic;
using System.Drawing;

namespace CNC_Drill_Controller1
{
    class DrillNode
    {
        public enum DrillNodeStatus
        {
            Idle,
            Next,
            Drilled,
            Selected
        }

        public PointF location;

        public DrillNodeStatus status;

        private Dictionary<DrillNodeStatus, Color> nodeStatusColors = new Dictionary<DrillNodeStatus, Color>()
        {
            {DrillNodeStatus.Idle, Color.Black}, 
            {DrillNodeStatus.Next, Color.Red}, 
            {DrillNodeStatus.Drilled, Color.DimGray}, 
            {DrillNodeStatus.Selected, Color.Blue}, 
        };

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

        public Color color()
        {
            return nodeStatusColors[status];
        }
    }
}
