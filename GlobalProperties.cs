using System;
using System.Windows.Forms;

namespace CNC_Drill_Controller1
{
    static class GlobalProperties
    {
        //Hardware config 
        public static int numStepsPerTurns = 48;//48 steps per turn, double phase
        public static byte[] stepBytes = { 0x33, 0x66, 0xCC, 0x99 };
        public static int numStepBytes = 4;
        public static byte numStepMask = 0x03;//b'0000 0011'
        
        //Interface config
        public static uint baudRate = 4800;
        public static byte portDirectionMask = 61;//61 = 0x3D = b'00111101' = in in out out  out out in out

        //Switches bits of InputByte0
        public static int X_MaxSwitch_Bit = 0;
        public static int X_MinSwitch_Bit = 1;

        public static int Y_MinSwitch_Bit = 2;
        public static int Y_MaxSwitch_Bit = 3;

        public static int TopSwitch_Bit = 4;
        public static int BottomSwitch_Bit = 5;

        //Outputs bits of OutputByte0
        public static int X_PhaseA_Bit = 0;
        public static int X_PhaseB_Bit = 1;
        public static int X_PhaseC_Bit = 2;
        public static int X_PhaseD_Bit = 3;

        public static int Y_PhaseA_Bit = 4;
        public static int Y_PhaseB_Bit = 5;
        public static int Y_PhaseC_Bit = 6;
        public static int Y_PhaseD_Bit = 7;

        //Outputs bits of OutputByte1
        public static int X_Driver_Bit = 0;
        public static int Y_Driver_Bit = 1;
        public static int D_Driver_Bit = 2;
        public static int T_Driver_Bit = 3;

        public static int TQA_Driver_X_Pos_Bit = 4;//todo move to Z_axis
        public static int TQA_Driver_X_Neg_Bit = 5;
        public static int TQA_Driver_Y_Pos_Bit = 6;
        public static int TQA_Driver_Y_Neg_Bit = 7;

        //UI settings
        public static string Logfile_Filename = "CNC_Drill_CTRL.log";
        public static int X_Scale = 1;//961;
        public static int Y_Scale = 1;//961;
        public static int X_Backlash = 0;//4;
        public static int Y_Backlash = 0;//4;

        //UI refresh throttler
        public static int USB_Refresh_Period = 250;
        public static int GlobalProperties_Refresh_Period = 10000;
        public static int Label_Refresh_Period = 100;

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
