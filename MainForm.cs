using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace CNC_Drill_Controller1
{
    public partial class MainForm : Form
    {
        #region USB Interface Properties
        //oncomplete property : XCOPY "$(TargetDir)*.exe" "Z:\" /Y /I
        private USB_Control USB = new USB_Control();
        private int AxisOffsetCount;

        #endregion

        #region UI properties

        private bool CheckBoxInhibit;
        private DrillNode.DrillNodeStatus lastSelectedStatus;
        private int lastSelectedIndex;
        private char[] trimChars = { ' ' };

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

        #region Form Initialization

        public MainForm()
        {
            InitializeComponent();

            GlobalProperties.Logfile_Filename = (string)Properties.Settings.Default["Logfile_Filename"];
            GlobalProperties.X_Scale = (int)Properties.Settings.Default["X_Scale"];
            GlobalProperties.Y_Scale = (int)Properties.Settings.Default["Y_Scale"];
            GlobalProperties.X_Backlash = (int)Properties.Settings.Default["X_Backlash"];
            GlobalProperties.Y_Backlash = (int)Properties.Settings.Default["Y_Backlash"];
            FormClosing += OnFormClosing;
            USB.OnProgress += OnProgress;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ExtLog.Logger = logger1;
            Text +=Assembly.GetExecutingAssembly()
                                           .GetName()
                                           .Version
                                           .ToString();

            #region View initialization

            cursorCrossHair = new CrossHair(0, 0, Color.Blue);
            drillCrossHair = new CrossHair(0, 0, Color.Red);
            CNCTableBox = new Box(0, 0, 6, 6, Color.LightGray);
            drawingPageBox = new Box(0, 0, 8.5f, 11, Color.GhostWhite);
            nodeViewer = new Viewer(OutputLabel, new PointF(11.0f, 11.0f));
            nodeViewer.OnSelect += OnSelect;
            lastSelectedStatus = DrillNode.DrillNodeStatus.Idle;
            lastSelectedIndex = -1;
            RebuildListBoxAndViewerFromNodes();

            #endregion

            #region UI Initialization

            AxisOffsetComboBox.SelectedIndex = 0;
            AxisOffsetCount = 1;
            XScaleTextBox.Text = GlobalProperties.X_Scale.ToString("D");
            YScaleTextBox.Text = GlobalProperties.Y_Scale.ToString("D");
            XBacklastTextbox.Text = GlobalProperties.X_Backlash.ToString("D");
            YBacklastTextbox.Text = GlobalProperties.Y_Backlash.ToString("D");

            #endregion

            #region USB interface initialization

            USBdevicesComboBox.Items.Clear();
            var USBDevices = USB.GetDevicesList();
            if (USBDevices.Count > 0)
            {
                foreach (var uDev in USBDevices)
                {
                    USBdevicesComboBox.Items.Add(uDev);
                }
                USBdevicesComboBox.SelectedIndex = 0;
            }
            else USBdevicesComboBox.Items.Add("[None]");

            USB.SwitchesOutput.X_Driver = checkBoxX.Checked;
            USB.SwitchesOutput.Y_Driver = checkBoxY.Checked;
            USB.SwitchesOutput.Cycle_Drill = checkBoxD.Checked;

            #endregion
        }

        private void OnProgress(int progress, bool done)
        {
            Cursor = (done) ? Cursors.Default : Cursors.WaitCursor;
            toolStripProgressBar1.Value = progress;
            UIupdateTimer_Tick(this, null);
        }

        private void USBdevicesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var locStr = (string)USBdevicesComboBox.SelectedItem;
            locStr = locStr.Split(new[] { ':' })[0];
            uint loc;
            if (uint.TryParse(locStr, out loc)) USB.OpenDeviceByLocation(loc);
            else
            {
                logger1.AddLine("Failed to parse Location Id of: " + (string)USBdevicesComboBox.SelectedItem);
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs formClosingEventArgs)
        {
            try
            {
                saveLogToolStripMenuItem_Click(sender, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while saving logfile", MessageBoxButtons.OK);
            }

            Properties.Settings.Default["Logfile_Filename"] = GlobalProperties.Logfile_Filename;
            Properties.Settings.Default["X_Scale"] = GlobalProperties.X_Scale;
            Properties.Settings.Default["Y_Scale"] = GlobalProperties.Y_Scale;
            Properties.Settings.Default["X_Backlash"] = GlobalProperties.X_Backlash;
            Properties.Settings.Default["Y_Backlash"] = GlobalProperties.Y_Backlash;
            Properties.Settings.Default.Save();
        }
        
        #endregion
     
        #region Direct USB UI control methods

        private void PlusXbutton_Click(object sender, EventArgs e)
        {
            USB.MoveBy(AxisOffsetCount, 0);
        }
        private void MinusXbutton_Click(object sender, EventArgs e)
        {
            USB.MoveBy(-AxisOffsetCount, 0);
        }
        private void PlusYbutton_Click(object sender, EventArgs e)
        {
            USB.MoveBy(0, AxisOffsetCount);
        }
        private void MinusYbutton_Click(object sender, EventArgs e)
        {
            USB.MoveBy(0, -AxisOffsetCount);
        }
        private void checkBoxB_CheckedChanged(object sender, EventArgs e)
        {
            USB.SwitchesOutput.X_Driver = checkBoxX.Checked;
            USB.SwitchesOutput.Y_Driver = checkBoxY.Checked;
            USB.SwitchesOutput.Cycle_Drill = checkBoxD.Checked;

            if (!CheckBoxInhibit) USB.Transfer();
        }
        private void Sendbutton_Click(object sender, EventArgs e)
        {
            USB.Transfer();
        }
        private void ReloadUSBbutton_Click(object sender, EventArgs e)
        {
            Form1_Load(this, e);
        }
        private void IgnoreBacklashBox_CheckedChanged(object sender, EventArgs e)
        {
            USB.Inhibit_Backlash_Compensation = IgnoreBacklashBox.Checked;
        }
        private void IgnoreSyncCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            USB.Inhibit_Sync = IgnoreSyncCheckBox.Checked;
        }


        private void setXButton_Click(object sender, EventArgs e)
        {
            // (loc - delta) / scale = pos
            // loc - delta = (pos * scale)
            // loc - (pos * scale) = delta
            USB.X_Delta = (int)(USB.X_Abs_Location - (safeTextToFloat(XCurrentPosTextBox.Text) * GlobalProperties.X_Scale));
        }
        private void SetYButton_Click(object sender, EventArgs e)
        {
            USB.Y_Delta = (int)(USB.Y_Abs_Location - (safeTextToFloat(YCurrentPosTextBox.Text) * GlobalProperties.Y_Scale));
        }

        private void SetAllButton_Click(object sender, EventArgs e)
        {
            setXButton_Click(sender, e);
            SetYButton_Click(sender, e);
        }

        private void zeroXbutton_Click(object sender, EventArgs e)
        {
            XCurrentPosTextBox.Text = "0.000";
            setXButton_Click(this, e);
        }
        private void zeroYbutton_Click(object sender, EventArgs e)
        {
            YCurrentPosTextBox.Text = "0.000";
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
        
        #endregion

        #region ConversionHelpers
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
        private int safeTextToInt(string text)
        {
            int res;
            if (int.TryParse(text, out res))
            {
                return res;
            }
            logger1.AddLine("Failed to convert value: " + text);
            return 0;
        }
        #endregion

        #region Log methods
        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logger1.Clear();
        }

        private void saveLogToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var logfile = (File.Exists(GlobalProperties.Logfile_Filename)) ? File.AppendText(GlobalProperties.Logfile_Filename) : File.CreateText(GlobalProperties.Logfile_Filename);
            logfile.WriteLine("Saving Log [" + DateTime.Now.ToString("F") + "]");
            logfile.Write(logger1.Text);
            logfile.WriteLine("");
            logfile.Close();
            logger1.AddLine("Log Saved to " + GlobalProperties.Logfile_Filename);
        }
        #endregion

        #region UI Update and Settings

        private PointF GetViewCursorLocation()
        {
            var snapLocation = nodeViewer.MousePositionF;
            if (SnapViewBox.Checked)
            {
                var snapSize = safeTextToFloat(SnapSizeTextBox.Text);
                snapLocation.X = (float)Math.Round(snapLocation.X / snapSize) * snapSize;
                snapLocation.Y = (float)Math.Round(snapLocation.Y / snapSize) * snapSize;
            }

            return snapLocation;
        }

        private void XSetTransformButton_Click(object sender, EventArgs e)
        {
            GlobalProperties.X_Scale = safeTextToInt(XScaleTextBox.Text);
            GlobalProperties.X_Backlash = safeTextToInt(XBacklastTextbox.Text);
            logger1.AddLine("Set X Axis Scale to: " + GlobalProperties.X_Scale + " steps/inch, Backlash to: " + GlobalProperties.X_Backlash + "steps.");
        }

        private void YSetTransformButton_Click(object sender, EventArgs e)
        {
            GlobalProperties.Y_Scale = safeTextToInt(YScaleTextBox.Text);
            GlobalProperties.Y_Backlash = safeTextToInt(YBacklastTextbox.Text);
            logger1.AddLine("Set Y Axis Scale to: " + GlobalProperties.Y_Scale + " steps/inch, Backlash to: " + GlobalProperties.Y_Backlash + "steps.");
        }

        private void UIupdateTimer_Tick(object sender, EventArgs e)
        {
            #region UI update
            if (USB.IsOpen)
            {
                CheckBoxInhibit = true;
                //fetch data if too old
                if ((DateTime.Now.Subtract(USB.LastUpdate)).Milliseconds > 250)
                {
                    USB.Transfer();
                }

                if (USB.SwitchesInput.MinXswitch && USB.SwitchesInput.MaxXswitch) //check for impossible combinaison (step controller or power not plugged-in)
                {
                    XMinStatusLabel.BackColor = Color.DodgerBlue;
                    XMaxStatusLabel.BackColor = Color.DodgerBlue;
                }
                else
                {
                    XMinStatusLabel.BackColor = !USB.SwitchesInput.MinXswitch ? Color.Lime : Color.Red;
                    XMaxStatusLabel.BackColor = !USB.SwitchesInput.MaxXswitch ? Color.Lime : Color.Red;
                }

                if (USB.SwitchesInput.MinYswitch && USB.SwitchesInput.MaxYswitch)
                {
                    YMinStatusLabel.BackColor = Color.DodgerBlue;
                    YMaxStatusLabel.BackColor = Color.DodgerBlue;
                }
                else
                {
                    YMinStatusLabel.BackColor = !USB.SwitchesInput.MinYswitch ? Color.Lime : Color.Red;
                    YMaxStatusLabel.BackColor = !USB.SwitchesInput.MaxYswitch ? Color.Lime : Color.Red;
                }


                //top drill limit switch
                if (!USB.SwitchesInput.TopSwitch)
                {
                    if (checkBoxD.Checked)
                    {
                        checkBoxD.Checked = false;
                    }
                    TopStatusLabel.BackColor = SystemColors.Control;
                }
                else TopStatusLabel.BackColor = Color.Lime;

                //bottom drill limit switch
                if (!USB.SwitchesInput.BottomSwitch)
                {
                    if (checkBoxB.Checked)
                    {
                        checkBoxB.Checked = false;
                    }
                    BottomStatusLabel.BackColor = SystemColors.Control;
                }
                else BottomStatusLabel.BackColor = Color.Lime;

                XSyncStatusLabel.BackColor = USB.SwitchesInput.SyncXswitch ? Color.Lime : SystemColors.Control;
                YSyncStatusLabel.BackColor = USB.SwitchesInput.SyncYswitch ? Color.Lime : SystemColors.Control;

                CheckBoxInhibit = false;
            }
            #endregion

            #region View update

            XStatusLabel.Text = USB.X_Rel_Location.ToString("D5");
            YStatusLabel.Text = USB.Y_Rel_Location.ToString("D5");

            var curLoc = USB.CurrentLocation();
            Xlabel.Text = "X: " + curLoc.X.ToString("F3");
            Ylabel.Text = "Y: " + curLoc.Y.ToString("F3");

            var snapLocation = GetViewCursorLocation();
            cursorCrossHair.UpdatePosition(snapLocation);
            ViewXLabel.Text = snapLocation.X.ToString("F3");
            ViewYLabel.Text = snapLocation.Y.ToString("F3");
            ViewZoomLabel.Text = (int)(nodeViewer.ZoomLevel * 100) + "%";

            drillCrossHair.UpdatePosition(curLoc.X, curLoc.Y);

#endregion

            #region Refresh required elements

            OutputLabel.Refresh();
            Xlabel.Refresh();
            Ylabel.Refresh();
            statusStrip1.Refresh();
            logger1.Refresh();

            #endregion
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

        private void OnSelect(List<IViewerElements> selection)
        {
            if ((Nodes != null) && (Nodes.Count > 0))
            for (var i = 0; i < selection.Count; i++)
            {
                for (var j = 0; j < Nodes.Count; j++)
                {
                    if (selection[i].ID == Nodes[j].ID)
                    {
                        listBox1.SelectedIndex = Nodes[j].ID;
                    }
                }
            }
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
                logger1.AddLine("Moving to: " + mx.ToString("F3") + ", " + my.ToString("F3"));
                USB.MoveTo(mx, my);
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
            XoriginTextbox.Text = "0.000";
            YOriginTextbox.Text = "0.000";
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
        
        #region View / Output Label Context Menu Methods
        private void OutputLabel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ViewContextMenu.Show(OutputLabel,  OutputLabel.PointToClient(Cursor.Position));
            }
        }
        private void ViewSetDRGOrigin_Click(object sender, EventArgs e)
        {
            var origOffset = GetViewCursorLocation();
            for (var i = 0; i < Nodes.Count; i++)
                Nodes[i].location = new PointF(Nodes[i].location.X - origOffset.X, Nodes[i].location.Y - origOffset.Y);
            RebuildListBoxAndViewerFromNodes();
            XoriginTextbox.Text = "0.000";
            YOriginTextbox.Text = "0.000";
        }
        private void ViewSetXYContext_Click(object sender, EventArgs e)
        {
            var newLocation = GetViewCursorLocation();
            XCurrentPosTextBox.Text = newLocation.X.ToString("F3");
            setXButton_Click(sender, e);
            YCurrentPosTextBox.Text = newLocation.Y.ToString("F3");
            SetYButton_Click(sender, e);
        }
        private void moveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var targetLocation = GetViewCursorLocation();
            logger1.AddLine("Moving to: " + targetLocation.X.ToString("F3") + ", " + targetLocation.Y.ToString("F3"));
            USB.MoveTo(targetLocation.X, targetLocation.Y);
        }
        #endregion

        #region Scripted Methods

        private void OptimizeButton_Click(object sender, EventArgs e)
        {
            var old_length = DrillNodeHelper.getPathLength(Nodes, USB.CurrentLocation());

            var NodesNN = DrillNodeHelper.OptimizeNodesNN(Nodes, USB.CurrentLocation());
            var NN_Length = DrillNodeHelper.getPathLength(NodesNN, USB.CurrentLocation());

            var NodesHSL = DrillNodeHelper.OptimizeNodesHScanLine(Nodes, new PointF(0, 0));
            var HSL_Length = DrillNodeHelper.getPathLength(NodesHSL, USB.CurrentLocation());

            var NodesVSL = DrillNodeHelper.OptimizeNodesVScanLine(Nodes, new PointF(0, 0));
            var VSL_Length = DrillNodeHelper.getPathLength(NodesVSL, USB.CurrentLocation());

            var best_SL_length = (VSL_Length < HSL_Length) ? VSL_Length : HSL_Length;
            var best_SL_path = (VSL_Length < HSL_Length) ? NodesVSL : NodesHSL;

            var best_length = (NN_Length < best_SL_length) ? NN_Length : best_SL_length;
            var best_path = (NN_Length < best_SL_length) ? NodesNN : best_SL_path;

            if (best_length < old_length)
            {
                Nodes = best_path;
            }

            else logger1.AddLine("Optimization test returned path longer or equal.");

            RebuildListBoxAndViewerFromNodes();
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
            var failed = !USB.IsOpen;

            if (!USB.SwitchesInput.MaxXswitch && !USB.SwitchesInput.MinXswitch && !USB.SwitchesInput.MaxYswitch && !USB.SwitchesInput.MinYswitch && USB.SwitchesInput.TopSwitch && !USB.SwitchesInput.BottomSwitch && !failed)
            {
                Nodes[listBox1.SelectedIndex].status = DrillNode.DrillNodeStatus.Next;
                UpdateNodeColors();
                logger1.AddLine("Moving to: " + Nodes[listBox1.SelectedIndex].Location);
                UIupdateTimer_Tick(sender, e);

                Enabled = false;
                USB.MoveTo(Nodes[listBox1.SelectedIndex].location.X, Nodes[listBox1.SelectedIndex].location.Y);

                logger1.AddLine("Drilling...");
                checkBoxD.Checked = true;
                UIupdateTimer_Tick(sender, e);

                var maxTries = 20;
                while (USB.SwitchesInput.TopSwitch && !failed)
                {
                    USB.Transfer();
                    failed = !USB.IsOpen || (maxTries < 0);
                    UIupdateTimer_Tick(sender, e);
                    maxTries--;
                    Thread.Sleep(50);
                }

                checkBoxD.Checked = false;
                Refresh();

                maxTries = 20;
                while (!USB.SwitchesInput.TopSwitch && !failed)
                {
                    USB.Transfer();
                    failed = !USB.IsOpen || (maxTries < 0);
                    UIupdateTimer_Tick(sender, e);
                    maxTries--;
                    Thread.Sleep(50);
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
            var failed = !USB.IsOpen;

            if (!USB.SwitchesInput.MaxXswitch && !USB.SwitchesInput.MinXswitch && !USB.SwitchesInput.MaxYswitch &&
                !USB.SwitchesInput.MinYswitch && USB.SwitchesInput.TopSwitch && !USB.SwitchesInput.BottomSwitch && !failed)
            {
                for (var i = 0; i < listBox1.Items.Count; i++)
                {
                    if (!USB.SwitchesInput.MaxXswitch && !USB.SwitchesInput.MinXswitch && !USB.SwitchesInput.MaxYswitch &&
                        !USB.SwitchesInput.MinYswitch && USB.SwitchesInput.TopSwitch && !USB.SwitchesInput.BottomSwitch && !failed)
                    {
                        if (Nodes[i].status != DrillNode.DrillNodeStatus.Drilled)
                        {
                            Nodes[i].status = DrillNode.DrillNodeStatus.Next;
                            UpdateNodeColors();
                            logger1.AddLine("Moving to [" + (i+1) + "/" + listBox1.Items.Count + "]: " + Nodes[i].Location);
                            UIupdateTimer_Tick(sender, e);

                            Enabled = false;
                            USB.MoveTo(Nodes[i].location.X, Nodes[i].location.Y);

                            logger1.AddLine("Drilling...");
                            checkBoxD.Checked = true;
                            UIupdateTimer_Tick(sender, e);


                            var maxTries = 20;
                            while (USB.SwitchesInput.TopSwitch && !failed)
                            {
                                USB.Transfer();
                                failed = !USB.IsOpen || (maxTries < 0);
                                UIupdateTimer_Tick(sender, e);
                                maxTries--;
                                Thread.Sleep(50);
                            }

                            checkBoxD.Checked = false;
                            Refresh();

                            maxTries = 20;
                            while (!USB.SwitchesInput.TopSwitch && !failed)
                            {
                                USB.Transfer();
                                failed = !USB.IsOpen || (maxTries < 0);
                                UIupdateTimer_Tick(sender, e);
                                maxTries--;
                                Thread.Sleep(50);
                            }
                            Nodes[i].status = DrillNode.DrillNodeStatus.Drilled;
                            UpdateNodeColors();
                            UIupdateTimer_Tick(sender, e);
                        }
                        else
                        {
                            logger1.AddLine("Node [" + (i+1) + "/" + listBox1.Items.Count + "] already drilled.");
                        }
                    }
                }

                logger1.AddLine("Scripted Run Completed.");
            }
            else logger1.AddLine("Can't init drill cycle, limit switches are not properly set or USB interface is Closed.");
            Enabled = true;
        }

        private void SeekZeroButton_Click(object sender, EventArgs e)
        {
            logger1.AddLine("Seeking Axis Origins...");

            if (!USB.SwitchesInput.MaxXswitch && !USB.SwitchesInput.MinXswitch &&
                !USB.SwitchesInput.MaxYswitch && !USB.SwitchesInput.MinYswitch && USB.IsOpen)
            {
                USB.Inhibit_LimitSwitches_Warning = true;
                Enabled = false;
                var failed = false;

                var delta_X = (USB.X_Rel_Location > GlobalProperties.X_Scale) ? USB.X_Rel_Location - GlobalProperties.X_Scale : 0;
                var delta_Y = (USB.Y_Rel_Location > GlobalProperties.Y_Scale) ? USB.Y_Rel_Location - GlobalProperties.Y_Scale : 0;

                USB.MoveBy(-delta_X, -delta_Y);
                UIupdateTimer_Tick(sender, e);

                var maxTries = 48;
                while (!USB.SwitchesInput.MinXswitch && USB.IsOpen && (maxTries > 0))
                {
                    USB.MoveBy(-24, 0);
                    maxTries--;
                    Thread.Sleep(50);
                    USB.Transfer();
                    UIupdateTimer_Tick(sender, e);
                    failed = maxTries <= 0;
                }

                maxTries = 48;
                while (USB.SwitchesInput.MinXswitch && USB.IsOpen && (maxTries > 0) && !failed)
                {
                    USB.MoveBy(1, 0);
                    Thread.Sleep(50);
                    USB.Transfer();
                    UIupdateTimer_Tick(sender, e);
                    failed = maxTries <= 0;
                }
                Refresh();
                maxTries = 48;
                while (!USB.SwitchesInput.MinYswitch && USB.IsOpen && (maxTries > 0) && !failed)
                {
                    USB.MoveBy(0, -24);
                    Thread.Sleep(50);
                    USB.Transfer();
                    UIupdateTimer_Tick(sender, e);
                    failed = maxTries <= 0;
                }

                maxTries = 48;
                while (USB.SwitchesInput.MinYswitch && USB.IsOpen && (maxTries > 0) && !failed)
                {
                    USB.MoveBy(0, 1);
                    Thread.Sleep(50);
                    USB.Transfer();
                    UIupdateTimer_Tick(sender, e);
                    failed = maxTries <= 0;
                }

                if (!failed)
                {
                    var loc = USB.CurrentLocation();
                    logger1.AddLine("Location Set to Zero, Origin was found at X=" + loc.X.ToString("F3") + " Y="+loc.Y.ToString("F3"));
                    zeroAllbutton_Click(sender, e);
                    logger1.AddLine("Scripted Run Completed.");
                }
                else logger1.AddLine("Origin not found (out of reach / farther than 1 inch from expected location)");
            }
            else logger1.AddLine("Can't init scripted sequence, limit switches are not properly set or USB interface is Closed.");
            Enabled = true;
            USB.Inhibit_LimitSwitches_Warning = false;
        }

        #endregion




        //todo offset nodes closer to margins / 6x6 table on load
    }
}
