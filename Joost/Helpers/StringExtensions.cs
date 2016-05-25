using System;

namespace Joost.Helpers
{
    public static class StringExtensions
    {
        public static string FormatTimeSince(this DateTime theDate)
        {
            var defaultDate = new DateTime();
            if (theDate == defaultDate) return string.Empty;
            var ts = DateTime.Now.Subtract(theDate);
            if (ts.TotalSeconds < 60)
            {
                return string.Format("{0} sec", ts.Seconds);
            }
            if (ts.TotalMinutes < 60)
            {
                return string.Format("{0} min", ts.Minutes);
            }
            if (ts.TotalHours >= 1 && ts.TotalHours < 2)
            {
                return string.Format("{0} hr", ts.Hours);
            }
            if (ts.TotalHours < 24)
            {
                return string.Format("{0} hrs", ts.Hours);
            }
            if (ts.TotalDays >= 1 && ts.TotalDays < 2)
            {
                return string.Format("{0} day", ts.Days);
            }
            return string.Format("{0} days", ts.Days);
        }
    }
}