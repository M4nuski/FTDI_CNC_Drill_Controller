using System;
using System.Windows.Forms;

namespace CNC_Drill_Controller1
{
    static class GlobalProperties
    {
        //Hardware config 
        public static int numStepsPerTurns = 48;//48 steps per turn, double phase
        public static byte[] stepBytes = { 0x03, 0x06, 0x0C, 0x09 };
        public static int numStepBytes = 4;
        public static byte numStepMask = 0x03;//b'0000 0011' //(stepByte.length-1)
        
        //Interface config
        public static uint baudRate = 3000000;
        public static byte portDirectionMask = 250;//250 = 0xFA = b'11111010' = out out out out  out in out in

        //Switches bits of InputByte0
        public static int X_MinSwitch_Bit = 0;
        public static int X_MaxSwitch_Bit = 1;

        public static int Y_MinSwitch_Bit = 2;
        public static int Y_MaxSwitch_Bit = 3;

        public static int TopSwitch_Bit = 4;
        public static int BottomSwitch_Bit = 5;

        //Outputs control bits of OutputByte0 and OutputByte1
        public static int StepMotor_Enable_Bit = 4;
        public static int Torque_Pos_Bit = 5;
        public static int Torque_Neg_Bit = 6;
        public static int Torque_Enable_Bit = 7;

        //Outputs control bits of OutputByte2
        public static int Drill_Cycle_Enable_Bit = 0;
        public static int StepMotor_Throttle_Bit = 4;

        //UI settings
        public static string Logfile_Filename = "CNC_Drill_CTRL.log";
        public static int X_Scale = 960;//961;
        public static int Y_Scale = 960;//961;
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
