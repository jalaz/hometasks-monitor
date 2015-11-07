namespace HometasksMonitoringPanel.Helpers
{
    public static class StringExtensions
    {
        public static bool Empty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool NotEmpty(this string str)
        {
            return !Empty(str);
        }

        public static string Fmt(this string format, params object[] @params)
        {
            return string.Format(format, @params);
        }
    }
}