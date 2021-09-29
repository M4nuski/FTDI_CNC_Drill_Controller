using System;
using System.IO;
using System.Windows.Forms;

namespace CNC_Drill_Controller1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new MainForm());
                //throw new Exception("message dans l'exception");
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var f =  File.CreateText("error.txt");
                f.WriteLine(ex.Message);
                f.WriteLine(ex.StackTrace);
                f.Flush();
                f.Close();
            }
        }
    }
}
