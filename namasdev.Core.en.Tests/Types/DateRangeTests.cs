using System;
using System.Linq;
using namasdev.Core.Exceptions;
using namasdev.Core.Types;
using Xunit;

namespace namasdev.Core.en.Tests.Types
{
    public class DateRangeTests
    {
        private static readonly DateTime From = new DateTime(2024, 1, 1);
        private static readonly DateTime To = new DateTime(2024, 1, 7);

        [Fact]
        public void Constructor_InvalidRange_Throws()
        {
            Assert.Throws<ExceptionFriendlyMessage>(() => new DateRange(To, From));
        }

        [Fact]
        public void DiffInDays_ReturnsCorrectDayCount()
        {
            var range = new DateRange(From, To);
            Assert.Equal(6, range.DiffInDays());
        }

        [Fact]
        public void DiffInHours_ReturnsCorrectHours()
        {
            var range = new DateRange(From, To);
            Assert.Equal(TimeSpan.FromDays(6), range.DiffInHours());
        }

        [Fact]
        public void GetDates_ReturnsAllDatesInRange()
        {
            var range = new DateRange(From, To);
            var dates = range.GetDates().ToList();
            Assert.Equal(7, dates.Count);
            Assert.Equal(From, dates.First());
            Assert.Equal(To, dates.Last());
        }

        [Fact]
        public void CountDays_NoExclusions_ReturnsAllDays()
        {
            var range = new DateRange(From, To); // Jan 1-7
            Assert.Equal(7, range.CountDays());
        }

        [Fact]
        public void CountDays_ExcludingWeekends_ReturnsOnlyWorkdays()
        {
            // Jan 1 (Mon) to Jan 7 (Sun) 2024
            var range = new DateRange(new DateTime(2024, 1, 1), new DateTime(2024, 1, 7));
            var count = range.CountDays([DayOfWeek.Saturday, DayOfWeek.Sunday]);
            Assert.Equal(5, count); // Mon-Fri
        }

        [Fact]
        public void SameDay_DiffInDays_ReturnsZero()
        {
            var range = new DateRange(From, From);
            Assert.Equal(0, range.DiffInDays());
        }
    }
}
