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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.logger1 = new M4nuskomponents.Logger();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
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
            this.Sendbutton = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.bevel1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(113, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(1198, 24);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.Text = "[None]";
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
            this.logger1.Location = new System.Drawing.Point(5, 651);
            this.logger1.Multiline = true;
            this.logger1.Name = "logger1";
            this.logger1.ReadOnly = true;
            this.logger1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logger1.Size = new System.Drawing.Size(1309, 179);
            this.logger1.TabIndex = 2;
            this.logger1.TimeStamp = true;
            this.logger1.TimeStampFormat = "HH-mm-ss";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(87, 147);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 76);
            this.button1.TabIndex = 3;
            this.button1.Text = "Y -";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(87, 275);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(40, 76);
            this.button2.TabIndex = 4;
            this.button2.Text = "Y +";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(134, 229);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(76, 40);
            this.button3.TabIndex = 5;
            this.button3.Text = "X +";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(5, 229);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(76, 40);
            this.button4.TabIndex = 6;
            this.button4.Text = "X -";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 830);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1314, 25);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Maximum = 4096;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(200, 19);
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
            this.checkBoxX.Location = new System.Drawing.Point(15, 39);
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
            this.checkBoxY.Location = new System.Drawing.Point(15, 66);
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
            this.checkBoxT.Location = new System.Drawing.Point(15, 93);
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
            this.checkBoxB.Location = new System.Drawing.Point(15, 120);
            this.checkBoxB.Name = "checkBoxB";
            this.checkBoxB.Size = new System.Drawing.Size(172, 21);
            this.checkBoxB.TabIndex = 11;
            this.checkBoxB.Text = "Drill Cycle from Bottom";
            this.checkBoxB.UseVisualStyleBackColor = true;
            this.checkBoxB.CheckedChanged += new System.EventHandler(this.checkBoxB_CheckedChanged);
            // 
            // Sendbutton
            // 
            this.Sendbutton.Location = new System.Drawing.Point(1214, 39);
            this.Sendbutton.Name = "Sendbutton";
            this.Sendbutton.Size = new System.Drawing.Size(88, 23);
            this.Sendbutton.TabIndex = 12;
            this.Sendbutton.Text = "Force Pool";
            this.Sendbutton.UseVisualStyleBackColor = true;
            this.Sendbutton.Click += new System.EventHandler(this.Sendbutton_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
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
            this.comboBox2.Location = new System.Drawing.Point(6, 357);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(204, 24);
            this.comboBox2.TabIndex = 13;
            this.comboBox2.Text = "1";
            // 
            // bevel1
            // 
            this.bevel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.bevel1.Location = new System.Drawing.Point(0, 33);
            this.bevel1.Name = "bevel1";
            this.bevel1.Size = new System.Drawing.Size(218, 355);
            this.bevel1.TabIndex = 14;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 125;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1314, 855);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.Sendbutton);
            this.Controls.Add(this.checkBoxB);
            this.Controls.Add(this.checkBoxT);
            this.Controls.Add(this.checkBoxY);
            this.Controls.Add(this.checkBoxX);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.logger1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.bevel1);
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

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private M4nuskomponents.Logger logger1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
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
        private System.Windows.Forms.Button Sendbutton;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem saveLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label bevel1;
        private System.Windows.Forms.Timer timer1;
    }
}

