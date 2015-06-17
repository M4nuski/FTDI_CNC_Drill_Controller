using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M4nuskomponents;

namespace CNC_Drill_Controller1
{
    static class ExtLog
    {
        public static Logger Logger;

        static public void AddLine(string Text)
        {
            if (Logger != null) Logger.AddLine(Text);
        }
    }
}
