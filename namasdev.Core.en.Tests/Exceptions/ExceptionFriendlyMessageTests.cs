using System;
using namasdev.Core.Exceptions;
using Xunit;

namespace namasdev.Core.en.Tests.Exceptions
{
    public class ExceptionFriendlyMessageTests
    {
        [Fact]
        public void Constructor_MessageOnly_SetsMessage()
        {
            var ex = new ExceptionFriendlyMessage("user-visible message");
            Assert.Equal("user-visible message", ex.Message);
            Assert.Null(ex.InternalMessage);
            Assert.False(ex.MustLogError);
        }

        [Fact]
        public void Constructor_MessageAndInternalMessage_SetsBoth()
        {
            var ex = new ExceptionFriendlyMessage("user message", "internal details");
            Assert.Equal("user message", ex.Message);
            Assert.Equal("internal details", ex.InternalMessage);
            Assert.True(ex.MustLogError);
        }

        [Fact]
        public void Constructor_MessageAndInner_SetsInnerException()
        {
            var inner = new InvalidOperationException("root cause");
            var ex = new ExceptionFriendlyMessage("user message", inner);
            Assert.Equal(inner, ex.InnerException);
            Assert.False(ex.MustLogError);
        }

        [Fact]
        public void Constructor_AllParams_SetsAll()
        {
            var inner = new Exception("root");
            var ex = new ExceptionFriendlyMessage("user msg", "internal msg", inner);
            Assert.Equal("user msg", ex.Message);
            Assert.Equal("internal msg", ex.InternalMessage);
            Assert.Equal(inner, ex.InnerException);
            Assert.True(ex.MustLogError);
        }

        [Fact]
        public void MustLogError_EmptyInternalMessage_ReturnsFalse()
        {
            var ex = new ExceptionFriendlyMessage("msg", "");
            Assert.False(ex.MustLogError);
        }

        [Fact]
        public void MustLogError_WhitespaceInternalMessage_ReturnsFalse()
        {
            var ex = new ExceptionFriendlyMessage("msg", "   ");
            Assert.False(ex.MustLogError);
        }
    }
}
