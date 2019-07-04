using System;

namespace Commons.Scripts.Utility
{
    public static class TimeUtility
    {
        public static long GetUnixTime(DateTime time)
        {
            return (long) (time - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}