using M4nuskomponents;

namespace CNC_Drill_Controller1
{
    static class ExtLog
    {
        public static Logger Logger;

        static public void AddLine(string Text)
        {
            if (Logger != null)
            {
                if (Logger.InvokeRequired)
                {
                    Logger.Invoke((AddLineDelegate) AddLine, new object[] {Text});
                }
                else
                {
                    Logger.AddLine(Text);
                }
            }
        }
    }
}
