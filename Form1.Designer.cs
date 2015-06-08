namespace CNC_Drill_Controller1
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.USBdevicesComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.logger1 = new M4nuskomponents.Logger();
            this.MinusYbutton = new System.Windows.Forms.Button();
            this.PlusYbutton = new System.Windows.Forms.Button();
            this.PlusXbutton = new System.Windows.Forms.Button();
            this.MinusXbutton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
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
            this.checkBoxT = new System.Windows.Forms.CheckBox();
            this.checkBoxB = new System.Windows.Forms.CheckBox();
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
            this.MoveTobutton = new System.Windows.Forms.Button();
            this.SetAsXYbutton = new System.Windows.Forms.Button();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.XScalebutton = new System.Windows.Forms.Button();
            this.YScalebutton = new System.Windows.Forms.Button();
            this.ViewZoomLabel = new System.Windows.Forms.Label();
            this.ViewXLabel = new System.Windows.Forms.Label();
            this.ViewYLabel = new System.Windows.Forms.Label();
            this.RunButton = new System.Windows.Forms.Button();
            this.SeekZeroButton = new System.Windows.Forms.Button();
            this.ReloadUSBbutton = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // USBdevicesComboBox
            // 
            this.USBdevicesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.USBdevicesComboBox.FormattingEnabled = true;
            this.USBdevicesComboBox.Location = new System.Drawing.Point(113, 6);
            this.USBdevicesComboBox.Name = "USBdevicesComboBox";
            this.USBdevicesComboBox.Size = new System.Drawing.Size(890, 24);
            this.USBdevicesComboBox.TabIndex = 0;
            this.USBdevicesComboBox.Text = "[None]";
            this.USBdevicesComboBox.SelectedIndexChanged += new System.EventHandler(this.USBdevicesComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 17);
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
            this.logger1.Location = new System.Drawing.Point(5, 564);
            this.logger1.Multiline = true;
            this.logger1.Name = "logger1";
            this.logger1.ReadOnly = true;
            this.logger1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logger1.Size = new System.Drawing.Size(1001, 134);
            this.logger1.TabIndex = 2;
            this.logger1.TimeStamp = true;
            this.logger1.TimeStampFormat = "HH-mm-ss";
            // 
            // MinusYbutton
            // 
            this.MinusYbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinusYbutton.Location = new System.Drawing.Point(79, 185);
            this.MinusYbutton.Name = "MinusYbutton";
            this.MinusYbutton.Size = new System.Drawing.Size(40, 40);
            this.MinusYbutton.TabIndex = 3;
            this.MinusYbutton.Text = "Y -";
            this.MinusYbutton.UseVisualStyleBackColor = true;
            this.MinusYbutton.Click += new System.EventHandler(this.MinusYbutton_Click);
            // 
            // PlusYbutton
            // 
            this.PlusYbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlusYbutton.Location = new System.Drawing.Point(79, 275);
            this.PlusYbutton.Name = "PlusYbutton";
            this.PlusYbutton.Size = new System.Drawing.Size(40, 40);
            this.PlusYbutton.TabIndex = 4;
            this.PlusYbutton.Text = "Y +";
            this.PlusYbutton.UseVisualStyleBackColor = true;
            this.PlusYbutton.Click += new System.EventHandler(this.PlusYbutton_Click);
            // 
            // PlusXbutton
            // 
            this.PlusXbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlusXbutton.Location = new System.Drawing.Point(137, 229);
            this.PlusXbutton.Name = "PlusXbutton";
            this.PlusXbutton.Size = new System.Drawing.Size(40, 40);
            this.PlusXbutton.TabIndex = 5;
            this.PlusXbutton.Text = "X +";
            this.PlusXbutton.UseVisualStyleBackColor = true;
            this.PlusXbutton.Click += new System.EventHandler(this.PlusXbutton_Click);
            // 
            // MinusXbutton
            // 
            this.MinusXbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinusXbutton.Location = new System.Drawing.Point(21, 229);
            this.MinusXbutton.Name = "MinusXbutton";
            this.MinusXbutton.Size = new System.Drawing.Size(40, 40);
            this.MinusXbutton.TabIndex = 6;
            this.MinusXbutton.Text = "X -";
            this.MinusXbutton.UseVisualStyleBackColor = true;
            this.MinusXbutton.Click += new System.EventHandler(this.MinusXbutton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.XMinStatusLabel,
            this.XMaxStatusLabel,
            this.YMinStatusLabel,
            this.YMaxStatusLabel,
            this.TopStatusLabel,
            this.BottomStatusLabel,
            this.XStatusLabel,
            this.YStatusLabel,
            this.toolStripDropDownButton1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 698);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1006, 25);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Maximum = 4096;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 19);
            // 
            // XMinStatusLabel
            // 
            this.XMinStatusLabel.AutoSize = false;
            this.XMinStatusLabel.BackColor = System.Drawing.Color.Lime;
            this.XMinStatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.XMinStatusLabel.Name = "XMinStatusLabel";
            this.XMinStatusLabel.Size = new System.Drawing.Size(100, 20);
            this.XMinStatusLabel.Text = "X - Min";
            // 
            // XMaxStatusLabel
            // 
            this.XMaxStatusLabel.AutoSize = false;
            this.XMaxStatusLabel.BackColor = System.Drawing.Color.Lime;
            this.XMaxStatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.XMaxStatusLabel.Name = "XMaxStatusLabel";
            this.XMaxStatusLabel.Size = new System.Drawing.Size(100, 20);
            this.XMaxStatusLabel.Text = "X - Max";
            // 
            // YMinStatusLabel
            // 
            this.YMinStatusLabel.AutoSize = false;
            this.YMinStatusLabel.BackColor = System.Drawing.Color.Lime;
            this.YMinStatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.YMinStatusLabel.Name = "YMinStatusLabel";
            this.YMinStatusLabel.Size = new System.Drawing.Size(100, 20);
            this.YMinStatusLabel.Text = "Y - Min";
            // 
            // YMaxStatusLabel
            // 
            this.YMaxStatusLabel.AutoSize = false;
            this.YMaxStatusLabel.BackColor = System.Drawing.Color.Lime;
            this.YMaxStatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.YMaxStatusLabel.Name = "YMaxStatusLabel";
            this.YMaxStatusLabel.Size = new System.Drawing.Size(100, 20);
            this.YMaxStatusLabel.Text = "Y - Max";
            // 
            // TopStatusLabel
            // 
            this.TopStatusLabel.AutoSize = false;
            this.TopStatusLabel.BackColor = System.Drawing.Color.Lime;
            this.TopStatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.TopStatusLabel.Name = "TopStatusLabel";
            this.TopStatusLabel.Size = new System.Drawing.Size(100, 20);
            this.TopStatusLabel.Text = "Top";
            // 
            // BottomStatusLabel
            // 
            this.BottomStatusLabel.AutoSize = false;
            this.BottomStatusLabel.BackColor = System.Drawing.Color.Lime;
            this.BottomStatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.BottomStatusLabel.Name = "BottomStatusLabel";
            this.BottomStatusLabel.Size = new System.Drawing.Size(100, 20);
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
            this.saveLogToolStripMenuItem.Size = new System.Drawing.Size(141, 24);
            this.saveLogToolStripMenuItem.Text = "Save Log";
            this.saveLogToolStripMenuItem.Click += new System.EventHandler(this.saveLogToolStripMenuItem_Click);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(141, 24);
            this.clearLogToolStripMenuItem.Text = "Clear Log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // checkBoxX
            // 
            this.checkBoxX.AutoSize = true;
            this.checkBoxX.Location = new System.Drawing.Point(20, 77);
            this.checkBoxX.Name = "checkBoxX";
            this.checkBoxX.Size = new System.Drawing.Size(143, 21);
            this.checkBoxX.TabIndex = 8;
            this.checkBoxX.Text = "X Axis Step Driver";
            this.checkBoxX.UseVisualStyleBackColor = true;
            this.checkBoxX.CheckedChanged += new System.EventHandler(this.checkBoxB_CheckedChanged);
            // 
            // checkBoxY
            // 
            this.checkBoxY.AutoSize = true;
            this.checkBoxY.Location = new System.Drawing.Point(20, 104);
            this.checkBoxY.Name = "checkBoxY";
            this.checkBoxY.Size = new System.Drawing.Size(143, 21);
            this.checkBoxY.TabIndex = 9;
            this.checkBoxY.Text = "Y Axis Step Driver";
            this.checkBoxY.UseVisualStyleBackColor = true;
            this.checkBoxY.CheckedChanged += new System.EventHandler(this.checkBoxB_CheckedChanged);
            // 
            // checkBoxT
            // 
            this.checkBoxT.AutoSize = true;
            this.checkBoxT.Location = new System.Drawing.Point(20, 131);
            this.checkBoxT.Name = "checkBoxT";
            this.checkBoxT.Size = new System.Drawing.Size(157, 21);
            this.checkBoxT.TabIndex = 10;
            this.checkBoxT.Text = "Drill Cycle From Top";
            this.checkBoxT.UseVisualStyleBackColor = true;
            this.checkBoxT.CheckedChanged += new System.EventHandler(this.checkBoxB_CheckedChanged);
            // 
            // checkBoxB
            // 
            this.checkBoxB.AutoSize = true;
            this.checkBoxB.Location = new System.Drawing.Point(20, 158);
            this.checkBoxB.Name = "checkBoxB";
            this.checkBoxB.Size = new System.Drawing.Size(172, 21);
            this.checkBoxB.TabIndex = 11;
            this.checkBoxB.Text = "Drill Cycle from Bottom";
            this.checkBoxB.UseVisualStyleBackColor = true;
            this.checkBoxB.CheckedChanged += new System.EventHandler(this.checkBoxB_CheckedChanged);
            // 
            // forcePullButton
            // 
            this.forcePullButton.Location = new System.Drawing.Point(5, 39);
            this.forcePullButton.Name = "forcePullButton";
            this.forcePullButton.Size = new System.Drawing.Size(94, 23);
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
            "1",
            "3",
            "6",
            "12",
            "24 (0.025 / 0.5T)",
            "48 (0.050 / 1T) ",
            "96 (0.100 / 2T)",
            "120 (0.125 2.5T)",
            "240 (0.250 / 5T)",
            "480 (0.500 / 10T)",
            "960 (1.000 / 20T)"});
            this.AxisOffsetComboBox.Location = new System.Drawing.Point(15, 321);
            this.AxisOffsetComboBox.MaxDropDownItems = 16;
            this.AxisOffsetComboBox.Name = "AxisOffsetComboBox";
            this.AxisOffsetComboBox.Size = new System.Drawing.Size(177, 24);
            this.AxisOffsetComboBox.TabIndex = 13;
            this.AxisOffsetComboBox.SelectedIndexChanged += new System.EventHandler(this.AxisOffsetComboBox_SelectedIndexChanged);
            // 
            // bevel1
            // 
            this.bevel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.bevel1.Location = new System.Drawing.Point(5, 71);
            this.bevel1.Name = "bevel1";
            this.bevel1.Size = new System.Drawing.Size(196, 358);
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
            this.loadFileButton.Location = new System.Drawing.Point(556, 489);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(75, 23);
            this.loadFileButton.TabIndex = 15;
            this.loadFileButton.Text = "Load File";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.LoadFileButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "*.vdx";
            // 
            // XScaleTextBox
            // 
            this.XScaleTextBox.Location = new System.Drawing.Point(291, 40);
            this.XScaleTextBox.Name = "XScaleTextBox";
            this.XScaleTextBox.ReadOnly = true;
            this.XScaleTextBox.Size = new System.Drawing.Size(75, 22);
            this.XScaleTextBox.TabIndex = 16;
            this.XScaleTextBox.Text = "960";
            // 
            // YScaleTextBox
            // 
            this.YScaleTextBox.Location = new System.Drawing.Point(453, 40);
            this.YScaleTextBox.Name = "YScaleTextBox";
            this.YScaleTextBox.ReadOnly = true;
            this.YScaleTextBox.Size = new System.Drawing.Size(75, 22);
            this.YScaleTextBox.TabIndex = 17;
            this.YScaleTextBox.Text = "960";
            // 
            // XCurrentPosTextBox
            // 
            this.XCurrentPosTextBox.Location = new System.Drawing.Point(291, 98);
            this.XCurrentPosTextBox.Name = "XCurrentPosTextBox";
            this.XCurrentPosTextBox.Size = new System.Drawing.Size(75, 22);
            this.XCurrentPosTextBox.TabIndex = 18;
            this.XCurrentPosTextBox.Text = "0.0000";
            // 
            // YCurrentPosTextBox
            // 
            this.YCurrentPosTextBox.Location = new System.Drawing.Point(291, 127);
            this.YCurrentPosTextBox.Name = "YCurrentPosTextBox";
            this.YCurrentPosTextBox.Size = new System.Drawing.Size(75, 22);
            this.YCurrentPosTextBox.TabIndex = 19;
            this.YCurrentPosTextBox.Text = "0.0000";
            // 
            // setXButton
            // 
            this.setXButton.Location = new System.Drawing.Point(210, 98);
            this.setXButton.Name = "setXButton";
            this.setXButton.Size = new System.Drawing.Size(75, 23);
            this.setXButton.TabIndex = 22;
            this.setXButton.Text = "Set X";
            this.setXButton.UseVisualStyleBackColor = true;
            this.setXButton.Click += new System.EventHandler(this.setXButton_Click);
            // 
            // SetYButton
            // 
            this.SetYButton.Location = new System.Drawing.Point(210, 127);
            this.SetYButton.Name = "SetYButton";
            this.SetYButton.Size = new System.Drawing.Size(75, 23);
            this.SetYButton.TabIndex = 23;
            this.SetYButton.Text = "Set Y";
            this.SetYButton.UseVisualStyleBackColor = true;
            this.SetYButton.Click += new System.EventHandler(this.SetYButton_Click);
            // 
            // zeroXbutton
            // 
            this.zeroXbutton.Location = new System.Drawing.Point(210, 69);
            this.zeroXbutton.Name = "zeroXbutton";
            this.zeroXbutton.Size = new System.Drawing.Size(75, 23);
            this.zeroXbutton.TabIndex = 24;
            this.zeroXbutton.Text = "Zero  X";
            this.zeroXbutton.UseVisualStyleBackColor = true;
            this.zeroXbutton.Click += new System.EventHandler(this.zeroXbutton_Click);
            // 
            // zeroYbutton
            // 
            this.zeroYbutton.Location = new System.Drawing.Point(291, 69);
            this.zeroYbutton.Name = "zeroYbutton";
            this.zeroYbutton.Size = new System.Drawing.Size(75, 23);
            this.zeroYbutton.TabIndex = 25;
            this.zeroYbutton.Text = "Zero Y";
            this.zeroYbutton.UseVisualStyleBackColor = true;
            this.zeroYbutton.Click += new System.EventHandler(this.zeroYbutton_Click);
            // 
            // zeroAllbutton
            // 
            this.zeroAllbutton.Location = new System.Drawing.Point(372, 69);
            this.zeroAllbutton.Name = "zeroAllbutton";
            this.zeroAllbutton.Size = new System.Drawing.Size(75, 23);
            this.zeroAllbutton.TabIndex = 26;
            this.zeroAllbutton.Text = "Zero All";
            this.zeroAllbutton.UseVisualStyleBackColor = true;
            this.zeroAllbutton.Click += new System.EventHandler(this.zeroAllbutton_Click);
            // 
            // Xlabel
            // 
            this.Xlabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Xlabel.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Xlabel.Location = new System.Drawing.Point(31, 368);
            this.Xlabel.Name = "Xlabel";
            this.Xlabel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 2);
            this.Xlabel.Size = new System.Drawing.Size(132, 25);
            this.Xlabel.TabIndex = 35;
            this.Xlabel.Text = "X: 0.0000";
            // 
            // Ylabel
            // 
            this.Ylabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Ylabel.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ylabel.Location = new System.Drawing.Point(31, 393);
            this.Ylabel.Name = "Ylabel";
            this.Ylabel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 2);
            this.Ylabel.Size = new System.Drawing.Size(132, 25);
            this.Ylabel.TabIndex = 36;
            this.Ylabel.Text = "Y: 0.0000";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Items.AddRange(new object[] {
            "Load .vdx to populate list",
            "test data:",
            "0.000, 0.000",
            "1.000, 1.000",
            "0.500, 0.000",
            "1.000, 0.500"});
            this.listBox1.Location = new System.Drawing.Point(388, 175);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(159, 308);
            this.listBox1.TabIndex = 37;
            // 
            // MoveTobutton
            // 
            this.MoveTobutton.Location = new System.Drawing.Point(388, 489);
            this.MoveTobutton.Name = "MoveTobutton";
            this.MoveTobutton.Size = new System.Drawing.Size(75, 23);
            this.MoveTobutton.TabIndex = 38;
            this.MoveTobutton.Text = "MoveTo";
            this.MoveTobutton.UseVisualStyleBackColor = true;
            this.MoveTobutton.Click += new System.EventHandler(this.MoveTobutton_Click);
            // 
            // SetAsXYbutton
            // 
            this.SetAsXYbutton.Location = new System.Drawing.Point(472, 489);
            this.SetAsXYbutton.Name = "SetAsXYbutton";
            this.SetAsXYbutton.Size = new System.Drawing.Size(75, 23);
            this.SetAsXYbutton.TabIndex = 39;
            this.SetAsXYbutton.Text = "SetAsXY";
            this.SetAsXYbutton.UseVisualStyleBackColor = true;
            this.SetAsXYbutton.Click += new System.EventHandler(this.SetAsXYbutton_Click);
            // 
            // OutputLabel
            // 
            this.OutputLabel.BackColor = System.Drawing.Color.White;
            this.OutputLabel.Cursor = System.Windows.Forms.Cursors.Cross;
            this.OutputLabel.Location = new System.Drawing.Point(553, 33);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(450, 450);
            this.OutputLabel.TabIndex = 40;
            // 
            // XScalebutton
            // 
            this.XScalebutton.Location = new System.Drawing.Point(210, 39);
            this.XScalebutton.Name = "XScalebutton";
            this.XScalebutton.Size = new System.Drawing.Size(75, 23);
            this.XScalebutton.TabIndex = 41;
            this.XScalebutton.Text = "X Scale";
            this.XScalebutton.UseVisualStyleBackColor = true;
            // 
            // YScalebutton
            // 
            this.YScalebutton.Location = new System.Drawing.Point(372, 39);
            this.YScalebutton.Name = "YScalebutton";
            this.YScalebutton.Size = new System.Drawing.Size(75, 23);
            this.YScalebutton.TabIndex = 42;
            this.YScalebutton.Text = "Y Scale";
            this.YScalebutton.UseVisualStyleBackColor = true;
            // 
            // ViewZoomLabel
            // 
            this.ViewZoomLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ViewZoomLabel.Location = new System.Drawing.Point(637, 489);
            this.ViewZoomLabel.Name = "ViewZoomLabel";
            this.ViewZoomLabel.Size = new System.Drawing.Size(100, 23);
            this.ViewZoomLabel.TabIndex = 43;
            this.ViewZoomLabel.Text = "Zoom: ";
            // 
            // ViewXLabel
            // 
            this.ViewXLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ViewXLabel.Location = new System.Drawing.Point(743, 489);
            this.ViewXLabel.Name = "ViewXLabel";
            this.ViewXLabel.Size = new System.Drawing.Size(100, 23);
            this.ViewXLabel.TabIndex = 44;
            this.ViewXLabel.Text = "X: ";
            // 
            // ViewYLabel
            // 
            this.ViewYLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ViewYLabel.Location = new System.Drawing.Point(849, 489);
            this.ViewYLabel.Name = "ViewYLabel";
            this.ViewYLabel.Size = new System.Drawing.Size(100, 23);
            this.ViewYLabel.TabIndex = 45;
            this.ViewYLabel.Text = "Y: ";
            // 
            // RunButton
            // 
            this.RunButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunButton.Location = new System.Drawing.Point(388, 531);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(243, 27);
            this.RunButton.TabIndex = 46;
            this.RunButton.Text = "[Run Drilling Program]";
            this.RunButton.UseVisualStyleBackColor = true;
            // 
            // SeekZeroButton
            // 
            this.SeekZeroButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SeekZeroButton.Location = new System.Drawing.Point(5, 531);
            this.SeekZeroButton.Name = "SeekZeroButton";
            this.SeekZeroButton.Size = new System.Drawing.Size(243, 27);
            this.SeekZeroButton.TabIndex = 47;
            this.SeekZeroButton.Text = "[Find Axis Origins]";
            this.SeekZeroButton.UseVisualStyleBackColor = true;
            // 
            // ReloadUSBbutton
            // 
            this.ReloadUSBbutton.Location = new System.Drawing.Point(107, 39);
            this.ReloadUSBbutton.Name = "ReloadUSBbutton";
            this.ReloadUSBbutton.Size = new System.Drawing.Size(94, 23);
            this.ReloadUSBbutton.TabIndex = 48;
            this.ReloadUSBbutton.Text = "Reload USB";
            this.ReloadUSBbutton.UseVisualStyleBackColor = true;
            this.ReloadUSBbutton.Click += new System.EventHandler(this.ReloadUSBbutton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 723);
            this.Controls.Add(this.ReloadUSBbutton);
            this.Controls.Add(this.SeekZeroButton);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.ViewYLabel);
            this.Controls.Add(this.ViewXLabel);
            this.Controls.Add(this.ViewZoomLabel);
            this.Controls.Add(this.YScalebutton);
            this.Controls.Add(this.XScalebutton);
            this.Controls.Add(this.OutputLabel);
            this.Controls.Add(this.SetAsXYbutton);
            this.Controls.Add(this.MoveTobutton);
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
            this.Controls.Add(this.checkBoxB);
            this.Controls.Add(this.checkBoxT);
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
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CNC Drill Machine Controller (Software Interface)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel XMinStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel XMaxStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel YMinStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel YMaxStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel TopStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel BottomStatusLabel;
        private System.Windows.Forms.CheckBox checkBoxX;
        private System.Windows.Forms.CheckBox checkBoxY;
        private System.Windows.Forms.CheckBox checkBoxT;
        private System.Windows.Forms.CheckBox checkBoxB;
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
        private System.Windows.Forms.Button MoveTobutton;
        private System.Windows.Forms.Button SetAsXYbutton;
        private System.Windows.Forms.Label OutputLabel;
        private System.Windows.Forms.Button XScalebutton;
        private System.Windows.Forms.Button YScalebutton;
        private System.Windows.Forms.Label ViewZoomLabel;
        private System.Windows.Forms.Label ViewXLabel;
        private System.Windows.Forms.Label ViewYLabel;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button SeekZeroButton;
        private System.Windows.Forms.Button ReloadUSBbutton;
    }
}

