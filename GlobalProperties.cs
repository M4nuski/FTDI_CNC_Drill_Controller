namespace CNC_Drill_Controller1
{
    static class GlobalProperties
    {
        public const int max_steps_per_cylce = 48; //1 turn
        public static byte[] stepBytes = { 0x33, 0x66, 0xCC, 0x99 };
        public static byte portDirectionMask = 61;//61 = 0x3D = b'00111101' = in in out out  out out in out
        
        public static int X_Scale = 961;
        public static int Y_Scale = 961;
        public static int X_Backlash = 4;
        public static int Y_Backlash = 4;

        public static string Logfile_Filename = "CNC_Drill_CTRL.log";

        //private const int steps_between_sync = 48; //todo sync with drive rods's rotaty switches
    }
}
