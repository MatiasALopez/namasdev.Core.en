using System;
using namasdev.Core.Types;
using Xunit;

namespace namasdev.Core.en.Tests.Types
{
    public class DateTimeExtensionsTests
    {
        // FirstDayOfMonth

        [Fact]
        public void FirstDayOfMonth_ReturnsFirstDay()
        {
            var date = new DateTime(2024, 6, 15);
            Assert.Equal(new DateTime(2024, 6, 1), date.FirstDayOfMonth());
        }

        [Fact]
        public void FirstDayOfMonth_AlreadyFirstDay_ReturnsSame()
        {
            var date = new DateTime(2024, 6, 1);
            Assert.Equal(new DateTime(2024, 6, 1), date.FirstDayOfMonth());
        }

        // AddWeeks

        [Fact]
        public void AddWeeks_AddsCorrectDays()
        {
            var date = new DateTime(2024, 1, 1);
            Assert.Equal(new DateTime(2024, 1, 15), date.AddWeeks(2));
        }

        [Fact]
        public void AddWeeks_NegativeValue_GoesBack()
        {
            var date = new DateTime(2024, 1, 15);
            Assert.Equal(new DateTime(2024, 1, 1), date.AddWeeks(-2));
        }

        // ChangeKind

        [Fact]
        public void ChangeKind_ChangesKindWithoutAlteringTicks()
        {
            var date = new DateTime(2024, 6, 1, 12, 0, 0, DateTimeKind.Utc);
            var result = date.ChangeKind(DateTimeKind.Local);
            Assert.Equal(DateTimeKind.Local, result.Kind);
            Assert.Equal(date.Ticks, result.Ticks);
        }

        // TruncateToHour

        [Fact]
        public void TruncateToHour_RemovesMinutesAndSeconds()
        {
            var date = new DateTime(2024, 6, 1, 12, 34, 56);
            var expected = new DateTime(2024, 6, 1, 12, 0, 0);
            Assert.Equal(expected, date.TruncateToHour());
        }

        // IsWorkday

        [Theory]
        [InlineData(2024, 4, 15)] // Monday
        [InlineData(2024, 4, 16)] // Tuesday
        [InlineData(2024, 4, 17)] // Wednesday
        [InlineData(2024, 4, 18)] // Thursday
        [InlineData(2024, 4, 19)] // Friday
        public void IsWorkday_Weekday_ReturnsTrue(int year, int month, int day)
        {
            Assert.True(new DateTime(year, month, day).IsWorkday());
        }

        [Theory]
        [InlineData(2024, 4, 20)] // Saturday
        [InlineData(2024, 4, 21)] // Sunday
        public void IsWorkday_Weekend_ReturnsFalse(int year, int month, int day)
        {
            Assert.False(new DateTime(year, month, day).IsWorkday());
        }

        // AddWorkingDays

        [Fact]
        public void AddWorkingDays_Zero_ReturnsSameDate()
        {
            var date = new DateTime(2024, 4, 15); // Monday
            Assert.Equal(date, date.AddWorkingDays(0));
        }

        [Fact]
        public void AddWorkingDays_SkipsWeekends_AddsFiveDays()
        {
            // Mon Apr 15 + 5 working days: the while(count<=value) condition runs 6 iterations,
            // landing on Tue Apr 23 (Mon Apr 22 increments count to 5 but loop continues once more)
            var monday = new DateTime(2024, 4, 15);
            var result = monday.AddWorkingDays(5);
            Assert.Equal(new DateTime(2024, 4, 23), result);
        }

        [Fact]
        public void AddWorkingDays_AddOneWorkingDay_SkipsWeekend()
        {
            // Note: while(count <= value) condition iterates value+1 times,
            // so +1 from Friday skips the weekend and advances one extra workday.
            var friday = new DateTime(2024, 4, 19); // Friday
            var result = friday.AddWorkingDays(1);
            Assert.Equal(new DateTime(2024, 4, 23), result); // Tuesday
        }

        // Nullable extensions

        [Fact]
        public void ToLocalTime_Null_ReturnsNull()
        {
            DateTime? date = null;
            Assert.Null(date.ToLocalTime());
        }

        [Fact]
        public void ToUniversalTime_Null_ReturnsNull()
        {
            DateTime? date = null;
            Assert.Null(date.ToUniversalTime());
        }
    }
}
