using System;
using System.Collections.Generic;
using System.Drawing;

namespace CNC_Drill_Controller1
{
    delegate void ProgressEvent(int Progress, bool Done);

    interface IUSB_Controller
    {
        byte[] InputBuffer { get; set; }
        bool IsOpen { get; }
        DateTime LastUpdate { get; set; }

        bool MaxXswitch { get; }
        bool MinXswitch { get; }
        bool MaxYswitch { get; }
        bool MinYswitch { get; }
        bool TopSwitch { get; }
        bool BottomSwitch { get; }

        bool X_Driver { get; set; }
        bool Y_Driver { get; set; }
        bool T_Driver { get; set; }
        bool Cycle_Drill { get; set; }

        bool Inhibit_Backlash_Compensation { get; set; }
        bool Inhibit_LimitSwitches_Warning { get; set; }

        int X_Abs_Location { get; set; }
        int Y_Abs_Location { get; set; }
        int X_Delta { get; set; }
        int Y_Delta { get; set; }
        int X_Last_Direction { get; set; }
        int Y_Last_Direction { get; set; }
        int X_Rel_Location { get; }
        int Y_Rel_Location { get; }

        ProgressEvent OnProgress { get; set; }

        List<string> GetDevicesList();
        bool OpenDeviceByLocation(uint LocationID);

        void Transfer();

        PointF CurrentLocation();

        void MoveBy(int byX, int byY);
        void MoveTo(float X, float Y);

    }
}
