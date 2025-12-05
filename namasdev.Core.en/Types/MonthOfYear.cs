using System;
using System.Collections.Generic;
using System.Globalization;

namespace namasdev.Core.Types
{
    public class MonthOfYear : IComparable<MonthOfYear>
    {
        public MonthOfYear(CultureInfo culture = null)
            : this(DateTime.Today)
        {
            SetCulture(culture);
        }

        public MonthOfYear(short month, short year,
            CultureInfo culture = null)
        {
            ValidateMonthAndYear(month, year);

            Month = month;
            Year = year;

            SetCulture(culture);
        }

        public MonthOfYear(string monthName, short year,
            CultureInfo culture = null)
        {
            Validation.Validator.ValidateRequiredArgumentAndThrow(monthName, nameof(monthName));

            SetCulture(culture);

            try
            {
                Month = (short)(Array.IndexOf(Culture.DateTimeFormat.MonthNames, monthName.ToLower()) + 1);
            }
            catch (Exception)
            {
                throw new FormatException("Invalid month name.");
            }
                
            Year = year;

            ValidateMonthAndYear(Month, Year);
        }

        public MonthOfYear(string yearAndMonth,
            CultureInfo culture = null)
        {
            Validation.Validator.ValidateRequiredArgumentAndThrow(yearAndMonth, nameof(yearAndMonth));

            try 
	        {	      
                Year = short.Parse(yearAndMonth.Substring(0, 4));
                Month = short.Parse(yearAndMonth.Substring(4, 2));

                ValidateMonthAndYear(Month, Year);
	        }
	        catch (Exception)
	        {
		        throw new FormatException("Invalid year and month format (expected: yyyymm).");
	        }

            SetCulture(culture);
        }

        public MonthOfYear(DateTime date,
            CultureInfo culture = null)
        {
            Year = (short)date.Year;
            Month = (short)date.Month;

            SetCulture(culture);
        }

        public short Month { get; set; }
        public short Year { get; set; }
        public CultureInfo Culture { get; set; }

        public DateTime FirstDay
        {
            get { return new DateTime(Year, Month, 1); }
        }

        public DateTime LastDay
        {
            get { return new DateTime(Year, Month, Culture.Calendar.GetDaysInMonth(Year, Month)); }
        }

        public string YearAndMonth
        {
            get { return FormatYearAndMonth(Month, Year); }
        }

        public string YearAndMonthFormatted
        {
            get { return FormatYearAndMonth(Month, Year, useSeparator: true); }
        }

        public string MonthName
        {
            get { return Culture.DateTimeFormat.GetMonthName(Month); }
        }

        public string MonthNameAndYear
        {
            get { return $"{MonthName} {Year}"; }
        }

        private void ValidateMonthAndYear(short month, short year)
        {
            if (month < 1 || month > 12)
            {
                throw new Exception($"Invalid month ({month}).");
            }
            if (year <= 0)
            {
                throw new Exception($"Invalid year ({year}).");
            }
        }

        public MonthOfYear NextMonth()
        {
            return AddMonths(1);
        }

        public MonthOfYear PreviousMonth()
        {
            return AddMonths(-1);
        }

        public MonthOfYear AddMonths(int value)
        {
            return new MonthOfYear(FirstDay.AddMonths(value));
        }

        public DateTime GetDateForWorkingDay(int workingDayNumber)
        {
            DateTime date = FirstDay,
                lastDay = LastDay;

            int workingDay = 1;
            while (date < lastDay && workingDay < workingDayNumber)
            {
                date = date.AddDays(1);

                if (!(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
                {
                    workingDay++;
                }
            }

            return date;
        }

        public IEnumerable<DateTime> GetDates()
        {
            var dates = new List<DateTime>();
            DateTime day = FirstDay;
            while (day <= LastDay)
            {
                dates.Add(day);
                day = day.AddDays(1);
            }
            return dates;
        }

        public DateTime GetDate(int dayNumber)
        {
            return new DateTime(Year, Month, dayNumber);
        }

        public int DiffInMonths(MonthOfYear other)
        {
            bool inverted = this.CompareTo(other) > 0;
            int count = 0;
            MonthOfYear month = this;
            while (!month.Equals(other))
            {
                month = inverted
                    ? month.PreviousMonth()
                    : month.NextMonth();

                count++;
            }
            return inverted ? count * -1 : count;
        }

        public override string ToString()
        {
            return MonthNameAndYear;
        }

        public int CompareTo(MonthOfYear other)
        {
            if (other == null)
            {
                return 1;
            }

            return String.Compare(this.YearAndMonth, other.YearAndMonth);
        }

        public override int GetHashCode()
        {
            return this.YearAndMonth.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otro = obj as MonthOfYear;
            if (otro == null)
            {
                return false;
            }

            return this.GetHashCode().Equals(otro.GetHashCode());
        }

        public static string FormatYearAndMonth(short month, short year,
            bool useSeparator = false)
        {
            return $"{year:D4}{(useSeparator ? "-" : "")}{month:D2}";
        }

        public static IEnumerable<MonthOfYear> GetMonths(MonthOfYear from, MonthOfYear to)
        {
            var months = new List<MonthOfYear>();

            bool inverted = from.CompareTo(to) > 0;
            MonthOfYear month = from;
            if (month.Equals(to))
            {
                months.Add(from);
            }

            while (!month.Equals(to))
            {
                months.Add(month);

                month = inverted
                    ? month.PreviousMonth()
                    : month.NextMonth();
            }

            return months;
        }

        public static IEnumerable<MonthOfYear> GetMonthsForYears(short yearFrom, short yearTo)
        {
            var months = new List<MonthOfYear>();
            for (short year = yearFrom; year <= yearTo; year++)
            {
                for (short month = 1; month <= 12; month++)
                {
                    months.Add(new MonthOfYear { Year = year, Month = month });
                }
            }
            return months;
        }

        private void SetCulture(CultureInfo culture)
        {
            Culture = culture ?? Culture;
        }
    }
}
