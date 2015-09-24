using System;
using System.Collections.Generic;
using System.Drawing;
using FTD2XX_NET;

namespace CNC_Drill_Controller1
{
    class USB_Control
    {
        private FTDI USB_Interface = new FTDI();
        private byte[] OutputBuffer = new byte[64];
        private byte[] InputBuffer = new byte[64];

        public bool IsOpen { get { return USB_Interface.IsOpen; } }
        public DateTime LastUpdate = DateTime.Now;

        public struct InputSwitchStruct
        {
            public bool MaxXswitch, MinXswitch, MaxYswitch, MinYswitch;
            public bool TopSwitch, BottomSwitch;
            public bool SyncXswitch, SyncYswitch;
        }
        public InputSwitchStruct SwitchesInput;

        public struct OutputSwitchStruct
        {
            public bool X_Driver, Y_Driver;
            public bool Cycle_Top, Cycle_Bottom;
        }
        public OutputSwitchStruct SwitchesOutput;

        public int X_Abs_Location, Y_Abs_Location;

        public int X_Delta, Y_Delta; //todo save to application context
        public int X_Rel_Location { get { return X_Abs_Location - X_Delta; } }
        public int Y_Rel_Location { get { return Y_Abs_Location - Y_Delta; } }

        private int X_Last_Direction, Y_Last_Direction;
        public bool Inhibit_Backlash_Compensation, Inhibit_LimitSwitches_Warning;

        public int X_Sync_Modulus, Y_Sync_Modulus;
        public int Sync_Divisor = 48;

        public delegate void ProgressEvent(int Progress, bool Done);
        public ProgressEvent OnProgress;

        private void UpdateProgress(int Progress, bool Done) //todo change to workerthread
        {
            if (OnProgress != null) OnProgress(Progress, Done);
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

            ExtLog.AddLine("Setting default bauld rate");
            ftStatus = USB_Interface.SetBaudRate(1200);
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

            UpdateProgress(100, true);
            return true;
        }

        public InputSwitchStruct Transfer()
        {
            if (USB_Interface.IsOpen)
            {
                var stepData = CreateStepByte();
                var ctrlData = CreateControlByte();

                //serialize
                Serialize(stepData, ctrlData);
                //send/read

                uint res = 0;
                var ftStatus = USB_Interface.Write(OutputBuffer, 20, ref res);
                if ((ftStatus != FTDI.FT_STATUS.FT_OK) && (res != 20))
                {
                    USB_Interface.Close();
                    ExtLog.AddLine("Failed to write data (error " + ftStatus + ") (" + res + "/20)");
                }
                //de-serialize

                ftStatus = USB_Interface.Read(InputBuffer, 20, ref res);
                if ((ftStatus != FTDI.FT_STATUS.FT_OK) && (res != 20))
                {
                    USB_Interface.Close();
                    ExtLog.AddLine("Failed to read data (error " + ftStatus + ") (" + res + "/20)");
                }
                else
                {
                    LastUpdate = DateTime.Now;
                    SwitchesInput = DecodeSwitches();
                    //todo check sync and adjust if failed
                    //while checking sync CALL UPGRADE PROGRESS
                }
            }
            return SwitchesInput;
        }

        private byte CreateStepByte()
        {
            var x = (byte)(X_Abs_Location & 0x03);
            var y = (byte)(Y_Abs_Location & 0x03);
            return (byte)((GlobalProperties.stepBytes[x] & 0x0F) | (GlobalProperties.stepBytes[y] & 0xF0));
        }

        private byte CreateControlByte()
        {
            //0 Activate X Axis Step Controller
            //1 Activate Y Axis Step Controller
            //2 Activate Drill From Top
            //3 Activate Drill From Bottom
            byte output = 0;
            if (SwitchesOutput.X_Driver) output = (byte)(output | 1);
            if (SwitchesOutput.Y_Driver) output = (byte)(output | 2);
            if (SwitchesOutput.Cycle_Top) output = (byte)(output | 4);
            if (SwitchesOutput.Cycle_Bottom) output = (byte)(output | 8);
            return output;
        }

        private void Serialize(byte Steps, byte Ctrl)
        {   //bitFrame:
            //01234567890123456789
            //b0                b0
            //50                40
            //  7766554433221100
            //  
            //clear buffer
            for (var i = 0; i < 20; i++)
            {
                OutputBuffer[i] = 0x20;
            }

            //create clock
            for (var i = 0; i < 18; i++)
            {
                OutputBuffer[i] = (byte)((i % 2) + 0x20);
            }

            //strobe b5 down with clock cycle to load data
            OutputBuffer[0] = 0x00;
            OutputBuffer[1] = 0x01;

            //msb first
            //bit2 steps
            //bit3 ctrl

            for (var i = 7; i >= 0; i--)
            {
                var offset = 2 + ((7 - i) * 2);

                OutputBuffer[offset] = setBit(OutputBuffer[offset], 2, getBit(Steps, i));
                OutputBuffer[offset + 1] = setBit(OutputBuffer[offset + 1], 2, getBit(Steps, i));

                OutputBuffer[offset] = setBit(OutputBuffer[offset], 3, getBit(Ctrl, i));
                OutputBuffer[offset + 1] = setBit(OutputBuffer[offset + 1], 3, getBit(Ctrl, i));
            }

            //strobe b4 up
            OutputBuffer[18] = 0x10 + 0x20;
            OutputBuffer[19] = 0x00 + 0x20;
        }

