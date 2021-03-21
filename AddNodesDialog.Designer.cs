
namespace CNC_Drill_Controller1
{
    partial class AddNodesDialog
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.box_type = new System.Windows.Forms.ComboBox();
            this.box_dir = new System.Windows.Forms.ComboBox();
            this.box_pins = new System.Windows.Forms.NumericUpDown();
            this.box_spacing = new System.Windows.Forms.ComboBox();
            this.box_x = new System.Windows.Forms.TextBox();
            this.box_y = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.box_pins)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(319, 119);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(238, 119);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // box_type
            // 
            this.box_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.box_type.FormattingEnabled = true;
            this.box_type.Items.AddRange(new object[] {
            "DIP",
            "SIP"});
            this.box_type.Location = new System.Drawing.Point(67, 11);
            this.box_type.Name = "box_type";
            this.box_type.Size = new System.Drawing.Size(121, 21);
            this.box_type.TabIndex = 2;
            // 
            // box_dir
            // 
            this.box_dir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.box_dir.FormattingEnabled = true;
            this.box_dir.Items.AddRange(new object[] {
            "Horizontal",
            "Vertical"});
            this.box_dir.Location = new System.Drawing.Point(67, 38);
            this.box_dir.Name = "box_dir";
            this.box_dir.Size = new System.Drawing.Size(121, 21);
            this.box_dir.TabIndex = 3;
            // 
            // box_pins
            // 
            this.box_pins.Location = new System.Drawing.Point(273, 12);
            this.box_pins.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.box_pins.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.box_pins.Name = "box_pins";
            this.box_pins.Size = new System.Drawing.Size(121, 20);
            this.box_pins.TabIndex = 4;
            this.box_pins.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // box_spacing
            // 
            this.box_spacing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.box_spacing.FormattingEnabled = true;
            this.box_spacing.Items.AddRange(new object[] {
            "0.300",
            "0.600"});
            this.box_spacing.Location = new System.Drawing.Point(273, 38);
            this.box_spacing.Name = "box_spacing";
            this.box_spacing.Size = new System.Drawing.Size(121, 21);
            this.box_spacing.TabIndex = 5;
            // 
            // box_x
            // 
            this.box_x.Location = new System.Drawing.Point(67, 65);
            this.box_x.Name = "box_x";
            this.box_x.Size = new System.Drawing.Size(121, 20);
            this.box_x.TabIndex = 6;
            this.box_x.Text = "0.000";
            // 
            // box_y
            // 
            this.box_y.Location = new System.Drawing.Point(273, 66);
            this.box_y.Name = "box_y";
            this.box_y.Size = new System.Drawing.Size(121, 20);
            this.box_y.TabIndex = 7;
            this.box_y.Text = "0.000";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(207, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 14);
            this.label1.TabIndex = 8;
            this.label1.Text = "Pins";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(1, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 14);
            this.label2.TabIndex = 9;
            this.label2.Text = "Type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(1, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 14);
            this.label3.TabIndex = 11;
            this.label3.Text = "Direction";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(207, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 14);
            this.label4.TabIndex = 10;
            this.label4.Text = "Spacing";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(1, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 14);
            this.label5.TabIndex = 13;
            this.label5.Text = "X Position";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(207, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 14);
            this.label6.TabIndex = 12;
            this.label6.Text = "Y Position";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AddNodesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 155);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.box_y);
            this.Controls.Add(this.box_x);
            this.Controls.Add(this.box_spacing);
            this.Controls.Add(this.box_pins);
            this.Controls.Add(this.box_dir);
            this.Controls.Add(this.box_type);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddNodesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Nodes";
            ((System.ComponentModel.ISupportInitialize)(this.box_pins)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox box_type;
        private System.Windows.Forms.ComboBox box_dir;
        private System.Windows.Forms.NumericUpDown box_pins;
        private System.Windows.Forms.ComboBox box_spacing;
        private System.Windows.Forms.TextBox box_x;
        private System.Windows.Forms.TextBox box_y;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}