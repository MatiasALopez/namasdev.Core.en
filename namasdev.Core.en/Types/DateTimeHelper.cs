using System;
using System.Collections.Generic;
using System.Linq;

using namasdev.Core.Exceptions;

namespace namasdev.Core.Types
{
    public static class DateTimeHelper
    {
        public static DateTime? CreateFromDateAndTime(DateTime? date, TimeSpan? time)
        {
            return date.HasValue
                ? date.Value.Add(time ?? TimeSpan.Zero)
                : (DateTime?)null;
        }

        public static DateTime CreateFromDateAndTime(DateTime date, TimeSpan time)
        {
            return date.Date.Add(time);
        }

        public static DateTime CreateUtcFromUnixTime(long unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);
        }

        public static long CreateUnixTimeFromDateTime(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        public static DateTime Min(DateTime date1, DateTime date2)
        {
            return date1 < date2
                ? date1
                : date2;
        }

        public static DateTime Min(DateTime? date1, DateTime? date2, DateTime defaultDate)
        {
            return Min(date1 ?? defaultDate, date2 ?? defaultDate);
        }

        public static DateTime Max(DateTime date1, DateTime date2)
        {
            return date1 > date2
                ? date1
                : date2;
        }

        public static IEnumerable<DateTime> GetDatesInRange(DateTime from, DateTime to)
        {
            ValidateRangeAndThrow(from, to,
                includeTime: false);

            var res = new List<DateTime>();

            DateTime date = from;
            while (date <= to)
            {
                res.Add(date);

                date = date.AddDays(1);
            }

            return res;
        }

        public static int DiffInDays(DateTime from, DateTime to)
        {
            if (from > to)
            {
                return (int)(from - to.Date).TotalDays;
            }

            return (int)(to - from.Date).TotalDays;
        }

        public static TimeSpan DiffInHours(DateTime from, DateTime to)
        {
            ValidateRangeAndThrow(from, to,
                includeTime: true);

            return to.Subtract(from);
        }

        public static void ValidateRangeAndThrow(DateTime from, DateTime to,
            bool includeTime = false)
        {
            string errorMessage;
            if (!Validation.Validator.ValidateDateRange(from, to,
                out errorMessage,
                includeTime: includeTime))
            {
                throw new ExceptionFriendlyMessage(errorMessage);
            }
        }

        public static DateTime GetWorkingDayInMonth(int month, int yera, int workingDayNumber)
        {
            DateTime date = new DateTime(yera, month, 1),
                workingDay = DateTime.MinValue;
            int count = 0;
            while (date.Month == month)
            {
                if (date.IsWorkday())
                {
                    workingDay = date;
                    count++;
                }

                if (count == workingDayNumber)
                {
                    return date;
                }

                date = date.AddDays(1);
            }

            // NOTA (ML): if we reach this point no day was found so we return the last working day of the month
            return workingDay;
        }

        public static bool HourIsInRange(DateTime dateTime, TimeSpan hourFrom, TimeSpan hourTo)
        {
            return hourFrom < hourTo
                ? dateTime.TimeOfDay >= hourFrom && dateTime.TimeOfDay <= hourTo
                : dateTime.TimeOfDay >= hourFrom || dateTime.TimeOfDay <= hourTo;
        }

        public static int CountDays(DateTime from, DateTime to,
            DayOfWeek[] weekDaysToExclude = null)
        {
            return CountDays(GetDatesInRange(from, to), weekDaysToExclude);
        }

        public static int CountDays(IEnumerable<DateTime> dates,
            DayOfWeek[] weekDaysToExclude = null)
        {
            int count = 0;
            foreach (var date in dates)
            {
                if (weekDaysToExclude == null
                    || !weekDaysToExclude.Contains(date.DayOfWeek))
                {
                    count++;
                }
            }
            return count;
        }

        public static int CountDays(IEnumerable<DateTime> dates, DateTime from, DateTime to,
            DayOfWeek[] weekDaysToExclude = null)
        {
            return dates
                .Count(date =>
                    date <= to
                    && date >= from
                    && !weekDaysToExclude.Contains(date.DayOfWeek));
        }

        public static int CountDays(List<DateTime> dates, List<DateTime> datesRange,
            DayOfWeek[] weekDaysToExclude = null)
        {
            dates.RemoveAll(item => !datesRange.Contains(item));
            return dates
                .Count(date => !weekDaysToExclude.Contains(date.DayOfWeek));
        }
    }
}
