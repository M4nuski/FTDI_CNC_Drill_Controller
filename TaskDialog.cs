using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CNC_Drill_Controller1
{
    public partial class TaskDialog : Form
    {
        public TaskDialog()
        {
            InitializeComponent();
        }

        public new DialogResult ShowDialog()
        {
            button1.Enabled = true;
            button2.Enabled = false;
            label1.Text = "Running Task:";
            return base.ShowDialog();
        }

        public void update(int progress)
        {
            progressBar1.Value = progress;
        }

        public void done()
        {
            button2.Enabled = true;
            button1.Enabled = false;
            label1.Text = "Task Completed.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
