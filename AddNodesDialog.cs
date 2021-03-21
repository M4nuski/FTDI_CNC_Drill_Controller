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
    public partial class AddNodesDialog : Form
    {
        public class addnodeclass
        {
            public int pins;
            public string type;
            public string direction;
            public string spacing;
            public float x;
            public float y;
        }

        public addnodeclass DialogData = new addnodeclass();
        public AddNodesDialog()
        {
            InitializeComponent();
        }
        public new DialogResult ShowDialog()
        {
            if (box_type.Text == "") box_type.Text = box_type.Items[0].ToString();
            if (box_dir.Text == "") box_dir.Text = box_dir.Items[0].ToString();
            if (box_spacing.Text == "") box_spacing.Text = box_spacing.Items[0].ToString();

            return base.ShowDialog();
        }

        public DialogResult ShowDialog(float x, float y)
        {
            box_x.Text = x.ToString("F3");
            box_y.Text = y.ToString("F3");

            return ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var btn = (sender as Button);
            if (btn != null)
            {
                if (btn.DialogResult == DialogResult.OK)
                {
                    DialogData.x = TextConverter.SafeTextToFloat(box_x.Text, 0.000f);
                    DialogData.y = TextConverter.SafeTextToFloat(box_y.Text, 0.000f);
                    DialogData.pins = (int)box_pins.Value;

                    DialogData.type = box_type.Text;
                    DialogData.direction = box_dir.Text;
                    DialogData.spacing = box_spacing.Text;
                }
                DialogResult = btn.DialogResult;
            }
        }
    }
}
