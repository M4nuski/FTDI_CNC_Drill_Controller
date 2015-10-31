using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.Threading;

namespace CNC_Drill_Controller1
{
    class USB_Control_Emulator : IUSB_Controller
    {
        public byte[] InputBuffer { get; set; }
        private  byte[] obuf = new byte[64];
        public bool IsOpen {
            get { return true; }
        }
        public DateTime LastUpdate { get; set; }
        private bool cancelJob;

        public bool MaxXswitch { get; set; }
        public bool MinXswitch { get; set; }
        public bool MaxYswitch { get; set; }
        public bool MinYswitch { get; set; }
        public bool TopSwitch { get; set; }
        public bool BottomSwitch { get; set; }

        public bool X_Driver { get; set; }
        public bool Y_Driver { get; set; }
        public bool TQA_Driver { get; set; }
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

        public ProgressDelegate OnProgress { get; set; }
        public MoveDelegate OnMove { get; set; }

        public USB_Control_Emulator() 
        {
            InputBuffer  = new byte[64];

            X_Abs_Location = GlobalProperties.X_Pos;
            Y_Abs_Location = GlobalProperties.Y_Pos;
            X_Delta = GlobalProperties.X_Delta;
            Y_Delta = GlobalProperties.Y_Delta;
            X_Last_Direction = GlobalProperties.X_Dir;
            Y_Last_Direction = GlobalProperties.Y_Dir;
        }

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
            SignalGenerator.OutputByte0 = CreateStepByte();
            SignalGenerator.OutputByte1 = CreateControlByte();
            SignalGenerator.Serialize(ref obuf);
            InputBuffer = obuf;
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

        private byte CreateStepByte()
        {
            var x = (byte)(X_Abs_Location & GlobalProperties.numStepMask);
            var y = (byte)(Y_Abs_Location & GlobalProperties.numStepMask);
            return (byte)((GlobalProperties.stepBytes[x] & 0x0F) | (GlobalProperties.stepBytes[y] & 0xF0));
        }

        private byte CreateControlByte()
        {
            var TQA_X_Pos = (X_Last_Direction == 1);
            var TQA_X_Neg = (X_Last_Direction == -1);
            var TQA_Y_Pos = (Y_Last_Direction == 1);
            var TQA_Y_Neg = (Y_Last_Direction == -1);

            byte output = 0;
            if (X_Driver) output = (byte)(output | 1);
            if (Y_Driver) output = (byte)(output | 2);
            if (Cycle_Drill) output = (byte)(output | 4);
            if (TQA_Driver) output = (byte)(output | 8);

            if (TQA_X_Pos) output = (byte)(output | 16);
            if (TQA_X_Neg) output = (byte)(output | 32);
            if (TQA_Y_Pos) output = (byte)(output | 64);
            if (TQA_Y_Neg) output = (byte)(output | 128);

            return output;
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

        public void CancelMove()
        {
            cancelJob = true;
        }

        private bool jobCancelled()
        {
            return cancelJob;
        }

        public bool MoveBy(int byX, int byY)
        {
            cancelJob = false;
            var success = true;

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

                if (!Check_Limit_Switches())
                {
                    if (!Inhibit_LimitSwitches_Warning) ExtLog.AddLine("Limit switch triggered before end of move");
                    i = numMoves; //exit loop
                    success = false;
                }

                if (jobCancelled())
                {
                    ExtLog.AddLine("Move Cancelled");
                    i = numMoves; //exit loop
                    success = false;
                }
            }

            UpdateProgress(100, true);
            return success;
        }

        public bool MoveTo(float X, float Y)
        {
            var success = false;
            var current_pos = CurrentLocation();
            if (!MaxXswitch && !MinXswitch &&
                !MaxYswitch && !MinYswitch)
            {
                var deltaX = X - current_pos.X;
                var deltaY = Y - current_pos.Y;
                if (OnMove != null) OnMove(X, Y);
                success = MoveBy((int)(deltaX * GlobalProperties.X_Scale), (int)(deltaY * GlobalProperties.Y_Scale));
            }
            else ExtLog.AddLine("Limit switch warning must be cleared before moving.");
            return success;
        }

    }
}
