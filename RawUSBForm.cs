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
    public partial class RawUSBForm : Form
    {
        private StringBuilder sb = new StringBuilder();

        public RawUSBForm()
        {
            InitializeComponent();
        }

        public void Update(byte[] usbData)
        {
            if (usbData.Length == 64)
            {
                textBox1.Clear();
                sb.Clear();
                for (var i = 0; i < 32; i++)
                {
                    sb.Append(i.ToString("D2"));
                    sb.Append(" ");
                    sb.AppendLine(Convert.ToString(usbData[i], 2).PadLeft(8, '0'));
                }
                textBox1.Text = sb.ToString();

                textBox2.Clear();
                sb.Clear();
                for (var i = 32; i < 64; i++)
                {
                    sb.Append(i.ToString("D2"));
                    sb.Append(" ");
                    sb.AppendLine(Convert.ToString(usbData[i], 2).PadLeft(8, '0'));
                }
                textBox2.Text = sb.ToString();
            }
        }
    }
}
