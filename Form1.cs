using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FTD2XX_NET;

namespace CNC_Drill_Controller1
{
    public partial class Form1 : Form
    {
        private FTDI USB_Interface = new FTDI();

        private byte[] OutputBuffer = new byte[32000];
        private byte[] InputBuffer = new byte[32000];

        private const int max_steps_per_transfert = 48; //1 turn

        private DateTime lastUpdate = DateTime.Now;

        private bool CheckBoxInhibit;
        private bool seekingLimits;

        public bool MaxXswitch, MinXswitch, MaxYswitch, MinYswitch;
        public bool TopSwitch, BottomSwitch;

        public int X_Scale = 960;
        public int Y_Scale = 960;

        private byte[] stepBytes = { 0x33, 0x66, 0xCC, 0x99 };//{0x11, 0x22, 0x44, 0x88};//single phase

        //double phase
        //51 = 0x33 = b'00110011'
        //102 = 0x66 = b'01100110'
        //204 = 0xCC = b'11001100'
        //153 = 0x99 = b'10011001'

        private int X_Axis_Location, Y_Axis_Location, AxisOffsetCount;
        public int X_Axis_Delta, Y_Axis_Delta;

        private DrawingTypeDialog dtypeDialog = new DrawingTypeDialog();

        public Form1()
        {
            InitializeComponent();
            AxisOffsetCount = 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = 0;
            uint numUI = 0;
            var ftStatus = USB_Interface.GetNumberOfDevices(ref numUI);
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {
                if (numUI > 0)
                {
                    logger1.AddLine(numUI.ToString("D") + " Devices Found.");

                    var ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[numUI];
                    ftStatus = USB_Interface.GetDeviceList(ftdiDeviceList);

                    if (ftStatus == FTDI.FT_STATUS.FT_OK)
                    {
                        for (var i = 0; i < numUI; i++)
                        {
                            comboBox1.Items.Add(ftdiDeviceList[i].LocId + ":" + ftdiDeviceList[i].Description);
                        }


                        comboBox1.Text = comboBox1.Items[0].ToString();


                        logger1.AddLine("Opening first device");
                        ftStatus = USB_Interface.OpenBySerialNumber(ftdiDeviceList[0].SerialNumber);
                        if (ftStatus != FTDI.FT_STATUS.FT_OK)
                        {
                            logger1.AddLine("Failed to open device (error " + ftStatus + ")");
                        }


                        logger1.AddLine("Setting default bauld rate");
                        ftStatus = USB_Interface.SetBaudRate(300);
                        if (ftStatus != FTDI.FT_STATUS.FT_OK)
                        {
                            logger1.AddLine("Failed to set Baud rate (error " + ftStatus + ")");
                        }

                        logger1.AddLine("Setting BitMode");
                        ftStatus = USB_Interface.SetBitMode(61, FTDI.FT_BIT_MODES.FT_BIT_MODE_SYNC_BITBANG);
                        //61 = 0x3D = b'00111101'
                        if (ftStatus != FTDI.FT_STATUS.FT_OK)
                        {
                            logger1.AddLine("Failed to set BitMode (error " + ftStatus + ")");
                        }
                    }

                    else
                    {
                        logger1.AddLine("Failed to list devices (error " + ftStatus + ")");
                    }

                }
                else logger1.AddLine("No devices found.");
            }
            else
            {
                logger1.AddLine("Failed to get number of devices (error " + ftStatus + ")");
            }
        }

        private void Transfert()
        {
            if (USB_Interface.IsOpen)
            {
                var stepData = CreateStepByte(X_Axis_Location, Y_Axis_Location);
                var ctrlData = CreateControlByte(checkBoxX.Checked, checkBoxY.Checked, checkBoxT.Checked,
                    checkBoxB.Checked);

                //serialize
                Serialize(stepData, ctrlData);
                //send/read

                uint res = 0;
                var ftStatus = USB_Interface.Write(OutputBuffer, 20, ref res);
                if ((ftStatus != FTDI.FT_STATUS.FT_OK) && (res != 20))
                {
                    logger1.AddLine("Failed to write data (error " + ftStatus + ") (" + res + "/20)");
                }
                //de-serialize

                ftStatus = USB_Interface.Read(InputBuffer, 20, ref res);
                if ((ftStatus != FTDI.FT_STATUS.FT_OK) && (res != 20))
                {
                    logger1.AddLine("Failed to read data (error " + ftStatus + ") (" + res + "/20)");
                }
                else
                {
                    readSwitches();
                    lastUpdate = DateTime.Now;
                }
            }
        }

