using System;
using System.Collections.Generic;
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

        public bool X_StepMotor_Driver_Enable { get; set; }
        public bool Y_StepMotor_Driver_Enable { get; set; }
        public bool TQA_Driver_Enable { get; set; }
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

        public Void_IntBoolDelegate OnProgress { get; set; }
        public Bool_FloatFloatDelegate OnMove { get; set; }
        public Action OnMoveCompleted { get; set; }

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

        public void CloseDevice()
        {
            
        }

        public void Transfer()
        {
            SignalGenerator.OutputByte0 = CreateXAxisByte();
            SignalGenerator.OutputByte1 = CreateYAxisByte();
            SignalGenerator.OutputByte2 = CreateDrillByte();

            SignalGenerator.Serialize(ref obuf);
            InputBuffer = obuf;
            Thread.Sleep(GlobalProperties.latency);
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

        private byte CreateXAxisByte()
        {
            var x = (byte)(X_Abs_Location & GlobalProperties.numStepMask);
            x = GlobalProperties.stepBytes[x]; //bits 0-3
            x = SignalGenerator.SetBit(x, GlobalProperties.StepMotor_Enable_Bit, X_StepMotor_Driver_Enable); //bit4

            x = SignalGenerator.SetBit(x, GlobalProperties.Torque_Pos_Bit, (X_Last_Direction == 1)); //bit5
            x = SignalGenerator.SetBit(x, GlobalProperties.Torque_Neg_Bit, (X_Last_Direction == -1)); //bit6
            x = SignalGenerator.SetBit(x, GlobalProperties.Torque_Enable_Bit, TQA_Driver_Enable); //bit7
            return x;
        }

        private byte CreateYAxisByte()
        {
            var y = (byte)(Y_Abs_Location & GlobalProperties.numStepMask);
            y = GlobalProperties.stepBytes[y]; //bits 0-3
            y = SignalGenerator.SetBit(y, GlobalProperties.StepMotor_Enable_Bit, Y_StepMotor_Driver_Enable); //bit4

            y = SignalGenerator.SetBit(y, GlobalProperties.Torque_Pos_Bit, (Y_Last_Direction == 1)); //bit5
            y = SignalGenerator.SetBit(y, GlobalProperties.Torque_Neg_Bit, (Y_Last_Direction == -1)); //bit6
            y = SignalGenerator.SetBit(y, GlobalProperties.Torque_Enable_Bit, TQA_Driver_Enable); //bit7
            return y;
        }

        private byte CreateDrillByte()
        {
            var d = SignalGenerator.SetBit(0, GlobalProperties.Drill_Cycle_Enable_Bit, Cycle_Drill);
            //d = SignalGenerator.SetBit(0, GlobalProperties.StepMotor_Throttle_Bit, Axis_Driver_Throttle);
            return d;
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

        public bool MoveByStep(int dX, int dY)
        {
            cancelJob = false;
            var success = true;

            var absDX = Math.Abs(dX);
            var absDY = Math.Abs(dY);

            //process directions
            var XStepDirection = (dX == 0) ? 0 : dX / absDX;
            var YStepDirection = (dY == 0) ? 0 : dY / absDY;

            //process backlash
            X_Last_Direction = XStepDirection;
            Y_Last_Direction = YStepDirection;

            //process moves
            var numMoves = (absDX >= absDY) ? absDX : absDY;

            var stridex = 1.0f; //default maximum stride
            var stridey = 1.0f;

            if ((absDX != 0) || (absDY != 0)) //adjust stride
            {
                if (absDX > absDY)
                {
                    stridey = (float)absDY / absDX;
                }
                else if (absDY > absDX)
                {
                    stridex = (float)absDX / absDY;
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


        public bool MoveByPosition(float dX, float dY)
        {
            if (!MaxXswitch && !MinXswitch && !MaxYswitch && !MinYswitch)
            {
                OnMove?.Invoke(dX, dY);
                var success = MoveByStep((int)Math.Round(dX * GlobalProperties.X_Scale), (int)Math.Round(dY * GlobalProperties.Y_Scale));
                OnMoveCompleted?.Invoke();
                return success;
            }
            else
            {
                ExtLog.AddLine("Limit switch warning must be cleared before moving.");
                return false;
            }
        }

        public bool MoveToPosition(float X, float Y)
        {
            var current_pos = CurrentLocation();
            if (!MaxXswitch && !MinXswitch && !MaxYswitch && !MinYswitch)
            {
                var dX = X - current_pos.X;
                var dY = Y - current_pos.Y;
                OnMove?.Invoke(X, Y);
                var success = MoveByStep((int)Math.Round(dX * GlobalProperties.X_Scale), (int)Math.Round(dY * GlobalProperties.Y_Scale));
                OnMoveCompleted?.Invoke();
                return success;
            }
            else
            {
                ExtLog.AddLine("Limit switch warning must be cleared before moving.");
                return false;
            }
        }
        public bool MoveToStep(int X, int Y)
        {
            if (!MaxXswitch && !MinXswitch && !MaxYswitch && !MinYswitch)
            {
                OnMove?.Invoke(X, Y);
                var success = MoveByStep(X - X_Rel_Location, Y - Y_Rel_Location);
                OnMoveCompleted?.Invoke();
                return success;
            }
            else
            {
                ExtLog.AddLine("Limit switch warning must be cleared before moving.");
                return false;
            }
        }

    }
}
