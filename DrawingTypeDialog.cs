using System;
using System.Windows.Forms;

namespace CNC_Drill_Controller1
{
    public partial class DrawingTypeDialog : Form
    {

        public enum DrawingType
        {
            Drill,
            Trace,
            Machine
        }

        public struct DrawingConfigStruct
        {
            public DrawingType Type;
            public bool Inverted;
            public int svg_PPI;
            public bool vdx_vertical_flip;
            public bool reset_origin;
            public float origin_x;
            public float origin_y;
            public int gerber_intLen;
            public int gerber_fractLen;
        }

        public DrawingConfigStruct DrawingConfig = new DrawingConfigStruct //output struct and default;
        {   Type = DrawingType.Drill,
            Inverted = true, 
            svg_PPI = 72, 
            vdx_vertical_flip = true,
            reset_origin = true, 
            origin_x = 0.200f,
            origin_y = 0.200f,
            gerber_intLen = 2,
            gerber_fractLen = 4
        }; 
        
        public DrawingTypeDialog()
        {
            InitializeComponent();
        }

        public new DialogResult ShowDialog()
        {
            invert.Checked = DrawingConfig.Inverted;

            if (DrawingConfig.Type == DrawingType.Drill) drill.Select();
            if (DrawingConfig.Type == DrawingType.Trace) trace.Select();
            if (DrawingConfig.Type == DrawingType.Machine) machine.Select();

            flipvdx.Checked = DrawingConfig.vdx_vertical_flip;

            resetorigin.Checked = DrawingConfig.reset_origin;
            xreset.Text = DrawingConfig.origin_x.ToString("F3");
            yreset.Text = DrawingConfig.origin_y.ToString("F3");

            gerberInt.Value = DrawingConfig.gerber_intLen;
            gerberFract.Value = DrawingConfig.gerber_fractLen;

            return base.ShowDialog();
        }

        public DialogResult ShowDialog(DrawingConfigStruct InitialConfig)
        {
            DrawingConfig = InitialConfig;
            return ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var btn = (sender as Button);
            if (btn != null)
            {
                if (btn.DialogResult == DialogResult.OK)
                {
                    DrawingConfig.Inverted = invert.Checked;

                    if (drill.Checked) DrawingConfig.Type = DrawingType.Drill;
                    if (trace.Checked) DrawingConfig.Type = DrawingType.Trace;
                    if (machine.Checked) DrawingConfig.Type = DrawingType.Machine;

                    DrawingConfig.vdx_vertical_flip = flipvdx.Checked;

                    DrawingConfig.reset_origin = resetorigin.Checked;

                    DrawingConfig.origin_x = TextConverter.SafeTextToFloat(xreset.Text, 0.200f);
                    DrawingConfig.origin_y = TextConverter.SafeTextToFloat(yreset.Text, 0.200f);

                    DrawingConfig.gerber_intLen = (int)gerberInt.Value; 
                    DrawingConfig.gerber_fractLen = (int)gerberFract.Value;
                }
                DialogResult = btn.DialogResult;                
            }

        }


    }
}
