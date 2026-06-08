using namasdev.Core;
using Xunit;

namespace namasdev.Core.en.Tests
{
    public class MathHelperTests
    {
        // Max

        [Fact]
        public void Max_Decimal_ReturnsLargest()
        {
            Assert.Equal(10m, MathHelper.Max(1m, 10m, 5m));
        }

        [Fact]
        public void Max_Decimal_WithNulls_ReturnsLargestNonNull()
        {
            Assert.Equal(5m, MathHelper.Max(null, 5m, null));
        }

        [Fact]
        public void Max_Decimal_EmptyParams_ReturnsNull()
        {
            Assert.Null(MathHelper.Max(new decimal?[0]));
        }

        [Fact]
        public void Max_Decimal_AllNulls_ReturnsNull()
        {
            Assert.Null(MathHelper.Max((decimal?)null, (decimal?)null));
        }

        [Fact]
        public void Min_Decimal_ReturnsSmallest()
        {
            Assert.Equal(1m, MathHelper.Min(1m, 10m, 5m));
        }

        [Fact]
        public void Min_Decimal_EmptyParams_ReturnsNull()
        {
            Assert.Null(MathHelper.Min(new decimal?[0]));
        }

        // Sum

        [Fact]
        public void Sum_Double_ReturnsCorrectSum()
        {
            Assert.Equal(6.0, MathHelper.Sum(new double[] { 1.0, 2.0, 3.0 }));
        }

        [Fact]
        public void Sum_Double_WithRounding_RoundsResult()
        {
            Assert.Equal(6.67, MathHelper.Sum(new double[] { 2.222, 4.444 }, roundingDigits: 2));
        }

        [Fact]
        public void Sum_NullableDecimal_AllNulls_ReturnsZero()
        {
            // LINQ Sum ignores nulls; all-null yields 0, not null
            Assert.Equal(0m, MathHelper.Sum(new decimal?[] { null, null }));
        }

        [Fact]
        public void Sum_NullableDecimal_MixedValues_SumsNonNull()
        {
            Assert.Equal(3m, MathHelper.Sum(new decimal?[] { 1m, 2m, null }));
        }

        // Average

        [Fact]
        public void Average_Double_ReturnsCorrectAverage()
        {
            Assert.Equal(2.0, MathHelper.Average(new double[] { 1.0, 2.0, 3.0 }));
        }

        [Fact]
        public void Average_Decimal_SumAndCount_ZeroCount_ReturnsZero()
        {
            Assert.Equal(0m, MathHelper.Average(10m, 0));
        }

        [Fact]
        public void Average_Decimal_SumAndCount_ReturnsCorrect()
        {
            Assert.Equal(5m, MathHelper.Average(10m, 2));
        }

        [Fact]
        public void Average_NullableSumAndCount_BothNull_ReturnsNull()
        {
            Assert.Null(MathHelper.Average((decimal?)null, (int?)null));
        }

        // Percentage

        [Fact]
        public void Percentage_ReturnsCorrectRatio()
        {
            Assert.Equal(0.5m, MathHelper.Percentage(1, 2));
        }

        [Fact]
        public void Percentage_ZeroTotal_ReturnsZero()
        {
            Assert.Equal(0m, MathHelper.Percentage(5, 0));
        }

        [Fact]
        public void Percentage_WithRounding_RoundsResult()
        {
            Assert.Equal(0.33m, MathHelper.Percentage(1, 3, roundingDigits: 2));
        }

        [Fact]
        public void Percentage_NullCount_ReturnsNull()
        {
            Assert.Null(MathHelper.Percentage((int?)null, 10));
        }

        [Fact]
        public void PercentageIf_ConditionFalse_ReturnsNull()
        {
            Assert.Null(MathHelper.PercentageIf(false, 5, 10));
        }

        [Fact]
        public void PercentageIf_ConditionTrue_ReturnsPercentage()
        {
            Assert.Equal(0.5m, MathHelper.PercentageIf(true, 5, 10));
        }

        // Count

        [Fact]
        public void CountNotNull_Decimal_CountsNonNullValues()
        {
            Assert.Equal(2, MathHelper.CountNotNull(1m, null, 3m));
        }

        [Fact]
        public void CountNotNull_Double_CountsNonNullValues()
        {
            Assert.Equal(1, MathHelper.CountNotNull((double?)null, 2.0));
        }

        [Fact]
        public void CountTrue_CountsTrueValues()
        {
            Assert.Equal(2, MathHelper.CountTrue(true, false, true));
        }

        [Fact]
        public void CountTrue_NoTrues_ReturnsZero()
        {
            Assert.Equal(0, MathHelper.CountTrue(false, false));
        }
    }
}
