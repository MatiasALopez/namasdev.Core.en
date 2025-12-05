using System;

namespace namasdev.Core.Types
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime AddWeeks(this DateTime dateTime, int numberOfWeeks)
        {
            return dateTime.AddDays(numberOfWeeks * 7);
        }

        public static DateTime? ToLocalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToLocalTime() : (DateTime?)null;
        }

        public static DateTime? ToUniversalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToUniversalTime() : (DateTime?)null;
        }

        public static DateTime ChangeKind(this DateTime dateTime, DateTimeKind newKind)
        {
            return new DateTime(dateTime.Ticks, newKind);
        }

        public static DateTime TruncateToHour(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
        }

        public static bool IsWorkday(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        public static DateTime AddWorkingDays(this DateTime date, int value)
        {
            if (value == 0)
            {
                return date;
            }

            bool add = value > 0;
            int count = 0;
            while (count <= value)
            {
                date = date.AddDays(add ? 1 : -1);

                if (date.IsWorkday())
                {
                    count++;
                }
            }

            return date;
        }
    }
}
