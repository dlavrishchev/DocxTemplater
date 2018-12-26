using Xunit;

namespace DocxTemplater.Tests
{
    public class TextHighlightColorValidatorTests
    {
        private readonly TextHighlightColorValidator _validator;

        public TextHighlightColorValidatorTests()
        {
            _validator = new TextHighlightColorValidator();
        }

        [Theory]
        [InlineData("black", true)]
        [InlineData("blue", true)]
        [InlineData("cyan", true)]
        [InlineData("darkBlue", true)]
        [InlineData("darkCyan", true)]
        [InlineData("darkGray", true)]
        [InlineData("darkGreen", true)]
        [InlineData("darkMagenta", true)]
        [InlineData("darkRed", true)]
        [InlineData("darkYellow", true)]
        [InlineData("green", true)]
        [InlineData("lightGray", true)]
        [InlineData("magenta", true)]
        [InlineData("red", true)]
        [InlineData("white", true)]
        [InlineData("yellow", true)]
        public void IsSupportedColor_SupportedColor_ReturnsTrue(string color, bool expected)
        {
            var result = _validator.IsSupportedColor(color);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsSupportedColor_SupportedColorUpperCase_ReturnsTrue()
        {
            var result = _validator.IsSupportedColor("BLACK");
            Assert.True(result);
        }

        [Fact]
        public void IsSupportedColor_SupportedColorLowerCase_ReturnsTrue()
        {
            var result = _validator.IsSupportedColor("darkblue");
            Assert.True(result);
        }

        [Theory]
        [InlineData("DarkRed", true)]
        [InlineData("Darkred", true)]
        public void IsSupportedColor_SupportedColorMixedCase_ReturnsTrue(string color, bool expected)
        {
            var result = _validator.IsSupportedColor(color);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsSupportedColor_NullString_ReturnsFalse()
        {
            var result = _validator.IsSupportedColor(null);
            Assert.False(result);
        }

        [Fact]
        public void IsSupportedColor_EmptyString_ReturnsFalse()
        {
            var result = _validator.IsSupportedColor("");
            Assert.False(result);
        }

        [Fact]
        public void IsSupportedColor_WhitespaceString_ReturnsFalse()
        {
            var result = _validator.IsSupportedColor("  ");
            Assert.False(result);
        }

        [Fact]
        public void IsSupportedColor_NotSupportedColor_ReturnsFalse()
        {
            var result = _validator.IsSupportedColor("foo");
            Assert.False(result);
        }
    }
}