        private static bool getBit(byte data, int bit)
        {
            return ((data & (byte)Math.Pow(2, bit)) > 0);
        }

        private static byte setBit(byte input, byte bitToSet, bool set)
        {
            return (byte)(input | ((set) ? (byte)(Math.Pow(2, bitToSet)) : 0));
        }

        private InputSwitchStruct DecodeSwitches()
        {
            SwitchesInput.MaxXswitch = !getBit(InputBuffer[17], 1);
            SwitchesInput.MinXswitch = !getBit(InputBuffer[15], 1);

            SwitchesInput.MaxYswitch = !getBit(InputBuffer[11], 1);
            SwitchesInput.MinYswitch = !getBit(InputBuffer[13], 1);

            SwitchesInput.TopSwitch = !getBit(InputBuffer[9], 1);
            SwitchesInput.BottomSwitch = !getBit(InputBuffer[7], 1);

            SwitchesInput.SyncXswitch = !getBit(InputBuffer[3], 1);
            SwitchesInput.SyncYswitch = !getBit(InputBuffer[5], 1);

            return SwitchesInput;
        }

        public PointF CurrentLocation()
        {
            var current_X = ((float)X_Abs_Location - X_Delta) / GlobalProperties.X_Scale;
            var current_Y = ((float)Y_Abs_Location - Y_Delta) / GlobalProperties.Y_Scale;
            return new PointF(current_X, current_Y);
        }


        #region Internal USB control methods
        public void MoveBy(int byX, int byY)
        {
            var abyX = Math.Abs(byX);
            var abyY = Math.Abs(byY);

            //process directions
            var XStepDirection = (byX == 0) ? 0 : byX / abyX;
            var YStepDirection = (byY == 0) ? 0 : byY / abyY;

            //process backlash
            if (!Inhibit_Backlash_Compensation)
            {
                if ((XStepDirection != 0) && (XStepDirection != X_Last_Direction))
                {
                    abyX += GlobalProperties.X_Backlash;
                    X_Delta += GlobalProperties.X_Backlash * XStepDirection;
                    X_Last_Direction = XStepDirection;
                }
                if ((YStepDirection != 0) && (YStepDirection != Y_Last_Direction))
                {
                    abyY += GlobalProperties.Y_Backlash;
                    Y_Delta += GlobalProperties.Y_Backlash * YStepDirection;
                    Y_Last_Direction = YStepDirection;
                }
            }
            else
            {
                X_Last_Direction = XStepDirection;
                Y_Last_Direction = YStepDirection;
            }


            //process moves
            var numMoves = (abyX >= abyY) ? abyX : abyY;

            var stridex = 1.0f; //default maximum stride
            var stridey = 1.0f;

            if ((abyX != 0) || (abyY != 0)) //adjust stride
            {
                if (abyX > abyY) 
                {
                    stridey = (float)abyY / abyY;
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

                if (fractx >= 1.0f)
                {
                    X_Abs_Location += XStepDirection;
                    fractx -= 1.0f;
                }

                if (fracty >= 1.0f)
                {
                    Y_Abs_Location += YStepDirection;
                    fracty -= 1.0f;
                }

                Transfer();
            }


            //if (SwitchesInput.MaxXswitch || SwitchesInput.MinXswitch || SwitchesInput.MaxYswitch ||
            //    SwitchesInput.MinYswitch)
            //{
            //    if (!Inhibit_LimitSwitches_Warning) ExtLog.AddLine("Limit switch triggered before end of move");

            //} //todo move to sync switch test and report progress

            UpdateProgress(100, true);
        }

        public void MoveTo(float X, float Y)
        {
            var current_pos = CurrentLocation();
            if (!SwitchesInput.MaxXswitch && !SwitchesInput.MinXswitch &&
                !SwitchesInput.MaxYswitch && !SwitchesInput.MinYswitch)
            {
                var deltaX = X - current_pos.X;
                var deltaY = Y - current_pos.Y;
                MoveBy((int)(deltaX * GlobalProperties.X_Scale), (int)(deltaY * GlobalProperties.Y_Scale));
            }
            else ExtLog.AddLine("Limit switch warning must be cleared before moving.");
        }
        #endregion


    }
}
