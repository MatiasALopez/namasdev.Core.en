using System;
using System.Globalization;
using namasdev.Core.Types;
using Xunit;

namespace namasdev.Core.en.Tests.Types
{
    public class StringExtensionsTests
    {
        // Capitalize

        [Fact]
        public void Capitalize_LowercaseWords_ReturnsTitleCase()
        {
            Assert.Equal("Hello World", "hello world".Capitalize(CultureInfo.InvariantCulture));
        }

        // RemoveAccentsAndSpecialCharacters

        [Fact]
        public void RemoveAccentsAndSpecialCharacters_AccentedChars_RemovesAccents()
        {
            Assert.Equal("aeiou", "áéíóú".RemoveAccentsAndSpecialCharacters());
        }

        [Fact]
        public void RemoveAccentsAndSpecialCharacters_NullOrWhitespace_ReturnsEmpty()
        {
            Assert.Equal(string.Empty, "   ".RemoveAccentsAndSpecialCharacters());
            Assert.Equal(string.Empty, ((string)null!).RemoveAccentsAndSpecialCharacters());
        }

        [Fact]
        public void RemoveAccentsAndSpecialCharacters_NoAccents_ReturnsSameValue()
        {
            Assert.Equal("abc", "abc".RemoveAccentsAndSpecialCharacters());
        }

        // RemoveNonAlphanumeric

        [Fact]
        public void RemoveNonAlphanumeric_RemovesSpecialChars()
        {
            Assert.Equal("hello world", "hello, world!".RemoveNonAlphanumeric());
        }

        [Fact]
        public void RemoveNonAlphanumeric_NullOrWhitespace_ReturnsEmpty()
        {
            Assert.Equal(string.Empty, ((string)null!).RemoveNonAlphanumeric());
        }

        // Truncate

        [Fact]
        public void Truncate_LongString_TruncatesToLength()
        {
            Assert.Equal("Hello", "Hello World".Truncate(5));
        }

        [Fact]
        public void Truncate_ShortString_ReturnsOriginal()
        {
            Assert.Equal("Hi", "Hi".Truncate(10));
        }

        [Fact]
        public void Truncate_WithSuffix_AppendsSuffix()
        {
            Assert.Equal("Hello...", "Hello World".Truncate(5, "..."));
        }

        [Fact]
        public void Truncate_EmptyString_ReturnsEmpty()
        {
            Assert.Equal(string.Empty, "".Truncate(5));
        }

        [Fact]
        public void Truncate_NegativeLength_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => "Hello World".Truncate(-1));
        }

        // ValueNotEmptyOrNull

        [Fact]
        public void ValueNotEmptyOrNull_NonEmpty_ReturnsSameValue()
        {
            Assert.Equal("hello", "hello".ValueNotEmptyOrNull());
        }

        [Fact]
        public void ValueNotEmptyOrNull_Whitespace_ReturnsNull()
        {
            Assert.Null("  ".ValueNotEmptyOrNull());
        }

        [Fact]
        public void ValueNotEmptyOrNull_Empty_ReturnsReplacement()
        {
            Assert.Equal("default", "".ValueNotEmptyOrNull("default"));
        }

        // TrimEnd

        [Fact]
        public void TrimEnd_EndingMatches_TrimsIt()
        {
            Assert.Equal("Hello", "Hello World".TrimEnd(" World"));
        }

        [Fact]
        public void TrimEnd_NoMatch_ReturnsOriginal()
        {
            Assert.Equal("Hello", "Hello".TrimEnd("World"));
        }

        // ToFirstLetterLowercase

        [Fact]
        public void ToFirstLetterLowercase_UppercaseFirst_LowersFirst()
        {
            Assert.Equal("helloWorld", "HelloWorld".ToFirstLetterLowercase());
        }

        [Fact]
        public void ToFirstLetterLowercase_AlreadyLower_NoChange()
        {
            Assert.Equal("helloWorld", "helloWorld".ToFirstLetterLowercase());
        }

        [Fact]
        public void ToFirstLetterLowercase_NullOrWhitespace_ReturnsOriginal()
        {
            Assert.Equal("   ", "   ".ToFirstLetterLowercase());
        }
    }
}
