using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using FTD2XX_NET;
using M4nuskomponents;

namespace CNC_Drill_Controller1
{
    public partial class Form1 : Form
    {
        #region USB Interface Properties

        private FTDI USB_Interface = new FTDI();
        private byte[] OutputBuffer = new byte[32000];
        private byte[] InputBuffer = new byte[32000];
        private const int max_steps_per_transfert = 48; //1 turn
        public int X_Scale = 960;
        public int Y_Scale = 960;
        private byte[] stepBytes = { 0x33, 0x66, 0xCC, 0x99 };

        #endregion

        #region USB to UI properties

        private DateTime lastUpdate = DateTime.Now;
        private bool CheckBoxInhibit;
        //public bool MaxXswitch, MinXswitch, MaxYswitch, MinYswitch;
        //public bool TopSwitch, BottomSwitch;
        private int X_Axis_Location, Y_Axis_Location, AxisOffsetCount;
        public int X_Axis_Delta, Y_Axis_Delta;

        private struct SwitchFeedBack
        {
            public bool MaxXswitch, MinXswitch, MaxYswitch, MinYswitch;
            public bool TopSwitch, BottomSwitch;
        }

        private SwitchFeedBack SwitchState;
        private DrillNode.DrillNodeStatus lastSelectedStatus;
        private int lastSelectedIndex;

        #endregion

        #region View Properties

        private DrawingTypeDialog dtypeDialog = new DrawingTypeDialog();
        private const float NodeDiameter = 0.05f;
        private List<DrillNode> Nodes;
        private Viewer nodeViewer;
        private CrossHair cursorCrossHair;
        private CrossHair drillCrossHair;
        private Box CNCTableBox;
        private Box drawingPageBox;

        #endregion

        private char[] trimChars = { ' ' };
        private bool seekingLimits; //TODO: add limit switch auto seeking, ask for approximate location and creep toward minX and minY

        #region Form Initialization
        public Form1()
        {
            InitializeComponent();
            AxisOffsetCount = 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ExtLog.Logger = logger1;
            #region View initialization

            cursorCrossHair = new CrossHair(0, 0, Color.Blue);
            drillCrossHair = new CrossHair(0, 0, Color.Red);
            CNCTableBox = new Box(0, 0, 6, 6, Color.LightGray);
            drawingPageBox = new Box(0, 0, 8.5f, 11, Color.GhostWhite);
            nodeViewer = new Viewer(OutputLabel, new PointF(11.0f, 11.0f));
            lastSelectedStatus = DrillNode.DrillNodeStatus.Idle;
            lastSelectedIndex = -1;
            RebuildListBoxAndViewerFromNodes();

            #endregion

            AxisOffsetComboBox.SelectedIndex = 0;

            #region USB interface initialization

            USBdevicesComboBox.Items.Clear();

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
                            USBdevicesComboBox.Items.Add(ftdiDeviceList[i].LocId + ":" + ftdiDeviceList[i].Description);
                        }

                        USBdevicesComboBox.SelectedIndex = 0;
                        //OpenDeviceByLocation(ftdiDeviceList[0].LocId);
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
            #endregion
        }
        #endregion

        #region Direct USB UI control methods
        private void PlusXbutton_Click(object sender, EventArgs e)
        {
            moveBy(AxisOffsetCount, 0);
        }
        private void MinusXbutton_Click(object sender, EventArgs e)
        {
            moveBy(-AxisOffsetCount, 0);
        }
        private void PlusYbutton_Click(object sender, EventArgs e)
        {
            moveBy(0, AxisOffsetCount);

        }
        private void MinusYbutton_Click(object sender, EventArgs e)
        {
            moveBy(0, -AxisOffsetCount);
        }
        private void checkBoxB_CheckedChanged(object sender, EventArgs e)
        {
            if (!CheckBoxInhibit) Transfer();
        }

