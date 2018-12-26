using System;
using System.Drawing;
using Xunit;

namespace DocxTemplater.Tests
{
    public class TextColorConverterTests
    {
        [Theory]
        [InlineData("#FF0000")]
        [InlineData("#ff0000")]
        [InlineData("#Ff0000")]
        [InlineData("FF0000")]
        public void Convert_ValidValueWithoutAlpha_ReturnsColorObject(string actual)
        {
            var converter = new TextColorConverter();
            var result = converter.Convert(actual);
            var expected = Color.FromArgb(255, 0, 0);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("#FFFF0000")]
        [InlineData("#ffff0000")]
        [InlineData("#FFFf0000")]
        [InlineData("FFFF0000")]
        public void Convert_ValidValueWithAlpha_ReturnsColorObject(string actual)
        {
            var converter = new TextColorConverter();
            var result = converter.Convert(actual);
            var expected = Color.FromArgb(255, 255, 0, 0);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Convert_NullString_ReturnsEmptyColor()
        {
            var converter = new TextColorConverter();
            var result = converter.Convert(null);
            var expected = Color.Empty;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Convert_EmptyString_ReturnsEmptyColor()
        {
            var converter = new TextColorConverter();
            var result = converter.Convert("");
            var expected = Color.Empty;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Convert_WhitespaceString_ReturnsEmptyColor()
        {
            var converter = new TextColorConverter();
            var result = converter.Convert(" ");
            var expected = Color.Empty;
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Convert_BadValueFormat_ThrowsArgumentException()
        {
            var converter = new TextColorConverter();
            Assert.Throws<ArgumentException>(() => converter.Convert("foo"));
            Assert.Throws<ArgumentException>(() => converter.Convert("#FF"));
        }

        [Fact]
        public void ColorToHex_ColorWithAlpha_ReturnsHexString()
        {
            var color = Color.FromArgb(255, 255, 0, 0);
            var result = TextColorConverter.ColorToHexString(color);
            var expected = "FF0000";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ColorToHex_ColorWithoutAlpha_ReturnsHexString()
        {
            var color = Color.FromArgb(255, 0, 0);
            var result = TextColorConverter.ColorToHexString(color);
            var expected = "FF0000";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ColorToHex_EmptyColor_ReturnsHexString()
        {
            var color = Color.Empty;
            var result = TextColorConverter.ColorToHexString(color);
            var expected = "000000";
            Assert.Equal(expected, result);
        }
    }
}
