using System;
using System.Text;
using System.Windows.Forms;

namespace CNC_Drill_Controller1
{
    public partial class RawUSBForm : Form
    {
        private StringBuilder sb = new StringBuilder();

        public RawUSBForm()
        {
            InitializeComponent();
        }

        public void Update(byte[] usbData)
        {
            if (usbData.Length >= 20)
            {
                textBox1.Clear();
                sb.Clear();
                for (var i = 0; i < 20; i++)
                {
                    sb.Append(i.ToString("D2"));
                    sb.Append(" ");
                    sb.AppendLine(Convert.ToString(usbData[i], 2).PadLeft(8, '0'));
                }
                textBox1.Text = sb.ToString();
            }
        }
    }
}
