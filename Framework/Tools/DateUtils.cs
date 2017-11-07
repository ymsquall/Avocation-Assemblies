using System;

namespace Framework.Tools
{
    public class DateUtils
    {
        static DateTime StartTime = new DateTime(1970,1,1);
        public const long OneMinute = 60 * 1000;
        public const long OneHour = 60 * OneMinute;
        public const long OneDay = 24 * OneHour;
        public const long HelfDay = 12 * OneHour;
        public const int OneFrame = (int)(1f / 60f * 1000f);
        public static long NowMillisecond
        {
            get
            {
                TimeSpan ts = DateTime.Now.Subtract(StartTime);
                return (long)ts.TotalMilliseconds;
            }
        }
        public static DateTime FromMillisecond(long millisecond)
        {
            DateTime dt = DateTime.Parse(StartTime.ToString("yyyy-MM-dd 00:00:00")).AddMilliseconds(millisecond);
            return dt;
        }
        public static long GetMillisecond(string dtStr)
        {
            DateTime dt;
            if (!DateTime.TryParse(dtStr, out dt))
                return 0;
            return GetMillisecond(dt);
        }
        public static long GetMillisecond(DateTime dt)
        {
            TimeSpan ts = dt.Subtract(StartTime);
            return (long)ts.TotalMilliseconds;
        }
    }
}
