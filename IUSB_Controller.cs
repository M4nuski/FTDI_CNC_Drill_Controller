using System;
using System.Collections.Generic;
using System.Drawing;

namespace CNC_Drill_Controller1
{
    interface IUSB_Controller
    {
        //Useful internals exposure
        byte[] InputBuffer { get; set; }
        bool IsOpen { get; }
        DateTime LastUpdate { get; set; }

        //Callback
        ProgressDelegate OnProgress { get; set; }
        MoveDelegate OnMove { get; set; }
        Action OnMoveCompleted { get; set; }

        //Input from CNC
        bool MaxXswitch { get; }
        bool MinXswitch { get; }
        bool MaxYswitch { get; }
        bool MinYswitch { get; }
        bool TopSwitch { get; }
        bool BottomSwitch { get; }

        //Output to CNC
        bool X_Driver { get; set; }
        bool Y_Driver { get; set; }
        bool TQA_Driver { get; set; }
        bool Cycle_Drill { get; set; }

        //Behaviour Modifier of Controller
        bool Inhibit_Backlash_Compensation { get; set; }
        bool Inhibit_LimitSwitches_Warning { get; set; }

        //Hardware State of CNC
        int X_Abs_Location { get; set; }
        int Y_Abs_Location { get; set; }
        int X_Delta { get; set; }
        int Y_Delta { get; set; }
        int X_Last_Direction { get; set; }
        int Y_Last_Direction { get; set; }
        int X_Rel_Location { get; }
        int Y_Rel_Location { get; }

        //Setup
        List<string> GetDevicesList();
        bool OpenDeviceByLocation(uint LocationID);
        void CloseDevice();

        //Helpers
        PointF CurrentLocation();
        bool Check_Limit_Switches();

        //Movements and Updates
        void CancelMove();
        void Transfer();
        bool MoveBy(int byX, int byY);
        bool MoveTo(float X, float Y);
    }
}
