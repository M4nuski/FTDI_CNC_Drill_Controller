using System;
using System.Windows.Forms;

namespace CNC_Drill_Controller1
{
    public partial class TaskDialog : Form
    {
        private DateTime startTime;
        public TaskDialog()
        {
            InitializeComponent();
        }

        public new DialogResult ShowDialog()
        {
            undone();
            return base.ShowDialog();
        }

        public new DialogResult ShowDialog(IWin32Window owner)
        {
            undone();
            return base.ShowDialog(owner);
        }

        public void update(int progress)
        {
            progressBar1.Value = progress;
        }

        private void undone()
        {
            progressBar1.Value = 0;
            button1.Enabled = true;
            button2.Enabled = false;
            label1.Text = "Running Task:";
            startTime = DateTime.Now;
            timer1.Enabled = true;
        }

        public void done()
        {
            progressBar1.Value = 100;
            button2.Enabled = true;
            button1.Enabled = false;
            label1.Text = "Task Completed.";
            timer1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = (DateTime.Now.Subtract(startTime)).ToString("hh\\:mm\\:ss");
        }

    }
}
