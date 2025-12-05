using System;
using System.Collections.Generic;

namespace namasdev.Core.Types
{
    public class DateRange
    {
        public DateRange(DateTime from, DateTime to)
        {
            DateTimeHelper.ValidateRangeAndThrow(from, to);

            From = from;
            To = to;
        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public IEnumerable<DateTime> GetDates()
        {
            return DateTimeHelper.GetDatesInRange(From, To);
        }

        public TimeSpan DiffInHours()
        {
            return DateTimeHelper.DiffInHours(From, To);
        }

        public int DiffInDays()
        {
            return DateTimeHelper.DiffInDays(From, To);
        }

        public int CountDays(
            DayOfWeek[] daysOfWeekToExclude = null)
        {
            return DateTimeHelper.CountDays(From, To, daysOfWeekToExclude);
        }
    }
}
