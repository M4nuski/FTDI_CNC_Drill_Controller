using System.Collections.Generic;

namespace CNC_Drill_Controller1
{
    interface INodeLoader
    {
        float NodeEpsilon { get; set; }

        float PageWidth { get; set; }
        float PageHeight { get; set; }
        List<DrillNode> DrillNodes { get; set; }

        void Load(string Filename, DrawingTypeDialog.DrawingConfigStruct DrawingConfig);
    }
}
