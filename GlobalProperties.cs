using System;
using System.Windows.Forms;

namespace CNC_Drill_Controller1
{
    static class GlobalProperties
    {
        //Hardware config 
        public static int numSeekMin = 50; // about an inch
        public static int fastSeekSteps = -5;
        public static int fastSeekDelay = 20;
        public static int slowSeekDelay = 250;
        public static int numSeekMax = 500; // about 10 inches

        public static int drillReleaseNumWait = 20; // 1sec per 0.05 sec test
        public static int drillReleaseWaitTime = 50;

        public static int drillCycleNumWait = 50; // 5sec per 0.1 sec test
        public static int drillCycleWaitTime = 100;
        //public static byte[] stepBytes = { 0x03, 0x06, 0x0C, 0x09 };// single phase
        // 0011 0110 1100 1001 
        public static byte[] stepBytes = { 0b1001, 0b1010, 0b0110, 0b0101 };// dual phase
        // 1001 1010 0110 0101
        // P N  P P  N P  N N
      //  public static byte[] stepBytes = { 0b1001, 0b1000, 0b1010, 0b0010, 0b0110, 0b0100, 0b0101, 0b0001 };// dual phase double step
        // 1001 1000 1010 0010  0110 0100 0101 0001
        // P N  P X  P P  X P   N P  N X  N N  X N
        public static int numStepBytes = 4;
        public static byte numStepMask = 0x03;//b'0000 0011' //(stepByte.length-1)
        //public static int numStepBytes = 8;
        //public static byte numStepMask = 0x07;//b'0000 0111' //(stepByte.length-1)// should be auto computed

        //Interface config
        public static uint baudRate = 3000000;
        public static byte portDirectionMask = 250;//250 = 0xFA = b'11111010' = out out out out  out in out in
        public static byte latency = 24;

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
        public static float X_Scale = 110.43478260869565f;
        public static float Y_Scale = 110.43478260869565f;
        public static float X_Backlash = 0.0f;
        public static float Y_Backlash = 0.0f;
        public static float X_Length = 6.000f;
        public static float Y_Length = 6.000f;

        //UI refresh throttler
        public static int USB_Refresh_Period = 100;
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
                latency = (byte)Properties.Settings.Default["usbTransferLatency"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading USB interface state", MessageBoxButtons.OK);
            }

            //UI settings
            try
            {
                Logfile_Filename = (string)Properties.Settings.Default["Logfile_Filename"];
                X_Scale = (float)Properties.Settings.Default["X_Scale"];
                Y_Scale = (float)Properties.Settings.Default["Y_Scale"];
                X_Backlash = (float)Properties.Settings.Default["X_Backlash"];
                Y_Backlash = (float)Properties.Settings.Default["Y_Backlash"];
                X_Length = (float)Properties.Settings.Default["X_Length"];
                Y_Length = (float)Properties.Settings.Default["Y_Length"];
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
                Properties.Settings.Default["X_Length"] = X_Length;
                Properties.Settings.Default["Y_Length"] = Y_Length;
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
