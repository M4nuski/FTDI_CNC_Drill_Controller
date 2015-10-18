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

        public byte[] InputBuffer { get; set; }

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

        public bool X_Driver { get; set; }
        public bool Y_Driver { get; set; }
        public bool T_Driver { get; set; }
        public bool Cycle_Drill { get; set; }

        public int X_Abs_Location{ get; set; }
        public int Y_Abs_Location { get; set; }
        public int X_Delta{ get; set; }
        public int Y_Delta{ get; set; }
        public int X_Last_Direction { get; set; }
        public int Y_Last_Direction{ get; set; }
        public int X_Rel_Location { get { return X_Abs_Location - X_Delta; } }
        public int Y_Rel_Location { get { return Y_Abs_Location - Y_Delta; } }
        
        public ProgressEvent OnProgress { get; set; }
            
        private void UpdateProgress(int Progress, bool Done)
        {
            if (OnProgress != null) OnProgress(Progress, Done);
        }

        public USB_Control() 
        {
            InputBuffer  = new byte[64];
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

            UpdateProgress(100, true);
            return true;
        }

        public void Transfer()
        {
            if (USB_Interface.IsOpen)
            {
                var stepData = CreateStepByte();
                var ctrlData = CreateControlByte();

                Serialize(stepData, ctrlData);

                if (SendToUSB())
                {
                    DeSerialize();
                    LastUpdate = DateTime.Now;
                }
            }
        }

        private bool SendToUSB()
        {
            var result = true;
            uint res = 0;
            var ftStatus = USB_Interface.Write(OutputBuffer, 64, ref res);
            if ((ftStatus != FTDI.FT_STATUS.FT_OK) && (res != 64))
            {
                USB_Interface.Close();
                ExtLog.AddLine("Failed to write data (error " + ftStatus + ") (" + res + "/64)");
                result = false;
            }
            else
            {
                ftStatus = USB_Interface.Read(InputBuffer, 64, ref res);
                if ((ftStatus != FTDI.FT_STATUS.FT_OK) && (res != 64))
                {
                    USB_Interface.Close();
                    ExtLog.AddLine("Failed to read data (error " + ftStatus + ") (" + res + "/64)");
                    result = false;
                }
            }
            return result;
        }

        private byte CreateStepByte()
        {
            var x = (byte)(X_Abs_Location & GlobalProperties.numStepMask);
            var y = (byte)(Y_Abs_Location & GlobalProperties.numStepMask);
            return (byte)((GlobalProperties.stepBytes[x] & 0x0F) | (GlobalProperties.stepBytes[y] & 0xF0));
        }

        private byte CreateControlByte()
        {
            //0 Activate X Axis Step Controller Driver
            //1 Activate Y Axis Step Controller Driver
            //2 Activate Drill
            //3 Activate Torque Assist Driver

            //4 TQA_X_Pos;
            //5 TQA_X_Neg;
            //6 TQA_Y_Pos;
            //7 TQA_Y_Neg;

            var TQA_X_Pos = (X_Last_Direction == 1);
            var TQA_X_Neg = (X_Last_Direction == -1);
            var TQA_Y_Pos = (Y_Last_Direction == 1);
            var TQA_Y_Neg = (Y_Last_Direction == -1);

            byte output = 0;
            if (X_Driver) output = (byte)(output | 1);
            if (Y_Driver) output = (byte)(output | 2);
            if (Cycle_Drill) output = (byte)(output | 4);
            if (T_Driver) output = (byte)(output | 8);

            if (TQA_X_Pos) output = (byte)(output | 16);
            if (TQA_X_Neg) output = (byte)(output | 32);
            if (TQA_Y_Pos) output = (byte)(output | 64);
            if (TQA_Y_Neg) output = (byte)(output | 128);

            return output;
        }

        private void Serialize(byte Steps, byte Ctrl)
        {   // Default states:
            //          0 Out Clock					Down 0x01
            //          1 In Inbuffer				N/A  0x02
            //          2 Out StepBuffer			N/A  0x04
            //          3 Out CtrlBuffer			N/A  0x08

            //          4 Out Latch USB to Outputs	Down 0x10
            //          5 Out Latch Input to USB	Up   0x20

            //clear buffer
            for (var i = 0; i < 64; i++)
            {
                OutputBuffer[i] = 0x20; //b5 up by default
            }

            //create clock to write 8 bits 0-15
            for (var i = 0; i < 16; i++)
            {
                OutputBuffer[i] = (byte)((i % 2) + 0x20);
            }

            //write data
            //msb first
            //bit2 steps
            //bit3 ctrl
            for (var i = 7; i >= 0; i--)
            {
                var offset = ((7 - i) * 2);

                OutputBuffer[offset] = setBit(OutputBuffer[offset], 2, getBit(Steps, i));
                OutputBuffer[offset + 1] = setBit(OutputBuffer[offset + 1], 2, getBit(Steps, i));

                OutputBuffer[offset] = setBit(OutputBuffer[offset], 3, getBit(Ctrl, i));
                OutputBuffer[offset + 1] = setBit(OutputBuffer[offset + 1], 3, getBit(Ctrl, i));
            }


            //strobe b4 up 16-17
            OutputBuffer[16] = 0x10 + 0x20;
            OutputBuffer[17] = 0x00 + 0x20;

            //delay of at least 18 for move and switches feedback 18-42

            //strobe b5 down with a clock cycle to load data 43-45
            OutputBuffer[43] = 0x00;
            OutputBuffer[44] = 0x01;
            OutputBuffer[45] = 0x00;

            //create clock to read 8 bits 46-61
            for (var i = 0; i < 16; i++)
            {
                OutputBuffer[46 + i] = (byte)((i % 2) + 0x20);
            }

            //add to clocks to make sure the interface's buffer have been sent 62 - 63
        }

        private static bool getBit(byte data, int bit)
        {
            return ((data & (byte)Math.Pow(2, bit)) > 0);
        }

        private static byte setBit(byte input, byte bitToSet, bool set)
        {
            return (byte)(input | ((set) ? (byte)(Math.Pow(2, bitToSet)) : 0));
        }

        private void DeSerialize()
        {
            MaxXswitch = !getBit(InputBuffer[61], 1);
            MinXswitch = !getBit(InputBuffer[59], 1);

            MinYswitch = !getBit(InputBuffer[57], 1);
            MaxYswitch = !getBit(InputBuffer[55], 1);

            TopSwitch = !getBit(InputBuffer[53], 1);
            BottomSwitch = !getBit(InputBuffer[51], 1);
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
        #endregion


    }
}