        private byte CreateStepByte(int X_Location, int Y_Location)
        {
            var x = (byte)(X_Location & 0x03);
            var y = (byte)(Y_Location & 0x03);
            return (byte)((stepBytes[x] & 0x0F) | (stepBytes[y] & 0xF0));
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

            //strobe b4
            OutputBuffer[18] = 0x10 + 0x20;
            OutputBuffer[19] = 0x00 + 0x20;
        }

        private bool getBit(byte data, int bit)
        {
            return ((data & (byte)Math.Pow(2, bit)) > 0);
        }

        private byte setBit(byte input, byte bitToSet, bool set)
        {
            return (byte)(input | ((set) ? (byte)(Math.Pow(2, bitToSet)) : 0));
        }

        private byte CreateControlByte(bool ActivateX, bool ActivateY, bool ActivateTop, bool ActivateBottom)
        {
            //0 Activate X Axis Step Controller
            //1 Activate Y Axis Step Controller
            //2 Activate Drill From Top
            //3 Activate Drill From Bottom
            byte output = 0;
            if (ActivateX) output = (byte)(output | 1);
            if (ActivateY) output = (byte)(output | 2);
            if (ActivateTop) output = (byte)(output | 4);
            if (ActivateBottom) output = (byte)(output | 8);
            return output;
        }

        private void readSwitches()
        {
            //todo: re-wire header as of now max and min limit switches are swapped
            MaxXswitch = !getBit(InputBuffer[15], 1);
            MinXswitch = !getBit(InputBuffer[17], 1);

            MaxYswitch = !getBit(InputBuffer[13], 1);
            MinYswitch = !getBit(InputBuffer[11], 1);

            TopSwitch = !getBit(InputBuffer[9], 1);
            BottomSwitch = !getBit(InputBuffer[7], 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // X +
            moveBy(AxisOffsetCount, 0);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            // X - 
            moveBy(-AxisOffsetCount, 0);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Y +
            moveBy(0, AxisOffsetCount);

        }        
        private void button1_Click(object sender, EventArgs e)
        {
            // Y -
            moveBy(0, -AxisOffsetCount);
        }



        private void Sendbutton_Click(object sender, EventArgs e)
        {
            Transfert();
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logger1.Clear();
        }

        private void saveLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var logfile = File.AppendText("CNC_Drill_CTRL.log");
            logfile.WriteLine("Saving Log [" + DateTime.Now.ToString("F") + "]");
            logfile.Write(logger1.Text);
            logfile.WriteLine("");
            logfile.Close();
            logger1.AddLine("Log Saved to CNC_Drill_CTRL.log");
        }

