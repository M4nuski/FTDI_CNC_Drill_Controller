namespace CNC_Drill_Controller1
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.USBdevicesComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.logger1 = new M4nuskomponents.Logger();
            this.MinusYbutton = new System.Windows.Forms.Button();
            this.PlusYbutton = new System.Windows.Forms.Button();
            this.PlusXbutton = new System.Windows.Forms.Button();
            this.MinusXbutton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.XMinStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.XMaxStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.YMinStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.YMaxStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.TopStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.BottomStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.XStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.YStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxX = new System.Windows.Forms.CheckBox();
            this.checkBoxY = new System.Windows.Forms.CheckBox();
            this.checkBoxD = new System.Windows.Forms.CheckBox();
            this.forcePullButton = new System.Windows.Forms.Button();
            this.AxisOffsetComboBox = new System.Windows.Forms.ComboBox();
            this.bevel1 = new System.Windows.Forms.Label();
            this.UIupdateTimer = new System.Windows.Forms.Timer(this.components);
            this.loadFileButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.XScaleTextBox = new System.Windows.Forms.TextBox();
            this.YScaleTextBox = new System.Windows.Forms.TextBox();
            this.XCurrentPosTextBox = new System.Windows.Forms.TextBox();
            this.YCurrentPosTextBox = new System.Windows.Forms.TextBox();
            this.setXButton = new System.Windows.Forms.Button();
            this.SetYButton = new System.Windows.Forms.Button();
            this.zeroXbutton = new System.Windows.Forms.Button();
            this.zeroYbutton = new System.Windows.Forms.Button();
            this.zeroAllbutton = new System.Windows.Forms.Button();
            this.Xlabel = new System.Windows.Forms.Label();
            this.Ylabel = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.NodesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.NodeContextSETXY = new System.Windows.Forms.ToolStripMenuItem();
            this.NodeContextMOVETO = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.NodeContextIDLE = new System.Windows.Forms.ToolStripMenuItem();
            this.NodeContextDRILED = new System.Windows.Forms.ToolStripMenuItem();
            this.NodeContextTARGET = new System.Windows.Forms.ToolStripMenuItem();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.XSetTranformButton = new System.Windows.Forms.Button();
            this.YSetTranformButton = new System.Windows.Forms.Button();
            this.ViewZoomLabel = new System.Windows.Forms.Label();
            this.ViewXLabel = new System.Windows.Forms.Label();
            this.ViewYLabel = new System.Windows.Forms.Label();
            this.ReloadUSBbutton = new System.Windows.Forms.Button();
            this.OffsetOriginBtton = new System.Windows.Forms.Button();
            this.YoriginTextbox = new System.Windows.Forms.TextBox();
            this.XoriginTextbox = new System.Windows.Forms.TextBox();
            this.OptimizeButton = new System.Windows.Forms.Button();
            this.ViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ViewSetXYContext = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewSetDRGOrigin = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SnapSizeTextBox = new System.Windows.Forms.TextBox();
            this.SnapViewBox = new System.Windows.Forms.CheckBox();
            this.SetAllButton = new System.Windows.Forms.Button();
            this.YBacklastTextbox = new System.Windows.Forms.TextBox();
            this.XBacklastTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.IgnoreBacklashBox = new System.Windows.Forms.CheckBox();
            this.checkBoxT = new System.Windows.Forms.CheckBox();
            this.showRawCheckbox = new System.Windows.Forms.CheckBox();
            this.AsyncStartFindOriginButton = new System.Windows.Forms.Button();
            this.AsyncDrillSelectedButton = new System.Windows.Forms.Button();
            this.DrillAllNodebutton = new System.Windows.Forms.Button();
            this.AbortMoveButton = new System.Windows.Forms.Button();
            this.ArrowCaptureTextbox = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.NodesContextMenu.SuspendLayout();
            this.ViewContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // USBdevicesComboBox
            // 
            this.USBdevicesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.USBdevicesComboBox.FormattingEnabled = true;
            this.USBdevicesComboBox.Location = new System.Drawing.Point(85, 5);
            this.USBdevicesComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.USBdevicesComboBox.Name = "USBdevicesComboBox";
            this.USBdevicesComboBox.Size = new System.Drawing.Size(912, 21);
            this.USBdevicesComboBox.TabIndex = 0;
            this.USBdevicesComboBox.Text = "[None]";
            this.USBdevicesComboBox.SelectedIndexChanged += new System.EventHandler(this.USBdevicesComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "USB Interface";
            // 
            // logger1
            // 
            this.logger1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logger1.Count = 0;
            this.logger1.CountStamp = false;
            this.logger1.CountStampFormat = "D4";
            this.logger1.DateStamp = false;
            this.logger1.DateStampFormat = "yyyy-MM-dd";
            this.logger1.Font = new System.Drawing.Font("Lucida Console", 9F);
            this.logger1.Location = new System.Drawing.Point(4, 573);
            this.logger1.Margin = new System.Windows.Forms.Padding(2);
            this.logger1.Multiline = true;
            this.logger1.Name = "logger1";
            this.logger1.ReadOnly = true;
            this.logger1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logger1.Size = new System.Drawing.Size(1006, 102);
            this.logger1.TabIndex = 2;
            this.logger1.TimeStamp = true;
            this.logger1.TimeStampFormat = "HH-mm-ss";
            // 
            // MinusYbutton
            // 
            this.MinusYbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinusYbutton.Location = new System.Drawing.Point(57, 196);
            this.MinusYbutton.Margin = new System.Windows.Forms.Padding(2);
            this.MinusYbutton.Name = "MinusYbutton";
            this.MinusYbutton.Size = new System.Drawing.Size(34, 34);
            this.MinusYbutton.TabIndex = 3;
            this.MinusYbutton.Text = "Y -";
            this.MinusYbutton.UseVisualStyleBackColor = true;
            this.MinusYbutton.Click += new System.EventHandler(this.MinusYbutton_Click);
            // 
            // PlusYbutton
            // 
            this.PlusYbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlusYbutton.Location = new System.Drawing.Point(57, 269);
            this.PlusYbutton.Margin = new System.Windows.Forms.Padding(2);
            this.PlusYbutton.Name = "PlusYbutton";
            this.PlusYbutton.Size = new System.Drawing.Size(34, 34);
            this.PlusYbutton.TabIndex = 4;
            this.PlusYbutton.Text = "Y +";
            this.PlusYbutton.UseVisualStyleBackColor = true;
            this.PlusYbutton.Click += new System.EventHandler(this.PlusYbutton_Click);
            // 
            // PlusXbutton
            // 
            this.PlusXbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlusXbutton.Location = new System.Drawing.Point(101, 232);
            this.PlusXbutton.Margin = new System.Windows.Forms.Padding(2);
            this.PlusXbutton.Name = "PlusXbutton";
            this.PlusXbutton.Size = new System.Drawing.Size(34, 34);
            this.PlusXbutton.TabIndex = 5;
            this.PlusXbutton.Text = "X +";
            this.PlusXbutton.UseVisualStyleBackColor = true;
            this.PlusXbutton.Click += new System.EventHandler(this.PlusXbutton_Click);
            // 
            // MinusXbutton
            // 
            this.MinusXbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinusXbutton.Location = new System.Drawing.Point(14, 232);
            this.MinusXbutton.Margin = new System.Windows.Forms.Padding(2);
            this.MinusXbutton.Name = "MinusXbutton";
            this.MinusXbutton.Size = new System.Drawing.Size(34, 34);
            this.MinusXbutton.TabIndex = 6;
            this.MinusXbutton.Text = "X -";
            this.MinusXbutton.UseVisualStyleBackColor = true;
            this.MinusXbutton.Click += new System.EventHandler(this.MinusXbutton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.XMinStatusLabel,
            this.XMaxStatusLabel,
            this.YMinStatusLabel,
            this.YMaxStatusLabel,
            this.TopStatusLabel,
            this.BottomStatusLabel,
            this.XStatusLabel,
            this.YStatusLabel,
            this.toolStripDropDownButton1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 677);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1008, 25);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.MarqueeAnimationSpeed = 10;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(75, 19);
            this.toolStripProgressBar.Step = 1;
            // 
            // XMinStatusLabel
            // 
            this.XMinStatusLabel.AutoSize = false;
            this.XMinStatusLabel.BackColor = System.Drawing.Color.Lime;
            this.XMinStatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.XMinStatusLabel.Name = "XMinStatusLabel";
            this.XMinStatusLabel.Size = new System.Drawing.Size(80, 20);
            this.XMinStatusLabel.Text = "X - Min";
            // 
            // XMaxStatusLabel
            // 
            this.XMaxStatusLabel.AutoSize = false;
            this.XMaxStatusLabel.BackColor = System.Drawing.Color.Lime;
            this.XMaxStatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.XMaxStatusLabel.Name = "XMaxStatusLabel";
            this.XMaxStatusLabel.Size = new System.Drawing.Size(80, 20);
            this.XMaxStatusLabel.Text = "X - Max";
            // 
            // YMinStatusLabel
            // 
            this.YMinStatusLabel.AutoSize = false;
            this.YMinStatusLabel.BackColor = System.Drawing.Color.Lime;
            this.YMinStatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.YMinStatusLabel.Name = "YMinStatusLabel";
            this.YMinStatusLabel.Size = new System.Drawing.Size(80, 20);
            this.YMinStatusLabel.Text = "Y - Min";
            // 
            // YMaxStatusLabel
            // 
            this.YMaxStatusLabel.AutoSize = false;
            this.YMaxStatusLabel.BackColor = System.Drawing.Color.Lime;
            this.YMaxStatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.YMaxStatusLabel.Name = "YMaxStatusLabel";
            this.YMaxStatusLabel.Size = new System.Drawing.Size(80, 20);
            this.YMaxStatusLabel.Text = "Y - Max";
            // 
            // TopStatusLabel
            // 
            this.TopStatusLabel.AutoSize = false;
            this.TopStatusLabel.BackColor = System.Drawing.Color.Lime;
            this.TopStatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.TopStatusLabel.Name = "TopStatusLabel";
            this.TopStatusLabel.Size = new System.Drawing.Size(80, 20);
            this.TopStatusLabel.Text = "Top";
            // 
            // BottomStatusLabel
            // 
            this.BottomStatusLabel.AutoSize = false;
            this.BottomStatusLabel.BackColor = System.Drawing.Color.Lime;
            this.BottomStatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.BottomStatusLabel.Name = "BottomStatusLabel";
            this.BottomStatusLabel.Size = new System.Drawing.Size(80, 20);
            this.BottomStatusLabel.Text = "Bottom";
            // 
            // XStatusLabel
            // 
            this.XStatusLabel.AutoSize = false;
            this.XStatusLabel.Name = "XStatusLabel";
            this.XStatusLabel.Size = new System.Drawing.Size(100, 20);
            this.XStatusLabel.Text = "00000000";
            // 
            // YStatusLabel
            // 
            this.YStatusLabel.AutoSize = false;
            this.YStatusLabel.Name = "YStatusLabel";
            this.YStatusLabel.Size = new System.Drawing.Size(100, 20);
            this.YStatusLabel.Text = "00000000";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLogToolStripMenuItem,
            this.clearLogToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 23);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // saveLogToolStripMenuItem
            // 
            this.saveLogToolStripMenuItem.Name = "saveLogToolStripMenuItem";
            this.saveLogToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.saveLogToolStripMenuItem.Text = "Save Log";
            this.saveLogToolStripMenuItem.Click += new System.EventHandler(this.saveLogToolStripMenuItem_Click);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.clearLogToolStripMenuItem.Text = "Clear Log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // checkBoxX
            // 
            this.checkBoxX.AutoSize = true;
            this.checkBoxX.Checked = true;
            this.checkBoxX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxX.Location = new System.Drawing.Point(15, 63);
            this.checkBoxX.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxX.Name = "checkBoxX";
            this.checkBoxX.Size = new System.Drawing.Size(111, 17);
            this.checkBoxX.TabIndex = 8;
            this.checkBoxX.Text = "X Axis Step Driver";
            this.checkBoxX.UseVisualStyleBackColor = true;
            this.checkBoxX.CheckedChanged += new System.EventHandler(this.checkBoxB_CheckedChanged);
            // 
            // checkBoxY
            // 
            this.checkBoxY.AutoSize = true;
            this.checkBoxY.Checked = true;
            this.checkBoxY.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxY.Location = new System.Drawing.Point(15, 84);
            this.checkBoxY.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxY.Name = "checkBoxY";
            this.checkBoxY.Size = new System.Drawing.Size(111, 17);
            this.checkBoxY.TabIndex = 9;
            this.checkBoxY.Text = "Y Axis Step Driver";
            this.checkBoxY.UseVisualStyleBackColor = true;
            this.checkBoxY.CheckedChanged += new System.EventHandler(this.checkBoxB_CheckedChanged);
            // 
            // checkBoxD
            // 
            this.checkBoxD.AutoSize = true;
            this.checkBoxD.Location = new System.Drawing.Point(15, 128);
            this.checkBoxD.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxD.Name = "checkBoxD";
            this.checkBoxD.Size = new System.Drawing.Size(72, 17);
            this.checkBoxD.TabIndex = 10;
            this.checkBoxD.Text = "Drill Cycle";
            this.checkBoxD.UseVisualStyleBackColor = true;
            this.checkBoxD.CheckedChanged += new System.EventHandler(this.checkBoxB_CheckedChanged);
            // 
            // forcePullButton
            // 
            this.forcePullButton.Location = new System.Drawing.Point(4, 32);
            this.forcePullButton.Margin = new System.Windows.Forms.Padding(2);
            this.forcePullButton.Name = "forcePullButton";
            this.forcePullButton.Size = new System.Drawing.Size(70, 19);
            this.forcePullButton.TabIndex = 12;
            this.forcePullButton.Text = "Force Pool";
            this.forcePullButton.UseVisualStyleBackColor = true;
            this.forcePullButton.Click += new System.EventHandler(this.Sendbutton_Click);
            // 
            // AxisOffsetComboBox
            // 
            this.AxisOffsetComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AxisOffsetComboBox.FormattingEnabled = true;
            this.AxisOffsetComboBox.Items.AddRange(new object[] {
            "1 step",
            "2 step",
            "3 step",
            "4 step",
            "6 step",
            "8 step",
            "10 step",
            "11 step",
            "12 step",
            "16 step",
            "20 step",
            "24 step",
            "48 step",
            "50 step",
            "52 step",
            "96 step",
            "100 step",
            "200 step",
            "600 step",
            "1200 step",
            "0.001 in",
            "0.002 in",
            "0.005 in",
            "0.010 in",
            "0.020 in",
            "0.100 in",
            "0.250 in",
            "0.500 in",
            "1.000 in"});
            this.AxisOffsetComboBox.Location = new System.Drawing.Point(9, 307);
            this.AxisOffsetComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.AxisOffsetComboBox.MaxDropDownItems = 16;
            this.AxisOffsetComboBox.Name = "AxisOffsetComboBox";
            this.AxisOffsetComboBox.Size = new System.Drawing.Size(127, 21);
            this.AxisOffsetComboBox.TabIndex = 13;
            // 
            // bevel1
            // 
            this.bevel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.bevel1.Location = new System.Drawing.Point(4, 58);
            this.bevel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.bevel1.Name = "bevel1";
            this.bevel1.Size = new System.Drawing.Size(142, 270);
            this.bevel1.TabIndex = 14;
            // 
            // UIupdateTimer
            // 
            this.UIupdateTimer.Enabled = true;
            this.UIupdateTimer.Interval = 125;
            this.UIupdateTimer.Tick += new System.EventHandler(this.UIupdateTimer_Tick);
            // 
            // loadFileButton
            // 
            this.loadFileButton.Location = new System.Drawing.Point(328, 31);
            this.loadFileButton.Margin = new System.Windows.Forms.Padding(2);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(156, 21);
            this.loadFileButton.TabIndex = 15;
            this.loadFileButton.Text = "Load VDX/SVG File";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.LoadFileButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "All Supported Types|*.vdx;*.svg|MS Visio Files|*.vdx|Scalable Vertor Graphic|*.sv" +
    "g";
            // 
            // XScaleTextBox
            // 
            this.XScaleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.XScaleTextBox.Location = new System.Drawing.Point(73, 522);
            this.XScaleTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.XScaleTextBox.Name = "XScaleTextBox";
            this.XScaleTextBox.Size = new System.Drawing.Size(57, 20);
            this.XScaleTextBox.TabIndex = 16;
            this.XScaleTextBox.Text = "960";
            // 
            // YScaleTextBox
            // 
            this.YScaleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.YScaleTextBox.Location = new System.Drawing.Point(73, 545);
            this.YScaleTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.YScaleTextBox.Name = "YScaleTextBox";
            this.YScaleTextBox.Size = new System.Drawing.Size(57, 20);
            this.YScaleTextBox.TabIndex = 17;
            this.YScaleTextBox.Text = "960";
            // 
            // XCurrentPosTextBox
            // 
            this.XCurrentPosTextBox.Location = new System.Drawing.Point(78, 435);
            this.XCurrentPosTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.XCurrentPosTextBox.Name = "XCurrentPosTextBox";
            this.XCurrentPosTextBox.Size = new System.Drawing.Size(55, 20);
            this.XCurrentPosTextBox.TabIndex = 18;
            this.XCurrentPosTextBox.Text = "0.000";
            // 
            // YCurrentPosTextBox
            // 
            this.YCurrentPosTextBox.Location = new System.Drawing.Point(78, 458);
            this.YCurrentPosTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.YCurrentPosTextBox.Name = "YCurrentPosTextBox";
            this.YCurrentPosTextBox.Size = new System.Drawing.Size(55, 20);
            this.YCurrentPosTextBox.TabIndex = 19;
            this.YCurrentPosTextBox.Text = "0.000";
            // 
            // setXButton
            // 
            this.setXButton.Location = new System.Drawing.Point(18, 435);
            this.setXButton.Margin = new System.Windows.Forms.Padding(2);
            this.setXButton.Name = "setXButton";
            this.setXButton.Size = new System.Drawing.Size(56, 19);
            this.setXButton.TabIndex = 22;
            this.setXButton.Text = "Set X";
            this.setXButton.UseVisualStyleBackColor = true;
            this.setXButton.Click += new System.EventHandler(this.setXButton_Click);
            // 
            // SetYButton
            // 
            this.SetYButton.Location = new System.Drawing.Point(18, 458);
            this.SetYButton.Margin = new System.Windows.Forms.Padding(2);
            this.SetYButton.Name = "SetYButton";
            this.SetYButton.Size = new System.Drawing.Size(56, 19);
            this.SetYButton.TabIndex = 23;
            this.SetYButton.Text = "Set Y";
            this.SetYButton.UseVisualStyleBackColor = true;
            this.SetYButton.Click += new System.EventHandler(this.SetYButton_Click);
            // 
            // zeroXbutton
            // 
            this.zeroXbutton.Location = new System.Drawing.Point(18, 390);
            this.zeroXbutton.Margin = new System.Windows.Forms.Padding(2);
            this.zeroXbutton.Name = "zeroXbutton";
            this.zeroXbutton.Size = new System.Drawing.Size(56, 19);
            this.zeroXbutton.TabIndex = 24;
            this.zeroXbutton.Text = "Zero  X";
            this.zeroXbutton.UseVisualStyleBackColor = true;
            this.zeroXbutton.Click += new System.EventHandler(this.zeroXbutton_Click);
            // 
            // zeroYbutton
            // 
            this.zeroYbutton.Location = new System.Drawing.Point(78, 390);
            this.zeroYbutton.Margin = new System.Windows.Forms.Padding(2);
            this.zeroYbutton.Name = "zeroYbutton";
            this.zeroYbutton.Size = new System.Drawing.Size(56, 19);
            this.zeroYbutton.TabIndex = 25;
            this.zeroYbutton.Text = "Zero Y";
            this.zeroYbutton.UseVisualStyleBackColor = true;
            this.zeroYbutton.Click += new System.EventHandler(this.zeroYbutton_Click);
            // 
            // zeroAllbutton
            // 
            this.zeroAllbutton.Location = new System.Drawing.Point(18, 412);
            this.zeroAllbutton.Margin = new System.Windows.Forms.Padding(2);
            this.zeroAllbutton.Name = "zeroAllbutton";
            this.zeroAllbutton.Size = new System.Drawing.Size(116, 19);
            this.zeroAllbutton.TabIndex = 26;
            this.zeroAllbutton.Text = "Zero All";
            this.zeroAllbutton.UseVisualStyleBackColor = true;
            this.zeroAllbutton.Click += new System.EventHandler(this.zeroAllbutton_Click);
            // 
            // Xlabel
            // 
            this.Xlabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Xlabel.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Xlabel.Location = new System.Drawing.Point(10, 341);
            this.Xlabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Xlabel.Name = "Xlabel";
            this.Xlabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.Xlabel.Size = new System.Drawing.Size(133, 21);
            this.Xlabel.TabIndex = 35;
            this.Xlabel.Text = "X: -0.0000";
            // 
            // Ylabel
            // 
            this.Ylabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Ylabel.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ylabel.Location = new System.Drawing.Point(10, 362);
            this.Ylabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Ylabel.Name = "Ylabel";
            this.Ylabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.Ylabel.Size = new System.Drawing.Size(133, 21);
            this.Ylabel.TabIndex = 36;
            this.Ylabel.Text = "Y: 0.0000";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.ContextMenuStrip = this.NodesContextMenu;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(328, 83);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(156, 485);
            this.listBox1.TabIndex = 37;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // NodesContextMenu
            // 
            this.NodesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NodeContextSETXY,
            this.NodeContextMOVETO,
            this.toolStripSeparator1,
            this.NodeContextIDLE,
            this.NodeContextDRILED,
            this.NodeContextTARGET});
            this.NodesContextMenu.Name = "NodesContextMenu";
            this.NodesContextMenu.Size = new System.Drawing.Size(172, 120);
            // 
            // NodeContextSETXY
            // 
            this.NodeContextSETXY.Name = "NodeContextSETXY";
            this.NodeContextSETXY.Size = new System.Drawing.Size(171, 22);
            this.NodeContextSETXY.Text = "Set As Current X-Y";
            this.NodeContextSETXY.Click += new System.EventHandler(this.SetAsXYbutton_Click);
            // 
            // NodeContextMOVETO
            // 
            this.NodeContextMOVETO.Name = "NodeContextMOVETO";
            this.NodeContextMOVETO.Size = new System.Drawing.Size(171, 22);
            this.NodeContextMOVETO.Text = "Move To";
            this.NodeContextMOVETO.Click += new System.EventHandler(this.MoveTobutton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(168, 6);
            // 
            // NodeContextIDLE
            // 
            this.NodeContextIDLE.Name = "NodeContextIDLE";
            this.NodeContextIDLE.Size = new System.Drawing.Size(171, 22);
            this.NodeContextIDLE.Text = "Status.Idle";
            this.NodeContextIDLE.Click += new System.EventHandler(this.NodeContextIDLE_Click);
            // 
            // NodeContextDRILED
            // 
            this.NodeContextDRILED.Name = "NodeContextDRILED";
            this.NodeContextDRILED.Size = new System.Drawing.Size(171, 22);
            this.NodeContextDRILED.Text = "Status.Drilled";
            this.NodeContextDRILED.Click += new System.EventHandler(this.NodeContextDRILED_Click);
            // 
            // NodeContextTARGET
            // 
            this.NodeContextTARGET.Name = "NodeContextTARGET";
            this.NodeContextTARGET.Size = new System.Drawing.Size(171, 22);
            this.NodeContextTARGET.Text = "Status.Target";
            this.NodeContextTARGET.Click += new System.EventHandler(this.NodeContextTARGET_Click);
            // 
            // OutputLabel
            // 
            this.OutputLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputLabel.BackColor = System.Drawing.Color.White;
            this.OutputLabel.Cursor = System.Windows.Forms.Cursors.Cross;
            this.OutputLabel.Location = new System.Drawing.Point(497, 63);
            this.OutputLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(500, 500);
            this.OutputLabel.TabIndex = 40;
            this.OutputLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OutputLabel_MouseDown);
            this.OutputLabel.MouseEnter += new System.EventHandler(this.OutputLabel_MouseEnter);
            this.OutputLabel.MouseLeave += new System.EventHandler(this.OutputLabel_MouseLeave);
            // 
            // XSetTranformButton
            // 
            this.XSetTranformButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.XSetTranformButton.Location = new System.Drawing.Point(12, 523);
            this.XSetTranformButton.Margin = new System.Windows.Forms.Padding(2);
            this.XSetTranformButton.Name = "XSetTranformButton";
            this.XSetTranformButton.Size = new System.Drawing.Size(56, 19);
            this.XSetTranformButton.TabIndex = 41;
            this.XSetTranformButton.Text = "Set X";
            this.XSetTranformButton.UseVisualStyleBackColor = true;
            this.XSetTranformButton.Click += new System.EventHandler(this.XSetTransformButton_Click);
            // 
            // YSetTranformButton
            // 
            this.YSetTranformButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.YSetTranformButton.Location = new System.Drawing.Point(12, 546);
            this.YSetTranformButton.Margin = new System.Windows.Forms.Padding(2);
            this.YSetTranformButton.Name = "YSetTranformButton";
            this.YSetTranformButton.Size = new System.Drawing.Size(56, 19);
            this.YSetTranformButton.TabIndex = 42;
            this.YSetTranformButton.Text = "Set Y";
            this.YSetTranformButton.UseVisualStyleBackColor = true;
            this.YSetTranformButton.Click += new System.EventHandler(this.YSetTransformButton_Click);
            // 
            // ViewZoomLabel
            // 
            this.ViewZoomLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ViewZoomLabel.Location = new System.Drawing.Point(953, 32);
            this.ViewZoomLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ViewZoomLabel.Name = "ViewZoomLabel";
            this.ViewZoomLabel.Size = new System.Drawing.Size(40, 19);
            this.ViewZoomLabel.TabIndex = 43;
            this.ViewZoomLabel.Text = "Zoom: ";
            // 
            // ViewXLabel
            // 
            this.ViewXLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ViewXLabel.Location = new System.Drawing.Point(496, 32);
            this.ViewXLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ViewXLabel.Name = "ViewXLabel";
            this.ViewXLabel.Size = new System.Drawing.Size(40, 19);
            this.ViewXLabel.TabIndex = 44;
            this.ViewXLabel.Text = "X: ";
            // 
            // ViewYLabel
            // 
            this.ViewYLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ViewYLabel.Location = new System.Drawing.Point(540, 32);
            this.ViewYLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ViewYLabel.Name = "ViewYLabel";
            this.ViewYLabel.Size = new System.Drawing.Size(40, 19);
            this.ViewYLabel.TabIndex = 45;
            this.ViewYLabel.Text = "Y: ";
            // 
            // ReloadUSBbutton
            // 
            this.ReloadUSBbutton.Location = new System.Drawing.Point(76, 32);
            this.ReloadUSBbutton.Margin = new System.Windows.Forms.Padding(2);
            this.ReloadUSBbutton.Name = "ReloadUSBbutton";
            this.ReloadUSBbutton.Size = new System.Drawing.Size(70, 19);
            this.ReloadUSBbutton.TabIndex = 48;
            this.ReloadUSBbutton.Text = "Reload USB";
            this.ReloadUSBbutton.UseVisualStyleBackColor = true;
            this.ReloadUSBbutton.Click += new System.EventHandler(this.ReloadUSBbutton_Click);
            // 
            // OffsetOriginBtton
            // 
            this.OffsetOriginBtton.Location = new System.Drawing.Point(720, 31);
            this.OffsetOriginBtton.Margin = new System.Windows.Forms.Padding(2);
            this.OffsetOriginBtton.Name = "OffsetOriginBtton";
            this.OffsetOriginBtton.Size = new System.Drawing.Size(139, 21);
            this.OffsetOriginBtton.TabIndex = 52;
            this.OffsetOriginBtton.Text = "Offset Drawing Origin by:";
            this.OffsetOriginBtton.UseVisualStyleBackColor = true;
            this.OffsetOriginBtton.Click += new System.EventHandler(this.OffsetOriginBtton_Click);
            // 
            // YoriginTextbox
            // 
            this.YoriginTextbox.Location = new System.Drawing.Point(907, 31);
            this.YoriginTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.YoriginTextbox.Name = "YoriginTextbox";
            this.YoriginTextbox.Size = new System.Drawing.Size(40, 20);
            this.YoriginTextbox.TabIndex = 51;
            this.YoriginTextbox.Text = "0.000";
            // 
            // XoriginTextbox
            // 
            this.XoriginTextbox.Location = new System.Drawing.Point(863, 31);
            this.XoriginTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.XoriginTextbox.Name = "XoriginTextbox";
            this.XoriginTextbox.Size = new System.Drawing.Size(40, 20);
            this.XoriginTextbox.TabIndex = 50;
            this.XoriginTextbox.Text = "0.000";
            // 
            // OptimizeButton
            // 
            this.OptimizeButton.Location = new System.Drawing.Point(328, 57);
            this.OptimizeButton.Margin = new System.Windows.Forms.Padding(2);
            this.OptimizeButton.Name = "OptimizeButton";
            this.OptimizeButton.Size = new System.Drawing.Size(156, 21);
            this.OptimizeButton.TabIndex = 53;
            this.OptimizeButton.Text = "Optimize from current position";
            this.OptimizeButton.UseVisualStyleBackColor = true;
            this.OptimizeButton.Click += new System.EventHandler(this.OptimizeButton_Click);
            // 
            // ViewContextMenu
            // 
            this.ViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewSetXYContext,
            this.ViewSetDRGOrigin,
            this.moveToToolStripMenuItem});
            this.ViewContextMenu.Name = "ViewContextMenu";
            this.ViewContextMenu.Size = new System.Drawing.Size(190, 70);
            // 
            // ViewSetXYContext
            // 
            this.ViewSetXYContext.Name = "ViewSetXYContext";
            this.ViewSetXYContext.Size = new System.Drawing.Size(189, 22);
            this.ViewSetXYContext.Text = "Set As Current X-Y";
            this.ViewSetXYContext.Click += new System.EventHandler(this.ViewSetXYContext_Click);
            // 
            // ViewSetDRGOrigin
            // 
            this.ViewSetDRGOrigin.Name = "ViewSetDRGOrigin";
            this.ViewSetDRGOrigin.Size = new System.Drawing.Size(189, 22);
            this.ViewSetDRGOrigin.Text = "Set As Drawing Origin";
            this.ViewSetDRGOrigin.Click += new System.EventHandler(this.ViewSetDRGOrigin_Click);
            // 
            // moveToToolStripMenuItem
            // 
            this.moveToToolStripMenuItem.Name = "moveToToolStripMenuItem";
            this.moveToToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.moveToToolStripMenuItem.Text = "Move To";
            this.moveToToolStripMenuItem.Click += new System.EventHandler(this.moveToToolStripMenuItem_Click);
            // 
            // SnapSizeTextBox
            // 
            this.SnapSizeTextBox.Location = new System.Drawing.Point(660, 31);
            this.SnapSizeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.SnapSizeTextBox.Name = "SnapSizeTextBox";
            this.SnapSizeTextBox.Size = new System.Drawing.Size(40, 20);
            this.SnapSizeTextBox.TabIndex = 55;
            this.SnapSizeTextBox.Text = "0.050";
            // 
            // SnapViewBox
            // 
            this.SnapViewBox.AutoSize = true;
            this.SnapViewBox.Checked = true;
            this.SnapViewBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SnapViewBox.Location = new System.Drawing.Point(585, 33);
            this.SnapViewBox.Name = "SnapViewBox";
            this.SnapViewBox.Size = new System.Drawing.Size(70, 17);
            this.SnapViewBox.TabIndex = 56;
            this.SnapViewBox.Text = "Snap To:";
            this.SnapViewBox.UseVisualStyleBackColor = true;
            // 
            // SetAllButton
            // 
            this.SetAllButton.Location = new System.Drawing.Point(18, 482);
            this.SetAllButton.Margin = new System.Windows.Forms.Padding(2);
            this.SetAllButton.Name = "SetAllButton";
            this.SetAllButton.Size = new System.Drawing.Size(116, 19);
            this.SetAllButton.TabIndex = 57;
            this.SetAllButton.Text = "Set All";
            this.SetAllButton.UseVisualStyleBackColor = true;
            this.SetAllButton.Click += new System.EventHandler(this.SetAllButton_Click);
            // 
            // YBacklastTextbox
            // 
            this.YBacklastTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.YBacklastTextbox.Location = new System.Drawing.Point(134, 545);
            this.YBacklastTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.YBacklastTextbox.Name = "YBacklastTextbox";
            this.YBacklastTextbox.Size = new System.Drawing.Size(57, 20);
            this.YBacklastTextbox.TabIndex = 59;
            this.YBacklastTextbox.Text = "4";
            // 
            // XBacklastTextbox
            // 
            this.XBacklastTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.XBacklastTextbox.Location = new System.Drawing.Point(134, 522);
            this.XBacklastTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.XBacklastTextbox.Name = "XBacklastTextbox";
            this.XBacklastTextbox.Size = new System.Drawing.Size(57, 20);
            this.XBacklastTextbox.TabIndex = 58;
            this.XBacklastTextbox.Text = "4";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 507);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 60;
            this.label2.Text = "Scale:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(137, 507);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 61;
            this.label3.Text = "Backlash:";
            // 
            // IgnoreBacklashBox
            // 
            this.IgnoreBacklashBox.AutoSize = true;
            this.IgnoreBacklashBox.Checked = true;
            this.IgnoreBacklashBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IgnoreBacklashBox.Location = new System.Drawing.Point(15, 150);
            this.IgnoreBacklashBox.Name = "IgnoreBacklashBox";
            this.IgnoreBacklashBox.Size = new System.Drawing.Size(103, 17);
            this.IgnoreBacklashBox.TabIndex = 62;
            this.IgnoreBacklashBox.Text = "Ignore Backlash";
            this.IgnoreBacklashBox.UseVisualStyleBackColor = true;
            this.IgnoreBacklashBox.CheckedChanged += new System.EventHandler(this.IgnoreBacklashBox_CheckedChanged);
            // 
            // checkBoxT
            // 
            this.checkBoxT.AutoSize = true;
            this.checkBoxT.Location = new System.Drawing.Point(15, 106);
            this.checkBoxT.Name = "checkBoxT";
            this.checkBoxT.Size = new System.Drawing.Size(121, 17);
            this.checkBoxT.TabIndex = 63;
            this.checkBoxT.Text = "Torque Assist Driver";
            this.checkBoxT.UseVisualStyleBackColor = true;
            this.checkBoxT.CheckedChanged += new System.EventHandler(this.checkBoxB_CheckedChanged);
            // 
            // showRawCheckbox
            // 
            this.showRawCheckbox.AutoSize = true;
            this.showRawCheckbox.Location = new System.Drawing.Point(15, 173);
            this.showRawCheckbox.Name = "showRawCheckbox";
            this.showRawCheckbox.Size = new System.Drawing.Size(104, 17);
            this.showRawCheckbox.TabIndex = 64;
            this.showRawCheckbox.Text = "Show Raw Data";
            this.showRawCheckbox.UseVisualStyleBackColor = true;
            this.showRawCheckbox.CheckedChanged += new System.EventHandler(this.showRawCheckbox_CheckedChanged);
            // 
            // AsyncStartFindOriginButton
            // 
            this.AsyncStartFindOriginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AsyncStartFindOriginButton.Location = new System.Drawing.Point(151, 31);
            this.AsyncStartFindOriginButton.Name = "AsyncStartFindOriginButton";
            this.AsyncStartFindOriginButton.Size = new System.Drawing.Size(172, 21);
            this.AsyncStartFindOriginButton.TabIndex = 66;
            this.AsyncStartFindOriginButton.Text = "[Find Axis Origin]";
            this.AsyncStartFindOriginButton.UseVisualStyleBackColor = true;
            this.AsyncStartFindOriginButton.Click += new System.EventHandler(this.AsyncStartFindOriginButton_Click);
            // 
            // AsyncDrillSelectedButton
            // 
            this.AsyncDrillSelectedButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AsyncDrillSelectedButton.Location = new System.Drawing.Point(151, 59);
            this.AsyncDrillSelectedButton.Name = "AsyncDrillSelectedButton";
            this.AsyncDrillSelectedButton.Size = new System.Drawing.Size(172, 21);
            this.AsyncDrillSelectedButton.TabIndex = 67;
            this.AsyncDrillSelectedButton.Text = "[Drill Selected Node]";
            this.AsyncDrillSelectedButton.UseVisualStyleBackColor = true;
            this.AsyncDrillSelectedButton.Click += new System.EventHandler(this.AsyncDrillSelectedButton_Click);
            // 
            // DrillAllNodebutton
            // 
            this.DrillAllNodebutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DrillAllNodebutton.Location = new System.Drawing.Point(151, 86);
            this.DrillAllNodebutton.Name = "DrillAllNodebutton";
            this.DrillAllNodebutton.Size = new System.Drawing.Size(172, 21);
            this.DrillAllNodebutton.TabIndex = 68;
            this.DrillAllNodebutton.Text = "[Drill All Nodes]";
            this.DrillAllNodebutton.UseVisualStyleBackColor = true;
            this.DrillAllNodebutton.Click += new System.EventHandler(this.DrillAllNodebutton_Click);
            // 
            // AbortMoveButton
            // 
            this.AbortMoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AbortMoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AbortMoveButton.ForeColor = System.Drawing.Color.Red;
            this.AbortMoveButton.Location = new System.Drawing.Point(196, 543);
            this.AbortMoveButton.Name = "AbortMoveButton";
            this.AbortMoveButton.Size = new System.Drawing.Size(127, 23);
            this.AbortMoveButton.TabIndex = 69;
            this.AbortMoveButton.Text = "Abort Move";
            this.AbortMoveButton.UseVisualStyleBackColor = true;
            this.AbortMoveButton.Click += new System.EventHandler(this.AbortMoveButton_Click);
            // 
            // ArrowCaptureTextbox
            // 
            this.ArrowCaptureTextbox.Location = new System.Drawing.Point(59, 237);
            this.ArrowCaptureTextbox.Name = "ArrowCaptureTextbox";
            this.ArrowCaptureTextbox.Size = new System.Drawing.Size(27, 20);
            this.ArrowCaptureTextbox.TabIndex = 70;
            this.ArrowCaptureTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ArrowCaptureTextbox_KeyDown);
            this.ArrowCaptureTextbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ArrowCaptureTextbox_KeyUp);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1008, 702);
            this.Controls.Add(this.ArrowCaptureTextbox);
            this.Controls.Add(this.AbortMoveButton);
            this.Controls.Add(this.DrillAllNodebutton);
            this.Controls.Add(this.AsyncDrillSelectedButton);
            this.Controls.Add(this.AsyncStartFindOriginButton);
            this.Controls.Add(this.showRawCheckbox);
            this.Controls.Add(this.checkBoxT);
            this.Controls.Add(this.IgnoreBacklashBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.YBacklastTextbox);
            this.Controls.Add(this.XBacklastTextbox);
            this.Controls.Add(this.SetAllButton);
            this.Controls.Add(this.SnapViewBox);
            this.Controls.Add(this.SnapSizeTextBox);
            this.Controls.Add(this.OptimizeButton);
            this.Controls.Add(this.OffsetOriginBtton);
            this.Controls.Add(this.YoriginTextbox);
            this.Controls.Add(this.XoriginTextbox);
            this.Controls.Add(this.ReloadUSBbutton);
            this.Controls.Add(this.ViewYLabel);
            this.Controls.Add(this.ViewXLabel);
            this.Controls.Add(this.ViewZoomLabel);
            this.Controls.Add(this.YSetTranformButton);
            this.Controls.Add(this.XSetTranformButton);
            this.Controls.Add(this.OutputLabel);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.Ylabel);
            this.Controls.Add(this.Xlabel);
            this.Controls.Add(this.zeroAllbutton);
            this.Controls.Add(this.zeroYbutton);
            this.Controls.Add(this.zeroXbutton);
            this.Controls.Add(this.SetYButton);
            this.Controls.Add(this.setXButton);
            this.Controls.Add(this.YCurrentPosTextBox);
            this.Controls.Add(this.XCurrentPosTextBox);
            this.Controls.Add(this.YScaleTextBox);
            this.Controls.Add(this.XScaleTextBox);
            this.Controls.Add(this.loadFileButton);
            this.Controls.Add(this.AxisOffsetComboBox);
            this.Controls.Add(this.forcePullButton);
            this.Controls.Add(this.checkBoxD);
            this.Controls.Add(this.checkBoxY);
            this.Controls.Add(this.checkBoxX);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MinusXbutton);
            this.Controls.Add(this.PlusXbutton);
            this.Controls.Add(this.PlusYbutton);
            this.Controls.Add(this.MinusYbutton);
            this.Controls.Add(this.logger1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.USBdevicesComboBox);
            this.Controls.Add(this.bevel1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CNC Drill Machine Controller (Software Interface)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.NodesContextMenu.ResumeLayout(false);
            this.ViewContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox USBdevicesComboBox;
        private System.Windows.Forms.Label label1;
        private M4nuskomponents.Logger logger1;
        private System.Windows.Forms.Button MinusYbutton;
        private System.Windows.Forms.Button PlusYbutton;
        private System.Windows.Forms.Button PlusXbutton;
        private System.Windows.Forms.Button MinusXbutton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel XMinStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel XMaxStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel YMinStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel YMaxStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel TopStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel BottomStatusLabel;
        private System.Windows.Forms.CheckBox checkBoxX;
        private System.Windows.Forms.CheckBox checkBoxY;
        private System.Windows.Forms.CheckBox checkBoxD;
        private System.Windows.Forms.ToolStripStatusLabel XStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel YStatusLabel;
        private System.Windows.Forms.Button forcePullButton;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem saveLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.ComboBox AxisOffsetComboBox;
        private System.Windows.Forms.Label bevel1;
        private System.Windows.Forms.Timer UIupdateTimer;
        private System.Windows.Forms.Button loadFileButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox XScaleTextBox;
        private System.Windows.Forms.TextBox YScaleTextBox;
        private System.Windows.Forms.TextBox XCurrentPosTextBox;
        private System.Windows.Forms.TextBox YCurrentPosTextBox;
        private System.Windows.Forms.Button setXButton;
        private System.Windows.Forms.Button SetYButton;
        private System.Windows.Forms.Button zeroXbutton;
        private System.Windows.Forms.Button zeroYbutton;
        private System.Windows.Forms.Button zeroAllbutton;
        private System.Windows.Forms.Label Xlabel;
        private System.Windows.Forms.Label Ylabel;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label OutputLabel;
        private System.Windows.Forms.Button XSetTranformButton;
        private System.Windows.Forms.Button YSetTranformButton;
        private System.Windows.Forms.Label ViewZoomLabel;
        private System.Windows.Forms.Label ViewXLabel;
        private System.Windows.Forms.Label ViewYLabel;
        private System.Windows.Forms.Button ReloadUSBbutton;
        private System.Windows.Forms.ContextMenuStrip NodesContextMenu;
        private System.Windows.Forms.ToolStripMenuItem NodeContextSETXY;
        private System.Windows.Forms.ToolStripMenuItem NodeContextMOVETO;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem NodeContextIDLE;
        private System.Windows.Forms.ToolStripMenuItem NodeContextDRILED;
        private System.Windows.Forms.ToolStripMenuItem NodeContextTARGET;
        private System.Windows.Forms.Button OffsetOriginBtton;
        private System.Windows.Forms.TextBox YoriginTextbox;
        private System.Windows.Forms.TextBox XoriginTextbox;
        private System.Windows.Forms.Button OptimizeButton;
        private System.Windows.Forms.ContextMenuStrip ViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ViewSetXYContext;
        private System.Windows.Forms.ToolStripMenuItem ViewSetDRGOrigin;
        private System.Windows.Forms.TextBox SnapSizeTextBox;
        private System.Windows.Forms.CheckBox SnapViewBox;
        private System.Windows.Forms.ToolStripMenuItem moveToToolStripMenuItem;
        private System.Windows.Forms.Button SetAllButton;
        private System.Windows.Forms.TextBox YBacklastTextbox;
        private System.Windows.Forms.TextBox XBacklastTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox IgnoreBacklashBox;
        private System.Windows.Forms.CheckBox checkBoxT;
        private System.Windows.Forms.CheckBox showRawCheckbox;
        private System.Windows.Forms.Button AsyncStartFindOriginButton;
        private System.Windows.Forms.Button AsyncDrillSelectedButton;
        private System.Windows.Forms.Button DrillAllNodebutton;
        private System.Windows.Forms.Button AbortMoveButton;
        private System.Windows.Forms.TextBox ArrowCaptureTextbox;
    }
}

