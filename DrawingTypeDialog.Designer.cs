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
            this.SuspendLayout();
            // 
            // drill
            // 
            this.drill.AutoSize = true;
            this.drill.Checked = true;
            this.drill.Location = new System.Drawing.Point(15, 53);
            this.drill.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.drill.Name = "drill";
            this.drill.Size = new System.Drawing.Size(148, 17);
            this.drill.TabIndex = 0;
            this.drill.TabStop = true;
            this.drill.Text = "Drill (load ellipses location)";
            this.drill.UseVisualStyleBackColor = true;
            // 
            // trace
            // 
            this.trace.AutoSize = true;
            this.trace.Enabled = false;
            this.trace.Location = new System.Drawing.Point(15, 74);
            this.trace.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.trace.Name = "trace";
            this.trace.Size = new System.Drawing.Size(132, 17);
            this.trace.TabIndex = 1;
            this.trace.Text = "Trace (load all shapes)";
            this.trace.UseVisualStyleBackColor = true;
            // 
            // invert
            // 
            this.invert.AutoSize = true;
            this.invert.Checked = true;
            this.invert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.invert.Location = new System.Drawing.Point(15, 32);
            this.invert.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.invert.Name = "invert";
            this.invert.Size = new System.Drawing.Size(189, 17);
            this.invert.TabIndex = 2;
            this.invert.Text = "Invert Layout (for trough-hole PCB)";
            this.invert.UseVisualStyleBackColor = true;
            // 
            // OKbutton
            // 
            this.OKbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKbutton.Location = new System.Drawing.Point(187, 212);
            this.OKbutton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(56, 19);
            this.OKbutton.TabIndex = 3;
            this.OKbutton.Text = "Ok";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.button2_Click);
            // 
            // Cancelbutton
            // 
            this.Cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancelbutton.Location = new System.Drawing.Point(127, 212);
            this.Cancelbutton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.Size = new System.Drawing.Size(56, 19);
            this.Cancelbutton.TabIndex = 4;
            this.Cancelbutton.Text = "Cancel";
            this.Cancelbutton.UseVisualStyleBackColor = true;
            this.Cancelbutton.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select how the drawing data will be interpreted:";
            // 
            // machine
            // 
            this.machine.AutoSize = true;
            this.machine.Enabled = false;
            this.machine.Location = new System.Drawing.Point(15, 95);
            this.machine.Margin = new System.Windows.Forms.Padding(2);
            this.machine.Name = "machine";
            this.machine.Size = new System.Drawing.Size(193, 17);
            this.machine.TabIndex = 6;
            this.machine.Text = "Machine (load 3D Path/Heightmap)";
            this.machine.UseVisualStyleBackColor = true;
            // 
            // flipvdx
            // 
            this.flipvdx.AutoSize = true;
            this.flipvdx.Checked = true;
            this.flipvdx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.flipvdx.Location = new System.Drawing.Point(15, 116);
            this.flipvdx.Margin = new System.Windows.Forms.Padding(2);
            this.flipvdx.Name = "flipvdx";
            this.flipvdx.Size = new System.Drawing.Size(168, 17);
            this.flipvdx.TabIndex = 7;
            this.flipvdx.Text = "Flip Vertical Axis On VDX Files";
            this.flipvdx.UseVisualStyleBackColor = true;
            // 
            // resetorigin
            // 
            this.resetorigin.AutoSize = true;
            this.resetorigin.Checked = true;
            this.resetorigin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resetorigin.Location = new System.Drawing.Point(15, 137);
            this.resetorigin.Margin = new System.Windows.Forms.Padding(2);
            this.resetorigin.Name = "resetorigin";
            this.resetorigin.Size = new System.Drawing.Size(141, 17);
            this.resetorigin.TabIndex = 8;
            this.resetorigin.Text = "Reset Drawing Origin to:";
            this.resetorigin.UseVisualStyleBackColor = true;
            // 
            // xreset
            // 
            this.xreset.Location = new System.Drawing.Point(37, 159);
            this.xreset.Name = "xreset";
            this.xreset.Size = new System.Drawing.Size(100, 20);
            this.xreset.TabIndex = 9;
            this.xreset.Text = "0.200";
            // 
            // yreset
            // 
            this.yreset.Location = new System.Drawing.Point(143, 159);
            this.yreset.Name = "yreset";
            this.yreset.Size = new System.Drawing.Size(100, 20);
            this.yreset.TabIndex = 10;
            this.yreset.Text = "0.200";
            // 
            // DrawingTypeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 242);
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
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DrawingTypeDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DrawingTypeDialog";
            this.TopMost = true;
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
    }
}