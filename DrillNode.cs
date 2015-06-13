using System.Collections.Generic;
using System.Drawing;

namespace CNC_Drill_Controller1
{
    class DrillNode
    {
        public Color Color { get { return nodeStatusColors[status]; }}

        public enum DrillNodeStatus
        {
            Idle,
            Next,
            Drilled,
            Selected
        }

        public string Location {
            get { return location.X.ToString("F4") + " " + location.Y.ToString("F4"); }
        }

        public PointF location;

        public DrillNodeStatus status;

        public int ID;

        private Dictionary<DrillNodeStatus, Color> nodeStatusColors = new Dictionary<DrillNodeStatus, Color>
        {
            {DrillNodeStatus.Idle, Color.Black}, 
            {DrillNodeStatus.Next, Color.Red}, 
            {DrillNodeStatus.Drilled, Color.Yellow}, 
            {DrillNodeStatus.Selected, Color.Blue}, 
        };

        public DrillNode()
        {
            location = new PointF(0, 0);
            status = DrillNodeStatus.Idle;
            ID = -1;
        }

        public DrillNode(PointF Location, int ID)
        {
            location = Location;
            status = DrillNodeStatus.Idle;
            this.ID = ID;
        }
    }
}
