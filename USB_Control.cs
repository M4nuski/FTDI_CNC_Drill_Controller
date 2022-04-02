using System;
using System.Collections.Generic;
using System.Drawing;
using FTD2XX_NET;

namespace CNC_Drill_Controller1
{
    class USB_Control : IUSB_Controller
    {
        private FTDI USB_Interface = new FTDI();
        private byte[] OutputBuffer = new byte[64];
        private bool cancelJob;
        public byte[] InputBuffer { get; set; }
        private int dataSize;

        public bool IsOpen { get { return USB_Interface.IsOpen; } }
        public DateTime LastUpdate { get; set; }

        public bool Inhibit_Backlash_Compensation{ get; set; }
        public bool Inhibit_LimitSwitches_Warning{ get; set; }

        public bool MaxXswitch { get; set; }
        public bool MinXswitch { get; set; }
        public bool MaxYswitch { get; set; }
        public bool MinYswitch { get; set; }
        public bool TopSwitch { get; set; }
        public bool BottomSwitch { get; set; }

        public bool X_StepMotor_Driver_Enable { get; set; }
        public bool Y_StepMotor_Driver_Enable { get; set; }
        private bool TQA_Driver_Bit;
        public bool TQA_Driver_Enable { get; set; }
        public bool Cycle_Drill { get; set; }

        public int X_Abs_Location{ get; set; }
        public int Y_Abs_Location { get; set; }
        public int X_Delta{ get; set; }
        public int Y_Delta{ get; set; }
        public int X_Last_Direction { get; set; }
        public int Y_Last_Direction{ get; set; }
        public int X_Rel_Location { get { return X_Abs_Location - X_Delta; } }
        public int Y_Rel_Location { get { return Y_Abs_Location - Y_Delta; } }
        
        public Void_IntBoolDelegate OnProgress { get; set; }
        public Bool_FloatFloatDelegate OnMove { get; set; }
        public Action OnMoveCompleted { get; set; }
            
        private void UpdateProgress(int Progress, bool Done)
        {
            OnProgress?.Invoke(Progress, Done);
        }

        public USB_Control() 
        {
            InputBuffer  = new byte[64];

            X_Abs_Location = GlobalProperties.X_Pos;
            Y_Abs_Location = GlobalProperties.Y_Pos;
            X_Delta = GlobalProperties.X_Delta;
            Y_Delta = GlobalProperties.Y_Delta;
            X_Last_Direction = GlobalProperties.X_Dir;
            Y_Last_Direction = GlobalProperties.Y_Dir;
        }

        public List<string> GetDevicesList()
        {
            var result = new List<string>();

            uint numUI = 0;
            var ftStatus = USB_Interface.GetNumberOfDevices(ref numUI);
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {
                if (numUI > 0)
                {
                    ExtLog.AddLine(numUI.ToString("D") + " Devices Found.");

                    var ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[numUI];
                    ftStatus = USB_Interface.GetDeviceList(ftdiDeviceList);
                    if (ftStatus == FTDI.FT_STATUS.FT_OK)
                    {
                        for (var i = 0; i < numUI; i++)
                        {
                            result.Add(ftdiDeviceList[i].LocId.ToString("D4") + ":" + ftdiDeviceList[i].Description);
                        }
                    }
                    else
                    {
                        ExtLog.AddLine("Failed to list devices (error " + ftStatus + ")");
                    }
                }
                else ExtLog.AddLine("No devices found.");
            }
            else
            {
                ExtLog.AddLine("Failed to get number of devices (error " + ftStatus + ")");
            }
            return result;
        }

        public bool OpenDeviceByLocation(uint LocationID)
        {
            if (USB_Interface.IsOpen) USB_Interface.Close();

            ExtLog.AddLine("Opening device");
            var ftStatus = USB_Interface.OpenByLocation(LocationID);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                ExtLog.AddLine("Failed to open device (error " + ftStatus + ")");
                return false;
            }

            ExtLog.AddLine("Setting default bauld rate (" + GlobalProperties.baudRate + ")");
            ftStatus = USB_Interface.SetBaudRate(GlobalProperties.baudRate);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                ExtLog.AddLine("Failed to set Baud rate (error " + ftStatus + ")");
                return false;
            }