        private void checkBoxB_CheckedChanged(object sender, EventArgs e)
        {
            if (!CheckBoxInhibit) Transfert();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (USB_Interface.IsOpen)
            {
                CheckBoxInhibit = true;
                //fetch data if too old
                if ((DateTime.Now.Subtract(lastUpdate)).Milliseconds > 250)
                {
                    Transfert();
                }

                //Update UI with usb data
                XMinStatusLabel.BackColor = MinXswitch ? Color.Lime : Color.Red;
                XMaxStatusLabel.BackColor = MaxXswitch ? Color.Lime : Color.Red;
                YMinStatusLabel.BackColor = MinYswitch ? Color.Lime : Color.Red;
                YMaxStatusLabel.BackColor = MaxYswitch ? Color.Lime : Color.Red;

                //top drill limit switch
                if (TopSwitch)
                {
                    if (checkBoxT.Checked)
                    {
                        checkBoxT.Checked = false;
                    }
                    TopStatusLabel.BackColor = SystemColors.Control;
                }
                else TopStatusLabel.BackColor = Color.Lime;

                //bottom drill limit switch
                if (BottomSwitch)
                {
                    if (checkBoxB.Checked)
                    {
                        checkBoxB.Checked = false;
                    }
                    BottomStatusLabel.BackColor = SystemColors.Control;
                }
                else BottomStatusLabel.BackColor = Color.Lime;
                CheckBoxInhibit = false;
            }

            //update UI with internal stuff
            var current_X = (X_Axis_Location - X_Axis_Delta);
            var current_Y = (Y_Axis_Location - Y_Axis_Delta);
            XStatusLabel.Text = current_X.ToString("D5");
            YStatusLabel.Text = current_Y.ToString("D5");
            Xlabel.Text = ((float)current_X / X_Scale).ToString("F4");
            Ylabel.Text = ((float)current_Y / Y_Scale).ToString("F4");


        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                if (dtypeDialog.ShowDialog() == DialogResult.OK)
                {
                    logger1.AddLine("Opening File: " + openFileDialog1.FileName);
                    logger1.AddLine("Inverted: " + dtypeDialog.DrawingConfig.Inverted);
                    logger1.AddLine("Type: " + dtypeDialog.DrawingConfig.Type);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var toParse = (string)comboBox2.SelectedItem;
            toParse = toParse.Split(new[] { ' ' })[0];
            try
            {
                AxisOffsetCount = Convert.ToInt32(toParse);
            }
            catch
            {
                AxisOffsetCount = 1;
            }
        }

        private void moveBy(int X, int Y)
        {
            //todo max 1 turn (max_steps_per_transfert) before pooling for limit swwitches
            //todo if limit reached before end of moveby, post error in logger1

            var numMoves = (Math.Abs(X) >= Math.Abs(Y)) ? Math.Abs(X) : Math.Abs(Y);
            var offsetX = 0;
            if (X > 0)
            {
                offsetX = 1;
            }
            else if (X < 0)
            {
                offsetX = -1;
            }

            var offsetY = 0;
            if (Y > 0)
            {
                offsetY = 1;
            }
            else if (Y < 0)
            {
                offsetY = -1;
            }

            var numCycle = numMoves/max_steps_per_transfert;
            if (numCycle < 1) numCycle = 1;

            for (var i = 0; i < numCycle; i++)
            {
                var num_moves_for_this_cycle = (numMoves > max_steps_per_transfert) ? max_steps_per_transfert : numMoves;
                for (var j = 0; j < num_moves_for_this_cycle; j++)
                {
                    if (Math.Abs(X) != 0)
                    {
                        X_Axis_Location += offsetX;
                        X += offsetX;
                    }
                    if (Math.Abs(Y) != 0)
                    {
                        Y_Axis_Location += offsetY;
                        Y += offsetY;
                    }
                    Transfert();
                }
                numMoves -= num_moves_for_this_cycle;
                Refresh();
                if (MaxXswitch | MinXswitch | MaxYswitch | MinYswitch)
                {
                    if (!seekingLimits) logger1.AddLine("Limit switch triggered before end of move");
                    i = numCycle; //if limit reached exit loop
                }
            }
        }

        private void setXButton_Click(object sender, EventArgs e)
        {
            // (loc - delta) / scale = pos
            // loc - delta = (pos * scale)
            // loc - (pos * scale) = delta
            X_Axis_Delta = (int)(X_Axis_Location - (safeTextToFloat(XCurrentPosTextBox.Text) * X_Scale));
        }
        private void SetYButton_Click(object sender, EventArgs e)
        {
            Y_Axis_Delta = (int)(Y_Axis_Location - (safeTextToFloat(YCurrentPosTextBox.Text) * Y_Scale));
        }

        private float safeTextToFloat(string text)
        {
            float res;
            if (float.TryParse(text, out res))
            {
                return res;
            }
            logger1.AddLine("Failed to convert value: " + text);
            return 0.0f;
        }

        private void zeroXbutton_Click(object sender, EventArgs e)
        {
            XCurrentPosTextBox.Text = "0.0000";
            setXButton_Click(this, e);
        }

        private void zeroYbutton_Click(object sender, EventArgs e)
        {
            YCurrentPosTextBox.Text = "0.0000";
            SetYButton_Click(this, e);
        }

        private void zeroAllbutton_Click(object sender, EventArgs e)
        {
            zeroXbutton_Click(this, e);
            zeroYbutton_Click(this, e);
        }


    }
}
