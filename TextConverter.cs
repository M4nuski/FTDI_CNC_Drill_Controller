
namespace CNC_Drill_Controller1
{
    static class TextConverter
    {
        static public float SafeTextToFloat(string text)
        {
            float res;
            if (float.TryParse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out res))
            {
                return res;
            }
            ExtLog.AddLine("Failed to convert value: " + text);
            return 0.0f;
        }
        static public float SafeTextToFloat(string text, float fallback)
        {
            float res;
            if (float.TryParse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out res))
            {
                return res;
            }
            ExtLog.AddLine("Failed to convert value: " + text);
            return fallback;
        }

        static public int SafeTextToInt(string text)
        {
            int res;
            if (int.TryParse(text, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out res))
            {
                return res;
            }
            ExtLog.AddLine("Failed to convert value: " + text);
            return 0;
        }

        static public int SafeTextToInt(string text, int fallback)
        {
            int res;
            if (int.TryParse(text, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out res))
            {
                return res;
            }
            ExtLog.AddLine("Failed to convert value: " + text);
            return fallback;
        }
    }
}
