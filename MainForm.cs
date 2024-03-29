﻿using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;

[assembly: AssemblyVersion("2.2.*")]
namespace CNC_Drill_Controller1


{    

    public partial class MainForm : Form
    {
        #region USB Interface Properties
        //oncomplete property : XCOPY "$(TargetDir)*.exe" "Z:\" /Y /I
        public IUSB_Controller USB = new USB_Control();
        private DateTime lastUIupdate;

        #endregion

        #region UI properties

        private bool CheckBoxInhibit;
        private char[] trimChars = { ' ' };
        private RawUSBForm RawUsbForm = new RawUSBForm {Visible = false};
        private TaskDialog taskDialog = new TaskDialog();
        private bool globalCtrl = false;

        #endregion

        #region View Properties

        private DrawingTypeDialog dtypeDialog = new DrawingTypeDialog();
        private AddNodesDialog daddNodesDialog = new AddNodesDialog();
        private const float NodeDiameter = 0.05f;
        private Viewer nodeViewer;
        private CrossHair cursorCrossHair;
        private CrossHair drillCrossHair;
        private Box CNCTableBox;
        private Box drawingPageBox;
        private Cross moveTarget;

        #endregion

        #region Async Worker, Callback and Thread Sync Properties

        private TaskContainer TaskRunner;

        //for UI
        private Void_IntBoolDelegate ProgressCallback;
        private UpdateNodeDelegate UpdateNodeCallback;
        private Void_VoidDelgetate HideBox;
        private Void_VoidDelgetate ShowBox;

        #endregion

        #region Form Initialization

