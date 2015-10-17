using System;
using System.Windows.Forms;

namespace CNC_Drill_Controller1
{
    static class GlobalProperties
    {
        //Hardware config //48 steps per turn, double phase
        public static byte[] stepBytes = { 0x33, 0x66, 0xCC, 0x99 };
        public static int numStepBytes = 4;
        public static byte numStepMask = 0x03;//b'0000 0011'
        

        //Interface config
        public static uint baudRate = 1200;
        public static byte portDirectionMask = 61;//61 = 0x3D = b'00111101' = in in out out  out out in out

        //UI settings
        public static string Logfile_Filename = "CNC_Drill_CTRL.log";
        public static int X_Scale = 961;
        public static int Y_Scale = 961;
        public static int X_Backlash = 4;
        public static int Y_Backlash = 4;

        //USB state
        public static int X_Dir = 0;
        public static int Y_Dir = 0;
        public static int X_Pos = 0;
        public static int Y_Pos = 0;
        public static int X_Delta = 0;
        public static int Y_Delta = 0;

        public static DateTime LastSave;

        static GlobalProperties()
        {
            //USB interface state
            try
            {
                X_Dir = (int)Properties.Settings.Default["X_Last_Direction"];
                Y_Dir = (int)Properties.Settings.Default["Y_Last_Direction"];
                X_Pos = (int)Properties.Settings.Default["X_Abs_Position"];
                Y_Pos = (int)Properties.Settings.Default["Y_Abs_Position"];
                X_Delta = (int)Properties.Settings.Default["X_Delta"];
                Y_Delta = (int)Properties.Settings.Default["Y_Delta"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading USB interface state", MessageBoxButtons.OK);
            }

            //UI settings
            try
            {
                Logfile_Filename = (string)Properties.Settings.Default["Logfile_Filename"];
                X_Scale = (int)Properties.Settings.Default["X_Scale"];
                Y_Scale = (int)Properties.Settings.Default["Y_Scale"];
                X_Backlash = (int)Properties.Settings.Default["X_Backlash"];
                Y_Backlash = (int)Properties.Settings.Default["Y_Backlash"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading UI settings", MessageBoxButtons.OK);
            }
        }

        public static void SaveProperties()
        {
            //USB interface state
            try
            {
                Properties.Settings.Default["X_Last_Direction"] = X_Dir;
                Properties.Settings.Default["Y_Last_Direction"] = Y_Dir;
                Properties.Settings.Default["X_Abs_Position"] = X_Pos;
                Properties.Settings.Default["Y_Abs_Position"] = Y_Pos;
                Properties.Settings.Default["X_Delta"] = X_Delta;
                Properties.Settings.Default["Y_Delta"] = Y_Delta;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving USB interface state", MessageBoxButtons.OK);
            }

            //UI settings
            try
            {
                Properties.Settings.Default["Logfile_Filename"] = Logfile_Filename;
                Properties.Settings.Default["X_Scale"] = X_Scale;
                Properties.Settings.Default["Y_Scale"] = Y_Scale;
                Properties.Settings.Default["X_Backlash"] = X_Backlash;
                Properties.Settings.Default["Y_Backlash"] = Y_Backlash;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving UI settings", MessageBoxButtons.OK);
            }

            Properties.Settings.Default.Save();

            LastSave = DateTime.Now;
        }
    }
}
