using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace CNC_Drill_Controller1
{
    class USB_Control_Emulator : IUSB_Controller
    {
        public byte[] InputBuffer { get; set; }
        public bool IsOpen {
            get { return true; }
        }
        public DateTime LastUpdate { get; set; }

        public bool MaxXswitch { get; set; }
        public bool MinXswitch { get; set; }
        public bool MaxYswitch { get; set; }
        public bool MinYswitch { get; set; }
        public bool TopSwitch { get; set; }
        public bool BottomSwitch { get; set; }

        public bool X_Driver { get; set; }
        public bool Y_Driver { get; set; }
        public bool T_Driver { get; set; }
        public bool Cycle_Drill { get; set; }

        public bool Inhibit_Backlash_Compensation { get; set; }
        public bool Inhibit_LimitSwitches_Warning { get; set; }

        public int X_Abs_Location { get; set; }
        public int Y_Abs_Location { get; set; }
        public int X_Delta { get; set; }
        public int Y_Delta { get; set; }
        public int X_Last_Direction { get; set; }
        public int Y_Last_Direction { get; set; }
        public int X_Rel_Location { get { return X_Abs_Location - Y_Delta; } }
        public int Y_Rel_Location { get { return Y_Abs_Location - Y_Delta; } }

        private int drilldelay;

        public ProgressEvent OnProgress { get; set; }
        private void UpdateProgress(int Progress, bool Done)
        {
            if (OnProgress != null) OnProgress(Progress, Done);
        }

        public List<string> GetDevicesList()
        {
            return new List<string>{"0000:Emulator"};
        }

        public bool OpenDeviceByLocation(uint LocationID)
        {
            return true;
        }

        public void Transfer()
        {
            Thread.Sleep(1);
            LastUpdate = DateTime.Now;

            if (drilldelay > 0)
            {
                drilldelay--;
            }
            if (drilldelay == 0)
            {
                TopSwitch = true;
            }
            if (Cycle_Drill)
            {
                TopSwitch = false;
                drilldelay = 10;
            }

            var pos = CurrentLocation();
            MinXswitch = (pos.X < 0.0f);
            MaxXswitch = (pos.X > 6.0f);
            MinYswitch = (pos.Y < 0.0f);
            MaxYswitch = (pos.Y > 6.0f);

        }

        public PointF CurrentLocation()
        {
            var current_X = ((float)X_Abs_Location - X_Delta) / GlobalProperties.X_Scale;
            var current_Y = ((float)Y_Abs_Location - Y_Delta) / GlobalProperties.Y_Scale;
            return new PointF(current_X, current_Y);
        }

        public bool Check_Limit_Switches()
        {
            return !MaxXswitch && !MinXswitch && !MaxYswitch && !MinYswitch;
        }

        public void MoveBy(int byX, int byY)
        {
            var abyX = Math.Abs(byX);
            var abyY = Math.Abs(byY);

            //process directions
            var XStepDirection = (byX == 0) ? 0 : byX / abyX;
            var YStepDirection = (byY == 0) ? 0 : byY / abyY;

            //process backlash
            X_Last_Direction = XStepDirection;
            Y_Last_Direction = YStepDirection;

            //process moves
            var numMoves = (abyX >= abyY) ? abyX : abyY;

            var stridex = 1.0f; //default maximum stride
            var stridey = 1.0f;

            if ((abyX != 0) || (abyY != 0)) //adjust stride
            {
                if (abyX > abyY)
                {
                    stridey = (float)abyY / abyX;
                }
                else if (abyY > abyX)
                {
                    stridex = (float)abyX / abyY;
                }
            }

            var fractx = 0.0f;
            var fracty = 0.0f;

            for (var i = 0; i < numMoves; i++)
            {
                fractx += stridex;
                fracty += stridey;

                if (fractx >= 0.5f)
                {
                    X_Abs_Location += XStepDirection;
                    fractx -= 1.0f;
                }

                if (fracty >= 0.5f)
                {
                    Y_Abs_Location += YStepDirection;
                    fracty -= 1.0f;
                }

                Transfer();
                UpdateProgress(100 * i / numMoves, false);
                if (MaxXswitch || MinXswitch || MaxYswitch ||
                    MinYswitch)
                {
                    if (!Inhibit_LimitSwitches_Warning) ExtLog.AddLine("Limit switch triggered before end of move");
                    numMoves = i; //exit loop
                }
            }

            UpdateProgress(100, true);
        }

        public void MoveTo(float X, float Y)
        {
            var current_pos = CurrentLocation();
            if (!MaxXswitch && !MinXswitch &&
                !MaxYswitch && !MinYswitch)
            {
                var deltaX = X - current_pos.X;
                var deltaY = Y - current_pos.Y;
                MoveBy((int)(deltaX * GlobalProperties.X_Scale), (int)(deltaY * GlobalProperties.Y_Scale));
            }
            else ExtLog.AddLine("Limit switch warning must be cleared before moving.");
        }

    }
}