        public MainForm()
        {
            InitializeComponent();
            FormClosing += OnFormClosing;

            ProgressCallback = progressCallback;
            UpdateNodeCallback = updateNodeCallback;
            HideBox = hideBox;
            ShowBox = showBox;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ExtLog.Logger = logger1;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Text += ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4);
            }
            else
            {
                Text += Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            //Thread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            //Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            #region View initialization

            cursorCrossHair = new CrossHair(0, 0, Color.Blue);
            drillCrossHair = new CrossHair(0, 0, Color.Red);
            CNCTableBox = new Box(0, 0, 6, 6, Color.LightGray);
            drawingPageBox = new Box(0, 0, 8.5f, 11, Color.GhostWhite);
            moveTarget = new Cross(0, 0, Color.Yellow);
            nodeViewer = new Viewer(OutputLabel, new PointF(12.0f, 12.0f), new PointF(3.0f, 3.0f));
            nodeViewer.OnSelect += OnSelect;
            RebuildListBoxAndViewerFromNodes();

            #endregion

            #region UI Initialization

            AxisOffsetComboBox.SelectedIndex = 0;
            XScaleTextBox.Text = GlobalProperties.X_Scale.ToString("F3", GlobalProperties.culture);
            YScaleTextBox.Text = GlobalProperties.Y_Scale.ToString("F3", GlobalProperties.culture);
            XBacklastTextbox.Text = GlobalProperties.X_Backlash.ToString("F3", GlobalProperties.culture);
            YBacklastTextbox.Text = GlobalProperties.Y_Backlash.ToString("F3", GlobalProperties.culture);

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
            else
            {
                USBdevicesComboBox.Items.Add("[None]");
                USB = new USB_Control_Emulator();
            }

            USB.OnProgress = OnProgress;
            USB.OnMove = onMove;
            USB.OnMoveCompleted = OnMoveCompleted;

            USB.X_StepMotor_Driver_Enable = radioButtonAxisX.Checked || radioButtonAxisAll.Checked;
            USB.Y_StepMotor_Driver_Enable = radioButtonAxisY.Checked || radioButtonAxisAll.Checked;
            USB.TQA_Driver_Enable = checkBoxT.Checked;
            USB.Cycle_Drill = checkBoxD.Checked;

            USB.Inhibit_Backlash_Compensation = IgnoreBacklashBox.Checked;

            TaskRunner = new TaskContainer(this, taskDialog) { UpdateNodes = OnUpdateNode };

            #endregion

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
            USB.CloseDevice();
            try
            {
                saveLogToolStripMenuItem_Click(sender, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while saving logfile", MessageBoxButtons.OK);
            }

            GlobalProperties.SaveProperties();
        }

        #endregion

        #region Event Callback and Thread Sync Methods

        private void OnProgress(int progress, bool done)
        {
            Invoke(ProgressCallback, new object[] { progress, done });
        }
        private void progressCallback(int progress, bool done)
        {
            toolStripProgressBar.Value = progress;
            UIupdateTimer_Tick(this, null);
        }

        private void showBox()
        {
         //   Nodes.Show();
        }

        private void hideBox()
        {
         //   Nodes.Hide();
        }
        public void OnUpdateNode(int nodeIndex, DrillNode.DrillNodeStatus newStatus)
        {
            Invoke(UpdateNodeCallback, new object[] { nodeIndex, newStatus });
        }
        private void updateNodeCallback(int nodeIndex, DrillNode.DrillNodeStatus newStatus)
        {            
            (Nodes.Items[nodeIndex] as DrillNode).status = newStatus;
            UpdateNodeColors();
        }

        #endregion

        #region Direct USB UI control methods

        private void PlusXbutton_Click(object sender, EventArgs e)
        {
            USB.MoveByStep(getAxisOffsetStepCount(GlobalProperties.X_Scale), 0);
        }
        private void MinusXbutton_Click(object sender, EventArgs e)
        {
            USB.MoveByStep(-getAxisOffsetStepCount(GlobalProperties.X_Scale), 0);
        }
        private void PlusYbutton_Click(object sender, EventArgs e)
        {
            USB.MoveByStep(0, getAxisOffsetStepCount(GlobalProperties.Y_Scale));
        }
        private void MinusYbutton_Click(object sender, EventArgs e)
        {
            USB.MoveByStep(0, -getAxisOffsetStepCount(GlobalProperties.Y_Scale));
        }
        private void checkBoxB_CheckedChanged(object sender, EventArgs e)
        {
            USB.X_StepMotor_Driver_Enable = radioButtonAxisX.Checked || radioButtonAxisAll.Checked;
            USB.Y_StepMotor_Driver_Enable = radioButtonAxisY.Checked || radioButtonAxisAll.Checked;
            USB.TQA_Driver_Enable = checkBoxT.Checked;
            USB.Cycle_Drill = checkBoxD.Checked;

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
        private void showRawCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (showRawCheckbox.Checked)
            {
                RawUsbForm.Show();
                RawUsbForm.Update(USB.InputBuffer);
            }
            else
            {
                RawUsbForm.Hide();
            }
        }

        private void setXButton_Click(object sender, EventArgs e)
        {
            // (loc - delta) / scale = pos
            // loc - delta = (pos * scale)
            // loc - (pos * scale) = delta
            USB.X_Delta = (int)Math.Round(USB.X_Abs_Location - (TextConverter.SafeTextToFloat(XCurrentPosTextBox.Text) * GlobalProperties.X_Scale));
        }
        private void SetYButton_Click(object sender, EventArgs e)
        {
            USB.Y_Delta = (int)Math.Round(USB.Y_Abs_Location - (TextConverter.SafeTextToFloat(YCurrentPosTextBox.Text) * GlobalProperties.Y_Scale));
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

        private int getAxisOffsetStepCount(float scale)
        {
            var selection = ((string)AxisOffsetComboBox.SelectedItem).Split(new[] { ' ' });
            try
            {
                var delta = selection[0];
                var units = selection[1];
                var count = Convert.ToSingle(delta);
                return ((units != null) && (units == "in")) ? (int)Math.Round(count * scale) : (int)count;
            }
            catch
            {
                return 1;
            }
        }
        private void AxisOffsetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void AbortMoveButton_Click(object sender, EventArgs e)
        {
            USB.CancelMove();
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
            logfile.WriteLine("Saving Log [" + DateTime.Now.ToString("F", GlobalProperties.culture) + "]");
            logfile.Write(logger1.Text);
            logfile.WriteLine("");
            logfile.Close();
            logger1.AddLine("Log Saved to " + GlobalProperties.Logfile_Filename);
        }
        #endregion

        #region UI Update and Settings

        private PointF GetViewCursorLocation()
        {
            var snapLocation = nodeViewer.ViewMousePosition;
            if (SnapViewBox.Checked)
            {
                var snapSize = TextConverter.SafeTextToFloat(SnapSizeTextBox.Text);
                if (Math.Abs(snapSize) > 0.001f)
                {
                    snapLocation.X = (float)Math.Round(snapLocation.X / snapSize) * snapSize;
                    snapLocation.Y = (float)Math.Round(snapLocation.Y / snapSize) * snapSize;
                }
            }

            return snapLocation;
        }

        private bool onMove(float X, float Y)
        {
            moveTarget.UpdatePosition(X, Y);
            if (InvokeRequired)
            {
                Nodes.BeginInvoke(HideBox);
            } else
            {
                Nodes.Hide();
            }
            return true;
        }

        private void OnMoveCompleted()
        {
            if (InvokeRequired)
            {
                Nodes.BeginInvoke(ShowBox);
            }
            else
            {
                Nodes.Show();
            }
        }

        private void XSetTransformButton_Click(object sender, EventArgs e)
        {
            GlobalProperties.X_Scale = TextConverter.SafeTextToFloat(XScaleTextBox.Text);
            GlobalProperties.X_Backlash = TextConverter.SafeTextToFloat(XBacklastTextbox.Text);
            GlobalProperties.X_Length = TextConverter.SafeTextToFloat(XLengthTextBox.Text);
            logger1.AddLine($"Set X Axis Scale to: {GlobalProperties.X_Scale} steps/inch, Backlash to: {GlobalProperties.X_Backlash} steps, Length to: {GlobalProperties.X_Length}");
            CNCTableBox.UpdateSize(GlobalProperties.X_Length, GlobalProperties.Y_Length);
        }

        private void YSetTransformButton_Click(object sender, EventArgs e)
        {
            GlobalProperties.Y_Scale = TextConverter.SafeTextToFloat(YScaleTextBox.Text);
            GlobalProperties.Y_Backlash = TextConverter.SafeTextToFloat(YBacklastTextbox.Text);
            GlobalProperties.Y_Length = TextConverter.SafeTextToFloat(YLengthTextBox.Text);
            logger1.AddLine($"Set Y Axis Scale to: {GlobalProperties.Y_Scale} steps/inch, Backlash to: {GlobalProperties.Y_Backlash} steps, Length to: {GlobalProperties.Y_Length}");
            CNCTableBox.UpdateSize(GlobalProperties.X_Length, GlobalProperties.Y_Length);
        }

        private void UIupdateTimer_Tick(object sender, EventArgs e)
        {
            #region UI update
            if (USB.IsOpen)
            {
                CheckBoxInhibit = true;
                //fetch data if too old
                if ((DateTime.Now.Subtract(USB.LastUpdate)).Milliseconds > GlobalProperties.USB_Refresh_Period)
                {
                    USB.Transfer();
                }

                if (RawUsbForm.Visible) RawUsbForm.Update(USB.InputBuffer);
                else
                {
                    showRawCheckbox.Checked = false;
                }

                XMinStatusLabel.BackColor = !USB.MinXswitch ? Color.Lime : Color.Red;
                XMaxStatusLabel.BackColor = !USB.MaxXswitch ? Color.Lime : Color.Red;

                YMinStatusLabel.BackColor = !USB.MinYswitch ? Color.Lime : Color.Red;
                YMaxStatusLabel.BackColor = !USB.MaxYswitch ? Color.Lime : Color.Red;

                TopStatusLabel.BackColor = USB.TopSwitch ? Color.Lime : SystemColors.Control;
                BottomStatusLabel.BackColor = USB.BottomSwitch ? Color.Lime : SystemColors.Control;

                //reset drill cycle
                if (checkBoxD.Checked && !USB.TopSwitch && !USB.BottomSwitch)
                {
                    checkBoxD.Checked = false;
                }

                CheckBoxInhibit = false;
            }
            #endregion

            #region View update

            XStatusLabel.Text = USB.X_Rel_Location.ToString("D5");
            YStatusLabel.Text = USB.Y_Rel_Location.ToString("D5");

            var curLoc = USB.CurrentLocation();
            Xlabel.Text = string.Format(GlobalProperties.culture, "X: {0,8:F3}", curLoc.X);
            Ylabel.Text = string.Format(GlobalProperties.culture, "Y: {0,8:F3}", curLoc.Y);

            var snapLocation = GetViewCursorLocation();
            cursorCrossHair.UpdatePosition(snapLocation);
            ViewXLabel.Text = snapLocation.X.ToString("F3", GlobalProperties.culture);
            ViewYLabel.Text = snapLocation.Y.ToString("F3", GlobalProperties.culture);
            ViewZoomLabel.Text = (int)Math.Round(nodeViewer.ZoomLevel * 100) + "%";

            drillCrossHair.UpdatePosition(curLoc.X, curLoc.Y);

            #endregion

            #region Refresh required elements

            if ((DateTime.Now.Subtract(lastUIupdate)).Milliseconds > GlobalProperties.Label_Refresh_Period)
            {

                OutputLabel.Refresh();
                Xlabel.Refresh();
                Ylabel.Refresh();
                statusStrip1.Refresh();
                logger1.Refresh();

                lastUIupdate = DateTime.Now;
                Application.DoEvents();
            }

            #endregion

            #region Backup state

            GlobalProperties.X_Dir = USB.X_Last_Direction;
            GlobalProperties.Y_Dir = USB.Y_Last_Direction;

            GlobalProperties.X_Pos = USB.X_Abs_Location;
            GlobalProperties.Y_Pos = USB.Y_Abs_Location;

            GlobalProperties.X_Delta = USB.X_Delta;
            GlobalProperties.Y_Delta = USB.Y_Delta;

            if ((DateTime.Now.Subtract(GlobalProperties.LastSave)).Milliseconds > GlobalProperties.GlobalProperties_Refresh_Period)
            {
                GlobalProperties.SaveProperties();
            }

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
            if (!globalCtrl) Nodes.SelectedIndices.Clear();
            for (var i = 0; i < Nodes.Items.Count; ++i)
            {
                if (selection.Exists( (val) => { return val.ID == i; } )) Nodes.SelectedIndices.Add(i);
            }
        }

        private void LoadFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (dtypeDialog.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFileDialog1.FileName))
                    {
                        ExtLog.AddLine("Opening File: " + openFileDialog1.FileName);

                        var loader = (INodeLoader) null;
                        if (openFileDialog1.FileName.ToUpperInvariant().EndsWith(".VDX")) loader = new VDXLoader();
                        else if (openFileDialog1.FileName.ToUpperInvariant().EndsWith(".SVG")) loader = new SVGLoader();
                        else if (openFileDialog1.FileName.ToUpperInvariant().EndsWith(".TXT")) loader = new GerberTXTLoader();
                        else ExtLog.AddLine("File Type not supported.");

                        if (loader != null)
                        {
                            loader.PageWidth = 11.0f;
                            loader.PageHeight = 11.0f;
                            loader.DrillNodes = new List<DrillNode>();

                            var dresult = dtypeDialog.DrawingConfig;
                            loader.Load(openFileDialog1.FileName, dresult);

                            Nodes.Items.Clear();
                            Nodes.Items.AddRange(loader.DrillNodes.ToArray());

                            if (dresult.reset_origin)
                            {
                                var leftmost = loader.PageWidth;
                                var topmost = loader.PageHeight;

                                foreach (DrillNode node in Nodes.Items)
                                {
                                    if (node.location.X < leftmost) leftmost = node.location.X;
                                    if (node.location.Y < topmost) topmost = node.location.Y;
                                }

                                XoriginTextbox.Text = (-leftmost + dresult.origin_x).ToString("F4", GlobalProperties.culture);
                                YoriginTextbox.Text = (-topmost + dresult.origin_y).ToString("F4", GlobalProperties.culture);
                                OffsetOriginBtton_Click(sender, e);
                            }

                            ExtLog.AddLine(Nodes.Items.Count.ToString("D") + " Nodes loaded.");
                            ExtLog.AddLine("Page Width: " + loader.PageWidth.ToString("F1", GlobalProperties.culture));
                            ExtLog.AddLine("Page Height: " + loader.PageHeight.ToString("F1", GlobalProperties.culture));

                            drawingPageBox = new Box(0, 0, loader.PageWidth, loader.PageHeight, Color.GhostWhite);
                            RebuildListBoxAndViewerFromNodes();
                        }
                    }
                }
            }
        }

        private void MoveTobutton_Click(object sender, EventArgs e)
        {
            var movedata = (string)Nodes.SelectedItem;

            var axisdata = movedata.Split(trimChars);
            if (axisdata.Length == 2)
            {
                var mx = TextConverter.SafeTextToFloat(axisdata[0].Trim(trimChars));
                var my = TextConverter.SafeTextToFloat(axisdata[1].Trim(trimChars));
                logger1.AddLine("Moving to: " + mx.ToString("F3", GlobalProperties.culture) + ", " + my.ToString("F3", GlobalProperties.culture));
                USB.MoveToPosition(mx, my);
            }
        }
        private void SetAsXYbutton_Click(object sender, EventArgs e)
        {
            var movedata = (string)Nodes.SelectedItem;
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
            NodesContextMenu.Show(Nodes, Nodes.PointToClient(Cursor.Position));
        }
        private void NodeContextTARGET_Click(object sender, EventArgs e)
        {
            foreach (int index in Nodes.SelectedIndices) (Nodes.Items[index] as DrillNode).status = DrillNode.DrillNodeStatus.Next;
            UpdateNodeColors();
        }
        private void NodeContextDRILED_Click(object sender, EventArgs e)
        {
            foreach (int index in Nodes.SelectedIndices) (Nodes.Items[index] as DrillNode).status = DrillNode.DrillNodeStatus.Drilled;
            UpdateNodeColors();
        }
        private void NodeContextIDLE_Click(object sender, EventArgs e)
        {
            foreach (int index in Nodes.SelectedIndices) (Nodes.Items[index] as DrillNode).status = DrillNode.DrillNodeStatus.Idle;
            UpdateNodeColors();
        }
        private void nodeContextDelete_Click(object sender, EventArgs e)
        {
            foreach (int index in Nodes.SelectedIndices) Nodes.Items.Remove(Nodes.Items[index]);
            RebuildListBoxAndViewerFromNodes();
        }

        private void UpdateNodeColors()
        {
            for (var i = 0; i < Nodes.Items.Count; i++)
            {
                for (var j = 0; j < nodeViewer.Elements.Count; j++)
                {
                    if (nodeViewer.Elements[j].ID == i) nodeViewer.Elements[j].color = (Nodes.Items[i] as DrillNode).Color;
                }
            }
        }

        private void RebuildListBoxAndViewerFromNodes()
        {
            nodeViewer.Elements = new List<IViewerElements>
            {
                drawingPageBox,
                CNCTableBox,
                moveTarget,
                drillCrossHair,
                cursorCrossHair
            };

            for (var i = 0; i < Nodes.Items.Count; i++)
            {
                var node = Nodes.Items[i] as DrillNode;
                nodeViewer.Elements.Add(new Node(node.location, NodeDiameter, node.Color, i));
            }

            Nodes.DrawMode = DrawMode.OwnerDrawFixed;
            Nodes.DrawMode = DrawMode.Normal;

        }

        private void OffsetOriginBtton_Click(object sender, EventArgs e)
        {
            var offsetX = TextConverter.SafeTextToFloat(XoriginTextbox.Text);
            var offsetY = TextConverter.SafeTextToFloat(YoriginTextbox.Text);

            foreach (DrillNode node in Nodes.Items)
            {
                node.location.X += offsetX;
                node.location.Y += offsetY;
            }
            RebuildListBoxAndViewerFromNodes();
            XoriginTextbox.Text = "0.000";
            YoriginTextbox.Text = "0.000";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var node in nodeViewer.Elements) node.isSelected = false;
            foreach (int index in Nodes.SelectedIndices)
            {
                for (var i = 0; i < nodeViewer.Elements.Count; ++i)
                {
                    if (nodeViewer.Elements[i].ID == index) nodeViewer.Elements[i].isSelected = true;
                }
            }
            OutputLabel.Refresh();
        }
        #endregion

        #region View / Output Label Context Menu Methods
        private void OutputLabel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ViewContextMenu.Show(OutputLabel, OutputLabel.PointToClient(Cursor.Position));
            }
        }
        private void ViewSetDRGOrigin_Click(object sender, EventArgs e)
        {
            var origOffset = GetViewCursorLocation();
            if (Nodes.Items.Count > 0) for (var i = 0; i < Nodes.Items.Count; i++)
            {
                    var node = Nodes.Items[i] as DrillNode;
                    node.location = new PointF(node.location.X - origOffset.X, node.location.Y - origOffset.Y);
            }
            RebuildListBoxAndViewerFromNodes();
            XoriginTextbox.Text = "0.000";
            YoriginTextbox.Text = "0.000";
        }
        private void ViewSetXYContext_Click(object sender, EventArgs e)
        {
            var newLocation = GetViewCursorLocation();
            XCurrentPosTextBox.Text = newLocation.X.ToString("F3", GlobalProperties.culture);
            setXButton_Click(sender, e);
            YCurrentPosTextBox.Text = newLocation.Y.ToString("F3", GlobalProperties.culture);
            SetYButton_Click(sender, e);
        }
        private void moveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var targetLocation = GetViewCursorLocation();
            logger1.AddLine("Moving to: " + targetLocation.X.ToString("F3", GlobalProperties.culture) + ", " + targetLocation.Y.ToString("F3", GlobalProperties.culture));
            USB.MoveToPosition(targetLocation.X, targetLocation.Y);
        }
        #endregion

        #region Path and Control helpers

        private void OptimizeButton_Click(object sender, EventArgs e)
        {
            if (Nodes.Items.Count > 0)
            {
                var nl = new List<DrillNode>(Nodes.Items.Count);
                foreach (DrillNode node in Nodes.Items) nl.Add(node);

                var old_length = DrillNodeHelper.getPathLength(nl, USB.CurrentLocation());

                var NodesNN = DrillNodeHelper.OptimizeNodesNN(nl, USB.CurrentLocation());
                var NN_Length = DrillNodeHelper.getPathLength(NodesNN, USB.CurrentLocation());

                var NodesHSL = DrillNodeHelper.OptimizeNodesHScanLine(nl, new PointF(0, 0));
                var HSL_Length = DrillNodeHelper.getPathLength(NodesHSL, USB.CurrentLocation());

                var NodesVSL = DrillNodeHelper.OptimizeNodesVScanLine(nl, new PointF(0, 0));
                var VSL_Length = DrillNodeHelper.getPathLength(NodesVSL, USB.CurrentLocation());

                var best_SL_length = (VSL_Length < HSL_Length) ? VSL_Length : HSL_Length;
                var best_SL_path = (VSL_Length < HSL_Length) ? NodesVSL : NodesHSL;

                var best_length = (NN_Length < best_SL_length) ? NN_Length : best_SL_length;
                var best_path = (NN_Length < best_SL_length) ? NodesNN : best_SL_path;

                if (best_length < old_length)
                {
                    Nodes.Items.Clear();
                    Nodes.Items.AddRange(best_path.ToArray());
                }

                else logger1.AddLine("Optimization test returned path longer or equal.");

                RebuildListBoxAndViewerFromNodes();
            } else logger1.AddLine("No nodes to optimize.");
        }

        #endregion

        #region Async Tasks Handlers

        #region Async Find Axis Origins
        private void AsyncStartFindOriginButton_Click(object sender, EventArgs e)
        {
            TaskRunner.startAsyncWorkerWithTask(
                "Seeking Axis Origins (Async)...",
                TaskRunner.asyncWorkerDoWork_FindAxisOrigin,
                asyncWorkerDoWork_FindAllAxisOrigin_Cleanup, 
                new Tuple<bool, bool>(true, true));
        }
        private void AsyncStartFindXOriginButton_Click(object sender, EventArgs e)
        {
            TaskRunner.startAsyncWorkerWithTask(
                "Seeking X Axis Origin (Async)...",
                TaskRunner.asyncWorkerDoWork_FindAxisOrigin,
                asyncWorkerDoWork_FindXAxisOrigin_Cleanup,
                new Tuple<bool, bool>(true, false));
        }
        private void AsyncStartFindYOriginButton_Click(object sender, EventArgs e)
        {
            TaskRunner.startAsyncWorkerWithTask(
                "Seeking Y Axis Origin (Async)...",
                TaskRunner.asyncWorkerDoWork_FindAxisOrigin,
                asyncWorkerDoWork_FindYAxisOrigin_Cleanup,
                new Tuple<bool, bool>(false, true));
        }
        private void asyncWorkerDoWork_FindAllAxisOrigin_Cleanup(bool success)
        {
            ExtLog.AddLine("Task Completed");
            if (success)
            {
                var loc = USB.CurrentLocation();
                ExtLog.AddLine($"Location Set to Zero, Origin was found at X={loc.X:F3} Y={loc.Y:F3}");
                zeroAllbutton_Click(this, null);
            }
            else ExtLog.AddLine("Origin not found (out of reach / farther than 1 inch from initial location)");
        }
        private void asyncWorkerDoWork_FindXAxisOrigin_Cleanup(bool success)
        {
            ExtLog.AddLine("Task Completed");
            if (success)
            {
                var loc = USB.CurrentLocation();
                ExtLog.AddLine($"X Location Set to Zero, Origin was found at X={loc.X:F3}");
                zeroXbutton_Click(this, null);
            }
            else ExtLog.AddLine("Origin not found (out of reach / farther than 1 inch from initial location)");
        }
        private void asyncWorkerDoWork_FindYAxisOrigin_Cleanup(bool success)
        {
            ExtLog.AddLine("Task Completed");
            if (success)
            {
                var loc = USB.CurrentLocation();
                ExtLog.AddLine($"Y Location Set to Zero, Origin was found at Y={loc.X:F3}");
                zeroYbutton_Click(this, null);
            }
            else ExtLog.AddLine("Origin not found (out of reach / farther than 1 inch from initial location)");
        }
        #endregion

        #region Async Node Drilling
        private void DrillAllNodebutton_Click(object sender, EventArgs e)
        {
            if (Nodes.Items.Count > 0)
            {
                var nodeArray = new List<DrillNode>();
                for (var index = 0; index < Nodes.Items.Count; ++index)
                {
                    var node = (Nodes.Items[index] as DrillNode);
                    node._originalIndex = index;
                    nodeArray.Add(node);
                }
                nodeViewer.FitContentToControl();
                TaskRunner.startAsyncWorkerWithTask(
                    "Drill All Nodes (Async)...",
                    TaskRunner.asyncWorkerDoWork_DrillList,
                    asyncWorkerDoWork_Drill_Cleanup,
                    nodeArray);
            }
            else ExtLog.AddLine("No Nodes to Drill");
        }
        private void asyncWorkerDoWork_Drill_Cleanup(bool success)
        {
            ExtLog.AddLine(success ? "Task Completed" : "Drill Sequence Failed");
        }
        private void AsyncDrillSelectedButton_Click(object sender, EventArgs e)
        {
            if (Nodes.SelectedIndices.Count > 0)
            {
                var nodeArray = new List<DrillNode>();
                foreach (int index in Nodes.SelectedIndices)
                {
                    var node = (Nodes.Items[index] as DrillNode);
                    node._originalIndex = index;
                    nodeArray.Add(node);
                }
                nodeViewer.FitContentToControl();
                TaskRunner.startAsyncWorkerWithTask(
                    "Drill Selected Nodes (Async)...",
                    TaskRunner.asyncWorkerDoWork_DrillList,
                    asyncWorkerDoWork_Drill_Cleanup,
                    nodeArray);
            }
            else ExtLog.AddLine("No Nodes Selected to Drill");
        }

        #endregion

        #region Async Find Axis Lengths
        private void AsyncStartFindLengthButton_Click(object sender, EventArgs e)
        {
            TaskRunner.startAsyncWorkerWithTask(
                "Seeking Axis Lengths (Async)...",
                TaskRunner.asyncWorkerDoWork_FindAxisLengths,
                asyncWorkerDoWork_FindAllAxisLengths_Cleanup, 
                new Tuple<bool, bool>(true, true));
        }
        private void AsyncStartFindXLengthButton_Click(object sender, EventArgs e)
        {
            TaskRunner.startAsyncWorkerWithTask(
                "Seeking Axis Lengths (Async)...",
                TaskRunner.asyncWorkerDoWork_FindAxisLengths,
                asyncWorkerDoWork_FindXAxisLength_Cleanup,
                new Tuple<bool, bool>(true, false));
        }

        private void AsyncStartFindYLengthButton_Click(object sender, EventArgs e)
        {
            TaskRunner.startAsyncWorkerWithTask(
                "Seeking Axis Lengths (Async)...",
                TaskRunner.asyncWorkerDoWork_FindAxisLengths,
                asyncWorkerDoWork_FindYAxisLength_Cleanup,
                new Tuple<bool, bool>(false, true));
        }

        private void asyncWorkerDoWork_FindAllAxisLengths_Cleanup(bool success)
        {
            ExtLog.AddLine("Task Completed");
            if (success)
            {
                var loc = USB.CurrentLocation();
                ExtLog.AddLine($"Lengths found as X={loc.X:F3} Y={loc.Y:F3}");
                GlobalProperties.X_Length = loc.X;
                XLengthTextBox.Text = loc.X.ToString("F3", GlobalProperties.culture);
                GlobalProperties.Y_Length = loc.Y;
                YLengthTextBox.Text = loc.Y.ToString("F3", GlobalProperties.culture);

            }
            else ExtLog.AddLine("Length not found (out of reach / farther than 10 inch from initial location)");
        }
        private void asyncWorkerDoWork_FindXAxisLength_Cleanup(bool success)
        {
            ExtLog.AddLine("Task Completed");
            if (success)
            {
                var loc = USB.CurrentLocation();
                ExtLog.AddLine($"Length found as X={loc.X:F3}");
                GlobalProperties.X_Length = loc.X;
                XLengthTextBox.Text = loc.X.ToString("F3", GlobalProperties.culture);
            }
            else ExtLog.AddLine("Length not found (out of reach / farther than 10 inch from initial location)");
        }

        private void asyncWorkerDoWork_FindYAxisLength_Cleanup(bool success)
        {
            ExtLog.AddLine("Task Completed");
            if (success)
            {
                var loc = USB.CurrentLocation();
                ExtLog.AddLine($"Length found as Y={loc.Y:F3}");
                GlobalProperties.Y_Length = loc.Y;
                YLengthTextBox.Text = loc.Y.ToString("F3", GlobalProperties.culture);
            }
            else ExtLog.AddLine("Length not found (out of reach / farther than 10 inch from initial location)");
        }
        #endregion

        #endregion

        private void ArrowCaptureTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) MinusYbutton_Click(sender, null);
            if (e.KeyCode == Keys.Down) PlusYbutton_Click(sender, null);
            if (e.KeyCode == Keys.Left) MinusXbutton_Click(sender, null);
            if (e.KeyCode == Keys.Right) PlusXbutton_Click(sender, null);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void ArrowCaptureTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void zoomToFitcontent_button_Click(object sender, EventArgs e)
        {
            nodeViewer.FitContentToControl();
        }


        private void AddNodeButton_Click(object sender, EventArgs e)
        {
            if (daddNodesDialog.ShowDialog() == DialogResult.OK) AddNodesFromDialog();
        }

        private void ClearNodesButton_Click(object sender, EventArgs e)
        {
            Nodes.Items.Clear();
            RebuildListBoxAndViewerFromNodes();
        }

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            var snapLocation = GetViewCursorLocation();
            if (daddNodesDialog.ShowDialog(snapLocation.X, snapLocation.Y) == DialogResult.OK) AddNodesFromDialog();
        }

        private void AddNodesFromDialog()
        {
            var x = daddNodesDialog.DialogData.x;
            var y = daddNodesDialog.DialogData.y;

            // create nodes from dialog data
            if (daddNodesDialog.DialogData.type == "DIP")
            {
                var s = TextConverter.SafeTextToFloat(daddNodesDialog.DialogData.spacing, 0.300f);
                // DIP
                if (daddNodesDialog.DialogData.direction == "Horizontal")
                {
                    // horizontal
                    for (var i = 0; i < daddNodesDialog.DialogData.pins / 2; ++i)
                    {
                        Nodes.Items.Add(new DrillNode(new PointF(x, y)));
                        Nodes.Items.Add(new DrillNode(new PointF(x, y + s)));
                        x += 0.100f;
                    }
                }
                else
                {
                    // vertical
                    for (var i = 0; i < daddNodesDialog.DialogData.pins / 2; ++i)
                    {
                        Nodes.Items.Add(new DrillNode(new PointF(x, y)));
                        Nodes.Items.Add(new DrillNode(new PointF(x + s, y)));
                        y += 0.100f;
                    }
                }
            } else
            {
                // SIP
                if (daddNodesDialog.DialogData.direction == "Horizontal")
                {
                    // horizontal
                    for (var i = 0; i < daddNodesDialog.DialogData.pins; ++i)
                    {
                        Nodes.Items.Add(new DrillNode(new PointF(x, y)));
                        x += 0.100f;
                    }
                } else
                {
                    // vertical
                    for (var i = 0; i < daddNodesDialog.DialogData.pins; ++i)
                    {
                        Nodes.Items.Add(new DrillNode(new PointF(x, y)));
                        y += 0.100f;
                    }
                }
            }
            RebuildListBoxAndViewerFromNodes();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) globalCtrl = true;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) globalCtrl = false;
        }
    }
}
