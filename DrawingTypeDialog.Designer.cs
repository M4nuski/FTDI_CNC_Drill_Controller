namespace CNC_Drill_Controller1
{
    partial class DrawingTypeDialog
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
            this.drill = new System.Windows.Forms.RadioButton();
            this.trace = new System.Windows.Forms.RadioButton();
            this.invert = new System.Windows.Forms.CheckBox();
            this.OKbutton = new System.Windows.Forms.Button();
            this.Cancelbutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.machine = new System.Windows.Forms.RadioButton();
            this.flipvdx = new System.Windows.Forms.CheckBox();
            this.resetorigin = new System.Windows.Forms.CheckBox();
            this.xreset = new System.Windows.Forms.TextBox();
            this.yreset = new System.Windows.Forms.TextBox();
            this.gerberInt = new System.Windows.Forms.NumericUpDown();
            this.gerberFract = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_DPI_orig = new System.Windows.Forms.RadioButton();
            this.radioButton_DPI_72 = new System.Windows.Forms.RadioButton();
            this.radioButton_DPI_Custom = new System.Windows.Forms.RadioButton();
            this.textBox_DPI = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gerberInt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gerberFract)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // drill
            // 
            this.drill.AutoSize = true;
            this.drill.Checked = true;
            this.drill.Location = new System.Drawing.Point(20, 65);
            this.drill.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.drill.Name = "drill";
            this.drill.Size = new System.Drawing.Size(189, 20);
            this.drill.TabIndex = 0;
            this.drill.TabStop = true;
            this.drill.Text = "Drill (load ellipses location)";
            this.drill.UseVisualStyleBackColor = true;
            // 
            // trace
            // 
            this.trace.AutoSize = true;
            this.trace.Enabled = false;
            this.trace.Location = new System.Drawing.Point(20, 91);
            this.trace.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trace.Name = "trace";
            this.trace.Size = new System.Drawing.Size(167, 20);
            this.trace.TabIndex = 1;
            this.trace.Text = "Trace (load all shapes)";
            this.trace.UseVisualStyleBackColor = true;
            // 
            // invert
            // 
            this.invert.AutoSize = true;
            this.invert.Checked = true;
            this.invert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.invert.Location = new System.Drawing.Point(20, 39);
            this.invert.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.invert.Name = "invert";
            this.invert.Size = new System.Drawing.Size(229, 20);
            this.invert.TabIndex = 2;
            this.invert.Text = "Invert Layout (for Bottom/Mirrored)";
            this.invert.UseVisualStyleBackColor = true;
            // 
            // OKbutton
            // 
            this.OKbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKbutton.Location = new System.Drawing.Point(247, 309);
            this.OKbutton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(75, 23);
            this.OKbutton.TabIndex = 3;
            this.OKbutton.Text = "Ok";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.button2_Click);
            // 
            // Cancelbutton
            // 
            this.Cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancelbutton.Location = new System.Drawing.Point(167, 309);
            this.Cancelbutton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.Size = new System.Drawing.Size(75, 23);
            this.Cancelbutton.TabIndex = 4;
            this.Cancelbutton.Text = "Cancel";
            this.Cancelbutton.UseVisualStyleBackColor = true;
            this.Cancelbutton.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(283, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select how the drawing data will be interpreted:";
            // 
            // machine
            // 
            this.machine.AutoSize = true;
            this.machine.Enabled = false;
            this.machine.Location = new System.Drawing.Point(20, 117);
            this.machine.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.machine.Name = "machine";
            this.machine.Size = new System.Drawing.Size(237, 20);
            this.machine.TabIndex = 6;
            this.machine.Text = "Machine (load 3D Path/Heightmap)";
            this.machine.UseVisualStyleBackColor = true;
            // 
            // flipvdx
            // 
            this.flipvdx.AutoSize = true;
            this.flipvdx.Checked = true;
            this.flipvdx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.flipvdx.Location = new System.Drawing.Point(20, 143);
            this.flipvdx.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flipvdx.Name = "flipvdx";
            this.flipvdx.Size = new System.Drawing.Size(209, 20);
            this.flipvdx.TabIndex = 7;
            this.flipvdx.Text = "Flip Vertical Axis On VDX Files";
            this.flipvdx.UseVisualStyleBackColor = true;
            // 
            // resetorigin
            // 
            this.resetorigin.AutoSize = true;
            this.resetorigin.Checked = true;
            this.resetorigin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resetorigin.Location = new System.Drawing.Point(20, 169);
            this.resetorigin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.resetorigin.Name = "resetorigin";
            this.resetorigin.Size = new System.Drawing.Size(172, 20);
            this.resetorigin.TabIndex = 8;
            this.resetorigin.Text = "Reset Drawing Origin to:";
            this.resetorigin.UseVisualStyleBackColor = true;
            // 
            // xreset
            // 
            this.xreset.Location = new System.Drawing.Point(49, 196);
            this.xreset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xreset.Name = "xreset";
            this.xreset.Size = new System.Drawing.Size(132, 22);
            this.xreset.TabIndex = 9;
            this.xreset.Text = "0.200";
            // 
            // yreset
            // 
            this.yreset.Location = new System.Drawing.Point(191, 196);
            this.yreset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.yreset.Name = "yreset";
            this.yreset.Size = new System.Drawing.Size(131, 22);
            this.yreset.TabIndex = 10;
            this.yreset.Text = "0.200";
            // 
            // gerberInt
            // 
            this.gerberInt.Location = new System.Drawing.Point(190, 249);
            this.gerberInt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gerberInt.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.gerberInt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.gerberInt.Name = "gerberInt";
            this.gerberInt.Size = new System.Drawing.Size(63, 22);
            this.gerberInt.TabIndex = 11;
            this.gerberInt.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // gerberFract
            // 
            this.gerberFract.Location = new System.Drawing.Point(259, 249);
            this.gerberFract.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gerberFract.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.gerberFract.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.gerberFract.Name = "gerberFract";
            this.gerberFract.Size = new System.Drawing.Size(63, 22);
            this.gerberFract.TabIndex = 12;
            this.gerberFract.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 229);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "Gerber \"n, m\" format:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_DPI);
            this.groupBox1.Controls.Add(this.radioButton_DPI_Custom);
            this.groupBox1.Controls.Add(this.radioButton_DPI_72);
            this.groupBox1.Controls.Add(this.radioButton_DPI_orig);
            this.groupBox1.Location = new System.Drawing.Point(12, 225);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 107);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PPI/DPI";
            // 
            // radioButton_DPI_orig
            // 
            this.radioButton_DPI_orig.AutoSize = true;
            this.radioButton_DPI_orig.Checked = true;
            this.radioButton_DPI_orig.Location = new System.Drawing.Point(12, 24);
            this.radioButton_DPI_orig.Name = "radioButton_DPI_orig";
            this.radioButton_DPI_orig.Size = new System.Drawing.Size(74, 20);
            this.radioButton_DPI_orig.TabIndex = 0;
            this.radioButton_DPI_orig.TabStop = true;
            this.radioButton_DPI_orig.Text = "Original";
            this.radioButton_DPI_orig.UseVisualStyleBackColor = true;
            // 
            // radioButton_DPI_72
            // 
            this.radioButton_DPI_72.AutoSize = true;
            this.radioButton_DPI_72.Location = new System.Drawing.Point(12, 50);
            this.radioButton_DPI_72.Name = "radioButton_DPI_72";
            this.radioButton_DPI_72.Size = new System.Drawing.Size(80, 20);
            this.radioButton_DPI_72.TabIndex = 1;
            this.radioButton_DPI_72.Text = "Force 72";
            this.radioButton_DPI_72.UseVisualStyleBackColor = true;
            // 
            // radioButton_DPI_Custom
            // 
            this.radioButton_DPI_Custom.AutoSize = true;
            this.radioButton_DPI_Custom.Location = new System.Drawing.Point(12, 76);
            this.radioButton_DPI_Custom.Name = "radioButton_DPI_Custom";
            this.radioButton_DPI_Custom.Size = new System.Drawing.Size(76, 20);
            this.radioButton_DPI_Custom.TabIndex = 2;
            this.radioButton_DPI_Custom.Text = "Custom:";
            this.radioButton_DPI_Custom.UseVisualStyleBackColor = true;
            // 
            // textBox_DPI
            // 
            this.textBox_DPI.Location = new System.Drawing.Point(90, 77);
            this.textBox_DPI.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_DPI.Name = "textBox_DPI";
            this.textBox_DPI.Size = new System.Drawing.Size(53, 22);
            this.textBox_DPI.TabIndex = 15;
            this.textBox_DPI.Text = "100";
            // 
            // DrawingTypeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 338);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gerberFract);
            this.Controls.Add(this.gerberInt);
            this.Controls.Add(this.yreset);
            this.Controls.Add(this.xreset);
            this.Controls.Add(this.resetorigin);
            this.Controls.Add(this.flipvdx);
            this.Controls.Add(this.machine);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancelbutton);
            this.Controls.Add(this.OKbutton);
            this.Controls.Add(this.invert);
            this.Controls.Add(this.trace);
            this.Controls.Add(this.drill);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DrawingTypeDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DrawingTypeDialog";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.gerberInt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gerberFract)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton drill;
        private System.Windows.Forms.RadioButton trace;
        private System.Windows.Forms.CheckBox invert;
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Button Cancelbutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton machine;
        private System.Windows.Forms.CheckBox flipvdx;
        private System.Windows.Forms.CheckBox resetorigin;
        private System.Windows.Forms.TextBox xreset;
        private System.Windows.Forms.TextBox yreset;
        private System.Windows.Forms.NumericUpDown gerberInt;
        private System.Windows.Forms.NumericUpDown gerberFract;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_DPI;
        private System.Windows.Forms.RadioButton radioButton_DPI_Custom;
        private System.Windows.Forms.RadioButton radioButton_DPI_72;
        private System.Windows.Forms.RadioButton radioButton_DPI_orig;
    }
}