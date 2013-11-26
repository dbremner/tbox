using System;

namespace Common.Tools
{
    public static class DateTimeExtensions
    {
        public static DateTime GetFirstDayOfWeek(this DateTime date)
        {
            var daysAhead = (DayOfWeek.Sunday - (int)date.DayOfWeek);
            date = date.AddDays((int)daysAhead);
            return date;
        }

        public static DateTime GetLastDayOfWeek(this DateTime date)
        {
            var daysAhead = DayOfWeek.Saturday - (int)date.DayOfWeek;
            date = date.AddDays((int)daysAhead);
            return date;
        }

        public static DateTime Normalize(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }
    }
}
