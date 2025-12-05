using System;
using System.Collections.Generic;
using System.Globalization;

namespace namasdev.Core.Types
{
    public class QuarterOfYear : IComparable<QuarterOfYear>
    {
        public QuarterOfYear(CultureInfo culture = null)
            : this(DateTime.Now, culture)
        {
        }

        public QuarterOfYear(int quarter, int year,
            CultureInfo culture = null)
            : this((short)quarter, (short)year)
        {
        }

        public QuarterOfYear(short quarter, short year,
            CultureInfo culture = null)
        {
            Validate(quarter, year);

            Quarter = quarter;
            Year = year;

            SetCulture(culture);
        }

        public QuarterOfYear(string yearAndQuarter,
            CultureInfo culture = null)
        {
            Validation.Validator.ValidateRequiredArgumentAndThrow(yearAndQuarter, nameof(yearAndQuarter));

            try
            {
                Year = short.Parse(yearAndQuarter.Substring(0, 4));
                Quarter = short.Parse(yearAndQuarter.Substring(4, 1));

                Validate(Quarter, Year);
            }
            catch (Exception)
            {
                throw new FormatException("Invalid quarter and year format (yyyyq).");
            }

            SetCulture(culture);
        }

        public QuarterOfYear(DateTime date,
            CultureInfo culture = null)
        {
            Year = (short)date.Year;
            Quarter = Convert.ToInt16(Math.Ceiling(date.Month / 3m));

            SetCulture(culture);
        }

        public short Quarter { get; set; }
        public short Year { get; set; }
        public CultureInfo Culture { get; set; }

        public DateTime FirstDay
        {
            get { return new DateTime(Year, ((Quarter - 1) * 3) + 1, 1); }
        }

        public DateTime LastDay
        {
            get
            {
                int month = Quarter * 3;
                return new DateTime(Year, month, Culture.Calendar.GetDaysInMonth(Year, month));
            }
        }

        public string YearAndQuarter
        {
            get { return FormatYearAndQuarter(Quarter, Year); }
        }

        public string QuarterLongName
        {
            get
            {
                string sufix;
                switch (Quarter)
                {
                    case 1:
                        sufix = "st";
                        break;

                    case 2:
                        sufix = "nd";
                        break;

                    case 3:
                        sufix = "rd";
                        break;

                    case 4:
                        sufix = "th";
                        break;

                    default:
                        throw new Exception($"Invalid quarter ({Quarter}).");
                }

                return $"{Quarter}{sufix} quarter";
            }
        }

        public string QuarterShortName
        {
            get { return $"Q{Quarter}"; }
        }


        public string QuarterLongNameAndYear
        {
            get { return $"{QuarterLongName} of {Year}"; }
        }

        public string QuarterShortNameAndYear
        {
            get { return $"{QuarterShortName} {Year}"; }
        }

        private void Validate(short quarter, short year)
        {
            if (quarter < 1 || quarter > 4)
            {
                throw new Exception($"Invalid quarter ({quarter}).");
            }
            if (year <= 0)
            {
                throw new Exception($"Invalid year ({year}).");
            }
        }

        public QuarterOfYear NextQuarter()
        {
            return AddQuarters(1);
        }

        public QuarterOfYear PreviousQuarter()
        {
            return AddQuarters(-1);
        }

        public QuarterOfYear AddQuarters(int value)
        {
            return new QuarterOfYear(FirstDay.AddMonths(value * 3));
        }

        public IEnumerable<MonthOfYear> GetMonthsInQuarter()
        {
            var firstMonth = new MonthOfYear(FirstDay);
            return new MonthOfYear[] 
            {
                firstMonth,
                firstMonth.NextMonth(),
                new MonthOfYear(LastDay)
            };
        }

        public MonthOfYear GetMonthInQuarter(int number)
        {
            return new MonthOfYear(FirstDay).AddMonths(number - 1);
        }

        public override string ToString()
        {
            return QuarterLongNameAndYear;
        }

        public int CompareTo(QuarterOfYear other)
        {
            if (other == null)
            {
                return 1;
            }

            return String.Compare(this.YearAndQuarter, other.YearAndQuarter);
        }

        public override int GetHashCode()
        {
            return this.YearAndQuarter.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otro = obj as QuarterOfYear;
            if (otro == null)
            {
                return false;
            }

            return this.GetHashCode().Equals(otro.GetHashCode());
        }

        public static string FormatYearAndQuarter(short quarter, short year)
        {
            return $"{year:D4}{quarter:D1}";
        }

        public static string FormatYearAndQuarter(int quarter, int year)
        {
            return FormatYearAndQuarter((short)quarter, (short)year);
        }

        public static string FormatQuarterAndYear(int quarter, int year)
        {
            return FormatYearAndQuarter((short)quarter, (short)year);
        }

        private void SetCulture(CultureInfo culture)
        {
            Culture = culture ?? Culture;
        }
    }
}