            ExtLog.AddLine("Setting BitMode");
            ftStatus = USB_Interface.SetBitMode(GlobalProperties.portDirectionMask, FTDI.FT_BIT_MODES.FT_BIT_MODE_SYNC_BITBANG);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                ExtLog.AddLine("Failed to set BitMode (error " + ftStatus + ")");
                return false;
            }

            ExtLog.AddLine($"Setting Latency to {GlobalProperties.latency}");
            ftStatus = USB_Interface.SetLatency(GlobalProperties.latency);
                if (ftStatus != FTDI.FT_STATUS.FT_OK)
                {
                    ExtLog.AddLine("Failed to set Latency (error " + ftStatus + ")");
                    return false;
                }
        

            UpdateProgress(100, true);
            return true;
        }

        public void CloseDevice()
        {
            if (USB_Interface.IsOpen) USB_Interface.Close();
        }

        public void Transfer()
        {
            if (USB_Interface.IsOpen)
            {
                SignalGenerator.OutputByte0 = CreateXAxisByte();
                SignalGenerator.OutputByte1 = CreateYAxisByte();
                SignalGenerator.OutputByte2 = CreateDrillByte();

                dataSize = SignalGenerator.Serialize(ref OutputBuffer);

                if (SendToUSB())
                {
                    SignalGenerator.Deserialize(InputBuffer);
                    ReadSwitches();
                    LastUpdate = DateTime.Now;
                }
            }
        }

        private bool SendToUSB()
        {
            var result = true;
            uint res = 0;
            var ftStatus = USB_Interface.Write(OutputBuffer, dataSize, ref res);
            if ((ftStatus != FTDI.FT_STATUS.FT_OK) && (res != dataSize))
            {
                USB_Interface.Close();
                ExtLog.AddLine(string.Format("Failed to write data (error {0}) ({1}/{2})", ftStatus, res, dataSize));
                result = false;
            }
            else
            {
                ftStatus = USB_Interface.Read(InputBuffer, (uint)dataSize, ref res);
                if ((ftStatus != FTDI.FT_STATUS.FT_OK) && (res != dataSize))
                {
                    USB_Interface.Close();
                    ExtLog.AddLine(string.Format("Failed to write data (error {0}) ({1}/{2})", ftStatus, res, dataSize));
                    result = false;
                }
            }
            return result;
        }

        private byte CreateXAxisByte()
        {
            var x = (byte)(X_Abs_Location & GlobalProperties.numStepMask);
            x = GlobalProperties.stepBytes[x]; //bits 0-3
            x = SignalGenerator.SetBit(x, GlobalProperties.StepMotor_Enable_Bit, X_StepMotor_Driver_Enable); //bit4

            x = SignalGenerator.SetBit(x, GlobalProperties.Torque_Pos_Bit, (X_Last_Direction == 1)); //bit5
            x = SignalGenerator.SetBit(x, GlobalProperties.Torque_Neg_Bit, (X_Last_Direction == -1)); //bit6
            x = SignalGenerator.SetBit(x, GlobalProperties.Torque_Enable_Bit, TQA_Driver_Bit); //bit7
            return x;
        }

        private byte CreateYAxisByte()
        {
            var y = (byte)(Y_Abs_Location & GlobalProperties.numStepMask);
            y = GlobalProperties.stepBytes[y]; //bits 0-3
            y = SignalGenerator.SetBit(y, GlobalProperties.StepMotor_Enable_Bit, Y_StepMotor_Driver_Enable); //bit4

            y = SignalGenerator.SetBit(y, GlobalProperties.Torque_Pos_Bit, (Y_Last_Direction == 1)); //bit5
            y = SignalGenerator.SetBit(y, GlobalProperties.Torque_Neg_Bit, (Y_Last_Direction == -1)); //bit6
            y = SignalGenerator.SetBit(y, GlobalProperties.Torque_Enable_Bit, TQA_Driver_Bit); //bit7
            return y;
        }

        private byte CreateDrillByte()
        {
            var d = SignalGenerator.SetBit(0, GlobalProperties.Drill_Cycle_Enable_Bit, Cycle_Drill);
            d = SignalGenerator.SetBit(d, GlobalProperties.Drill_Cycle_Enable_Bit, false);
            //d = SignalGenerator.SetBit(d, GlobalProperties.StepMotor_Throttle_Bit, Axis_Driver_Throttle);
            return d;
        }

        private void ReadSwitches()
        {
            MinXswitch = !SignalGenerator.GetBit(SignalGenerator.InputByte0, GlobalProperties.X_MinSwitch_Bit);
            MaxXswitch = !SignalGenerator.GetBit(SignalGenerator.InputByte0, GlobalProperties.X_MaxSwitch_Bit);

            MinYswitch = !SignalGenerator.GetBit(SignalGenerator.InputByte0, GlobalProperties.Y_MinSwitch_Bit);
            MaxYswitch = !SignalGenerator.GetBit(SignalGenerator.InputByte0, GlobalProperties.Y_MaxSwitch_Bit);

            TopSwitch = !SignalGenerator.GetBit(SignalGenerator.InputByte0, GlobalProperties.TopSwitch_Bit);
            BottomSwitch = !SignalGenerator.GetBit(SignalGenerator.InputByte0, GlobalProperties.BottomSwitch_Bit);
        }

        public PointF CurrentLocation()
        {
            return new PointF(X_Rel_Location / GlobalProperties.X_Scale, Y_Rel_Location / GlobalProperties.Y_Scale);
        }

        public bool Check_Limit_Switches()
        {
            return !MaxXswitch && !MinXswitch && !MaxYswitch && !MinYswitch;
        }

        #region Internal USB control methods

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
            if (!Inhibit_Backlash_Compensation)
            {
                if ((XStepDirection != 0) && (XStepDirection != X_Last_Direction))
                {
                    absDX += (int)GlobalProperties.X_Backlash;
                    X_Delta += (int)GlobalProperties.X_Backlash * XStepDirection;
                    X_Last_Direction = XStepDirection;
                }
                if ((YStepDirection != 0) && (YStepDirection != Y_Last_Direction))
                {
                    absDY += (int)GlobalProperties.Y_Backlash;
                    Y_Delta += (int)GlobalProperties.Y_Backlash * YStepDirection;
                    Y_Last_Direction = YStepDirection;
                }
            }
            else
            {
                X_Last_Direction = XStepDirection;
                Y_Last_Direction = YStepDirection;
            }

            //Enable TQA Driver if requested
            TQA_Driver_Bit = TQA_Driver_Enable;
            Transfer();
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

            TQA_Driver_Bit = false; //Disable TQA Driver
            Transfer();
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
            else {
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


        #endregion
    }
}