        private void Sendbutton_Click(object sender, EventArgs e)
        {
            Transfer();
        }
        private void ReloadUSBbutton_Click(object sender, EventArgs e)
        {
            Form1_Load(this, e);
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

        private void AxisOffsetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var toParse = (string)AxisOffsetComboBox.SelectedItem;
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
        #endregion

        #region Log methods
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
        #endregion

        #region USB transfer method and heleprs
        private SwitchFeedBack Transfer()
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
                    USB_Interface.Close();
                    logger1.AddLine("Failed to write data (error " + ftStatus + ") (" + res + "/20)");
                }
                //de-serialize

                ftStatus = USB_Interface.Read(InputBuffer, 20, ref res);
                if ((ftStatus != FTDI.FT_STATUS.FT_OK) && (res != 20))
                {
                    USB_Interface.Close();
                    logger1.AddLine("Failed to read data (error " + ftStatus + ") (" + res + "/20)");
                }
                else
                {
                    SwitchState = readSwitches();
                    lastUpdate = DateTime.Now;
                }
            }
            return SwitchState;
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

        private SwitchFeedBack readSwitches()
        {
            //todo: re-wire header as of now max and min limit switches are swapped
            SwitchState.MaxXswitch = !getBit(InputBuffer[15], 1);
            SwitchState.MinXswitch = !getBit(InputBuffer[17], 1);

            SwitchState.MaxYswitch = !getBit(InputBuffer[13], 1);
            SwitchState.MinYswitch = !getBit(InputBuffer[11], 1);

            SwitchState.TopSwitch = !getBit(InputBuffer[9], 1);
            SwitchState.BottomSwitch = !getBit(InputBuffer[7], 1);
            return SwitchState;
        }
        #endregion

        #region USB controls methods
        private void USBdevicesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var locStr = (string)USBdevicesComboBox.SelectedItem;
            locStr = locStr.Split(new[] { ':' })[0];
            uint loc;
            if (uint.TryParse(locStr, out loc)) OpenDeviceByLocation(loc);
            else
            {
                logger1.AddLine("Failed to parse Location Id of: " + (string)USBdevicesComboBox.SelectedItem);
            }
        }

        private void OpenDeviceByLocation(uint LocationID)
        {
            if (USB_Interface.IsOpen) USB_Interface.Close();

            logger1.AddLine("Opening device");
            var ftStatus = USB_Interface.OpenByLocation(LocationID);
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
        #endregion

        private void UIupdateTimer_Tick(object sender, EventArgs e)
        {
            #region UI update from USB data
            if (USB_Interface.IsOpen)
            {
                CheckBoxInhibit = true;
                //fetch data if too old
                if ((DateTime.Now.Subtract(lastUpdate)).Milliseconds > 250)
                {
                    Transfer();
                }

                if (SwitchState.MinXswitch && SwitchState.MaxXswitch) //check for impossible combinaison (step controller or power not plugged-in)
                {
                    XMinStatusLabel.BackColor = Color.DodgerBlue;
                    XMaxStatusLabel.BackColor = Color.DodgerBlue;
                }
                else
                {
                    XMinStatusLabel.BackColor = !SwitchState.MinXswitch ? Color.Lime : Color.Red;
                    XMaxStatusLabel.BackColor = !SwitchState.MaxXswitch ? Color.Lime : Color.Red;
                }

                if (SwitchState.MinYswitch && SwitchState.MaxYswitch)
                {
                    YMinStatusLabel.BackColor = Color.DodgerBlue;
                    YMaxStatusLabel.BackColor = Color.DodgerBlue;
                }
                else
                {
                    YMinStatusLabel.BackColor = !SwitchState.MinYswitch ? Color.Lime : Color.Red;
                    YMaxStatusLabel.BackColor = !SwitchState.MaxYswitch ? Color.Lime : Color.Red;
                }


                //top drill limit switch
                if (!SwitchState.TopSwitch)
                {
                    if (checkBoxT.Checked)
                    {
                        checkBoxT.Checked = false;
                    }
                    TopStatusLabel.BackColor = SystemColors.Control;
                }
                else TopStatusLabel.BackColor = Color.Lime;

                //bottom drill limit switch
                if (!SwitchState.BottomSwitch)
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
            #endregion

            #region UI update from internal properties / view
            var current_X = (X_Axis_Location - X_Axis_Delta);
            var current_Y = (Y_Axis_Location - Y_Axis_Delta);
            XStatusLabel.Text = current_X.ToString("D5");
            YStatusLabel.Text = current_Y.ToString("D5");
            Xlabel.Text = "X: " + ((float)current_X / X_Scale).ToString("F4");
            Ylabel.Text = "Y: " + ((float)current_Y / Y_Scale).ToString("F4");

            ViewXLabel.Text = nodeViewer.MousePositionF.X.ToString("F4");
            ViewYLabel.Text = nodeViewer.MousePositionF.Y.ToString("F4");
            ViewZoomLabel.Text = (int)(nodeViewer.ZoomLevel * 100) + "%";

            cursorCrossHair.UpdatePosition(nodeViewer.MousePositionF);
            drillCrossHair.UpdatePosition((float)current_X / X_Scale, (float)current_Y / Y_Scale);
            OutputLabel.Refresh();
            #endregion
        }

        #region Internal USB control methods
        private void moveBy(int DeltaX, int DeltaY)
        {
            var numMoves = (Math.Abs(DeltaX) >= Math.Abs(DeltaY)) ? Math.Abs(DeltaX) : Math.Abs(DeltaY);

            var XStepDirection = 0;
            if (DeltaX > 0)
            {
                XStepDirection = 1;
            }
            else if (DeltaX < 0)
            {
                XStepDirection = -1;
            }

            var YStepDirection = 0;
            if (DeltaY > 0)
            {
                YStepDirection = 1;
            }
            else if (DeltaY < 0)
            {
                YStepDirection = -1;
            }
            //from http://stackoverflow.com/questions/17944/how-to-round-up-the-result-of-integer-division
            //int pageCount = (records + recordsPerPage - 1) / recordsPerPage;
            var numCycle = (numMoves + max_steps_per_transfert - 1) / max_steps_per_transfert;
            if (numCycle > 1)
            {
                Cursor.Current = Cursors.WaitCursor;
                toolStripProgressBar1.Maximum = numCycle - 1;
            }

            for (var i = 0; i < numCycle; i++)
            {
                var num_moves_for_this_cycle = (numMoves > max_steps_per_transfert) ? max_steps_per_transfert : numMoves;
                for (var j = 0; j < num_moves_for_this_cycle; j++)
                {
                    if (Math.Abs(DeltaX) != 0)
                    {
                        X_Axis_Location += XStepDirection;
                        DeltaX -= XStepDirection;
                    }
                    if (Math.Abs(DeltaY) != 0)
                    {
                        Y_Axis_Location += YStepDirection;
                        DeltaY -= YStepDirection;
                    }
                    Transfer();
                }
                toolStripProgressBar1.Value = i;
                numMoves -= num_moves_for_this_cycle;
                Refresh();
                if (SwitchState.MaxXswitch || SwitchState.MinXswitch || SwitchState.MaxYswitch || SwitchState.MinYswitch)
                {
                    if (!seekingLimits) logger1.AddLine("Limit switch triggered before end of move");
                    i = numCycle; //if limit reached exit loop
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private PointF CurrentLocation()
        {
            var current_X = ((float)X_Axis_Location - X_Axis_Delta) / X_Scale;
            var current_Y = ((float)Y_Axis_Location - Y_Axis_Delta) / Y_Scale;
            return new PointF(current_X, current_Y);
        }

        private void MoveTo(float X, float Y)
        {
            var current_pos = CurrentLocation();
            if ((current_pos.X < 5.900) && (current_pos.Y < 5.900))
            {
                var deltaX = X - current_pos.X;
                var deltaY = Y - current_pos.Y;
                moveBy((int)(deltaX * X_Scale), (int)(deltaY * Y_Scale));
            }
            else logger1.AddLine("Move target out of range.");
        }
        #endregion

        #region View / Listbox UI controls
        private void OutputLabel_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Hide();
        }

        private void OutputLabel_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Show();
        }

        private void LoadFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "*.vdx";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                if (dtypeDialog.ShowDialog() == DialogResult.OK)
                {
                    logger1.AddLine("Opening File: " + openFileDialog1.FileName);
                    logger1.AddLine("Inverted: " + dtypeDialog.DrawingConfig.Inverted);
                    logger1.AddLine("Type: " + dtypeDialog.DrawingConfig.Type);

                    var loader = new VDXLoader(openFileDialog1.FileName, dtypeDialog.DrawingConfig.Inverted);
                    Nodes = loader.DrillNodes;
                    logger1.AddLine(Nodes.Count.ToString("D") + " Nodes loaded.");

                    drawingPageBox = new Box(0, 0, loader.PageWidth, loader.PageHeight, Color.GhostWhite);
                    lastSelectedStatus = DrillNode.DrillNodeStatus.Idle;
                    RebuildListBoxAndViewerFromNodes();
                    lastSelectedIndex = -1;
                }
            }
        }

        private void MoveTobutton_Click(object sender, EventArgs e)
        {
            var movedata = (string)listBox1.SelectedItem;

            var axisdata = movedata.Split(trimChars);
            if (axisdata.Length == 2)
            {
                var mx = safeTextToFloat(axisdata[0].Trim(trimChars));
                var my = safeTextToFloat(axisdata[1].Trim(trimChars));
                logger1.AddLine("Moving to: " + mx.ToString("F5") + ", " + my.ToString("F5"));
                MoveTo(mx, my);
            }
        }
        private void SetAsXYbutton_Click(object sender, EventArgs e)
        {
            var movedata = (string)listBox1.SelectedItem;
            var axisdata = movedata.Split(trimChars);
            if (axisdata.Length == 2)
            {
                XCurrentPosTextBox.Text = axisdata[0].Trim(trimChars);
                YCurrentPosTextBox.Text = axisdata[1].Trim(trimChars);
                setXButton_Click(this, e);
                SetYButton_Click(this, e);
            }
        }
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            NodesContextMenu.Show(listBox1, listBox1.PointToClient(Cursor.Position));
        }
        private void NodeContextTARGET_Click(object sender, EventArgs e)
        {
            Nodes[listBox1.SelectedIndex].status = DrillNode.DrillNodeStatus.Next;
            UpdateNodeColors();
        }
        private void NodeContextDRILED_Click(object sender, EventArgs e)
        {
            Nodes[listBox1.SelectedIndex].status = DrillNode.DrillNodeStatus.Drilled;
            UpdateNodeColors();
        }
        private void NodeContextIDLE_Click(object sender, EventArgs e)
        {
            Nodes[listBox1.SelectedIndex].status = DrillNode.DrillNodeStatus.Idle;
            UpdateNodeColors();
        }


        private void RebuildListBoxAndViewerFromNodes()
        {
            listBox1.Items.Clear();
            lastSelectedStatus = DrillNode.DrillNodeStatus.Idle;
            lastSelectedIndex = -1;
            nodeViewer.Elements = new List<IViewerElements>
            {
                drawingPageBox,
                CNCTableBox,
                drillCrossHair,
                cursorCrossHair
            };

            if ((Nodes != null) && (Nodes.Count > 0))
            {
                for (var i = 0; i < Nodes.Count; i++)
                {
                    nodeViewer.Elements.Add(new Node(Nodes[i].location, NodeDiameter, Nodes[i].Color, i));
                    listBox1.Items.Add(Nodes[i].Location);
                    Nodes[i].ID = i;
                }
            }
        }

        private void OffsetOriginBtton_Click(object sender, EventArgs e)
        {
            var origOffset = new SizeF(safeTextToFloat(XoriginTextbox.Text), safeTextToFloat(YOriginTextbox.Text));
            for (var i = 0; i < Nodes.Count; i++)
                Nodes[i].location = new PointF(Nodes[i].location.X - origOffset.Width, Nodes[i].location.Y - origOffset.Height);
            RebuildListBoxAndViewerFromNodes();
            XoriginTextbox.Text = "0.0000";
            YOriginTextbox.Text = "0.0000";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lastSelectedIndex >= 0)
            {
                if (Nodes[lastSelectedIndex].status == DrillNode.DrillNodeStatus.Selected)
                    Nodes[lastSelectedIndex].status = lastSelectedStatus;
            }
            lastSelectedStatus = Nodes[listBox1.SelectedIndex].status;
            Nodes[listBox1.SelectedIndex].status = DrillNode.DrillNodeStatus.Selected;
            lastSelectedIndex = listBox1.SelectedIndex;
            UpdateNodeColors();
            OutputLabel.Refresh();
        }
        #endregion

        private bool OutOfScopeUSB_IsOpen()
        {
            return USB_Interface.IsOpen;
        }

        private void DrillButton_Click(object sender, EventArgs e)
        {
            //script :
            //check state of all switch for startup conditions.
            //disable controls/UI
            //set node as target/next
            //start time-out timer
            //init drill from top
            //wait for top switch off
            //wait for top switch on
            //set node as drilled
            //if timer expires before end of drill cycle set node as idle and log error

            logger1.AddLine("Starting Scripted Run [Drill Selected Node]...");
            var failed = !OutOfScopeUSB_IsOpen();

            if (!SwitchState.MaxXswitch && !SwitchState.MinXswitch && !SwitchState.MaxYswitch && !SwitchState.MinYswitch && SwitchState.TopSwitch && !SwitchState.BottomSwitch && !failed)
            {
                Nodes[listBox1.SelectedIndex].status = DrillNode.DrillNodeStatus.Next;
                UpdateNodeColors();
                logger1.AddLine("Moving to: " + Nodes[listBox1.SelectedIndex].Location);
                Refresh();

                Enabled = false;
                MoveTo(Nodes[listBox1.SelectedIndex].location.X, Nodes[listBox1.SelectedIndex].location.Y);

                logger1.AddLine("Drilling...");
                checkBoxT.Checked = true;
                Refresh();

                var maxTries = 10;
                while (SwitchState.TopSwitch && !failed)
                {
                    SwitchState = Transfer();
                    failed = !OutOfScopeUSB_IsOpen() || (maxTries < 0);
                    UIupdateTimer_Tick(this, null);
                    Refresh();
                    maxTries--;
                    Thread.Sleep(100);
                }
                checkBoxT.Checked = false;

                maxTries = 10;
                while (!SwitchState.TopSwitch && !failed)
                {
                    SwitchState = Transfer();
                    failed = !OutOfScopeUSB_IsOpen() || (maxTries < 0);
                    UIupdateTimer_Tick(this, null);
                    Refresh();
                    maxTries--;
                    Thread.Sleep(100);
                }
                Nodes[listBox1.SelectedIndex].status = DrillNode.DrillNodeStatus.Drilled;
                UpdateNodeColors();
                logger1.AddLine("Scripted Run Completed.");
            }
            else logger1.AddLine("Can't init drill cycle, limit switches are not properly set.");
            Enabled = true;
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            logger1.AddLine("Starting Scripted Run [Drill all Nodes]...");
            var failed = !OutOfScopeUSB_IsOpen();

            if (!SwitchState.MaxXswitch && !SwitchState.MinXswitch && !SwitchState.MaxYswitch &&
                !SwitchState.MinYswitch && SwitchState.TopSwitch && !SwitchState.BottomSwitch && !failed)
            {
                for (var i = 0; i < listBox1.Items.Count; i++)
                {
                    if (!SwitchState.MaxXswitch && !SwitchState.MinXswitch && !SwitchState.MaxYswitch &&
                        !SwitchState.MinYswitch && SwitchState.TopSwitch && !SwitchState.BottomSwitch && !failed)
                    {
                        if (Nodes[i].status != DrillNode.DrillNodeStatus.Drilled)
                        {
                            Nodes[i].status = DrillNode.DrillNodeStatus.Next;
                            UpdateNodeColors();
                            logger1.AddLine("Moving to [" + i + "/" + listBox1.Items.Count + "]: " + Nodes[i].Location);
                            Refresh();

                            Enabled = false;
                            MoveTo(Nodes[i].location.X, Nodes[i].location.Y);

                            logger1.AddLine("Drilling...");
                            checkBoxT.Checked = true;
                            Refresh();


                            var maxTries = 10;
                            while (SwitchState.TopSwitch && !failed)
                            {
                                SwitchState = Transfer();
                                failed = !OutOfScopeUSB_IsOpen() || (maxTries < 0);
                                UIupdateTimer_Tick(this, null);
                                Refresh();
                                maxTries--;
                                Thread.Sleep(100);
                            }

                            checkBoxT.Checked = false;
                            Refresh();


                            maxTries = 10;
                            while (!SwitchState.TopSwitch && !failed)
                            {
                                SwitchState = Transfer();
                                failed = !OutOfScopeUSB_IsOpen() || (maxTries < 0);
                                UIupdateTimer_Tick(this, null);
                                Refresh();
                                maxTries--;
                                Thread.Sleep(100);
                            }
                            Nodes[i].status = DrillNode.DrillNodeStatus.Drilled;
                            UpdateNodeColors();
                            Refresh();
                        }
                        else
                        {
                            logger1.AddLine("Node [" + i + "/" + listBox1.Items.Count + "] already drilled.");
                        }
                    }
                }

                logger1.AddLine("Scripted Run Completed.");
            }
            else logger1.AddLine("Can't init drill cycle, limit switches are not properly set or USB interface is Closed.");
            Enabled = true;
        }


        private void UpdateNodeColors()
        {
            for (var i = 0; i < Nodes.Count; i++)
            {
                for (var j = 0; j < nodeViewer.Elements.Count; j++)
                {
                    if (nodeViewer.Elements[j].ID == Nodes[i].ID)
                        nodeViewer.Elements[j].color = Nodes[i].Color;
                }
            }
        }




        private void OptimizeButton_Click(object sender, EventArgs e)
        {
            var old_length = DrillNodeHelper.getPathLength(Nodes, CurrentLocation());

            var NodesNN = DrillNodeHelper.OptimizeNodesNN(Nodes, CurrentLocation());
            var NN_Length = DrillNodeHelper.getPathLength(NodesNN, CurrentLocation());

            var NodesHSL = DrillNodeHelper.OptimizeNodesHScanLine(Nodes, new PointF(0, 0));
            var HSL_Length = DrillNodeHelper.getPathLength(NodesHSL, CurrentLocation());

            var NodesVSL = DrillNodeHelper.OptimizeNodesVScanLine(Nodes, new PointF(0, 0));
            var VSL_Length = DrillNodeHelper.getPathLength(NodesVSL, CurrentLocation());

            var best_SL_length = (VSL_Length < HSL_Length) ? VSL_Length : HSL_Length;
            var best_SL_path = (VSL_Length < HSL_Length) ? NodesVSL : NodesHSL;

            var best_length = (NN_Length < best_SL_length) ? NN_Length : best_SL_length;
            var best_path = (NN_Length < best_SL_length) ? NodesNN : best_SL_path;

            if (best_length < old_length)
            {
                Nodes = best_path;

            } else logger1.AddLine("Optimization test returned path longer or equal.");

            RebuildListBoxAndViewerFromNodes();
        }



        //todo split optimization methods into separate methods and call all of them when Button is pressed
        //todo to run all methods and select shortest.

        //todo double click in output label set position as current
        //todo add grid, rulers and snapping to Viewer
        //todo select node by clicking in viewer (mouse up without panning)

        //todo offset nodes closer to margins / 6x6 table on load

        //todo extract methods to other class / files to clean this file

    }
}
