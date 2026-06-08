using System;
using namasdev.Core.Types;
using Xunit;

namespace namasdev.Core.en.Tests.Types
{
    public class ConverterTests
    {
        // ConvertToGuidList

        [Fact]
        public void ConvertToGuidList_ValidInput_ReturnsArray()
        {
            var g1 = Guid.NewGuid();
            var g2 = Guid.NewGuid();
            var result = Converter.ConvertToGuidList($"{g1},{g2}");
            Assert.Equal(2, result.Length);
            Assert.Equal(g1, result[0]);
            Assert.Equal(g2, result[1]);
        }

        [Fact]
        public void ConvertToGuidList_NullOrWhitespace_ReturnsNull()
        {
            Assert.Null(Converter.ConvertToGuidList(null));
            Assert.Null(Converter.ConvertToGuidList("  "));
        }

        [Fact]
        public void ConvertToGuidList_CustomSeparator_Splits()
        {
            var g1 = Guid.NewGuid();
            var g2 = Guid.NewGuid();
            var result = Converter.ConvertToGuidList($"{g1}|{g2}", "|");
            Assert.Equal(2, result.Length);
        }

        // ConvertToShortList

        [Fact]
        public void ConvertToShortList_ValidInput_ReturnsArray()
        {
            var result = Converter.ConvertToShortList("1,2,3");
            Assert.Equal(new short[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void ConvertToShortList_NullOrWhitespace_ReturnsNull()
        {
            Assert.Null(Converter.ConvertToShortList(null));
        }

        // ConvertToStringList

        [Fact]
        public void ConvertToStringList_ValidInput_ReturnsArray()
        {
            var result = Converter.ConvertToStringList("a,b,c");
            Assert.Equal(new[] { "a", "b", "c" }, result);
        }

        [Fact]
        public void ConvertToStringList_EmptyEntries_SkipsThem()
        {
            var result = Converter.ConvertToStringList("a,,b");
            Assert.Equal(new[] { "a", "b" }, result);
        }

        [Fact]
        public void ConvertToStringList_NullOrWhitespace_ReturnsNull()
        {
            Assert.Null(Converter.ConvertToStringList(null));
            Assert.Null(Converter.ConvertToStringList("  "));
        }
    }
}
