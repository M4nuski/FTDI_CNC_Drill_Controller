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
    public partial class DrawingTypeDialog : Form
    {

        public enum DrawingType
        {
            Drill,
            Etch
        }
        public struct DrawingConfigStruct
        {
            public DrawingType Type;
            public bool Inverted;
        }

        public DrawingConfigStruct DrawingConfig; // output structure
        
        public DrawingTypeDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(DrawingConfigStruct InitialConfig)
        {
            DrawingConfig = InitialConfig;
            return base.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var btn = (sender as Button);
            if (btn != null)
            {
                if (btn.DialogResult == DialogResult.OK)
                {
                    DrawingConfig.Inverted = checkBox1.Checked;
                    DrawingConfig.Type = radioButton1.Checked ? DrawingType.Drill : DrawingType.Etch;
                }
                DialogResult = btn.DialogResult;                
            }

        }
    }
}
