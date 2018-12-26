using System.Xml.Linq;
using DocxTemplater.Helpers;
using Xunit;

namespace DocxTemplater.Tests.HelpersTests
{
    public class ItalicElementHelperTests
    {
        [Fact]
        public void IsItalicElementExist_ElementExist_ReturnsTrue()
        {
            var xml = @"             
                <w:rPr xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"">
                    <w:i/>
                    <w:lang w:val=""en-US""/>
                </w:rPr>";

            var element = XElement.Parse(xml);
            var result = ItalicElementHelper.IsItalicElementExist(element);

            Assert.True(result);
        }

        [Fact]
        public void IsItalicElementExist_ElementNotExist_ReturnsFalse()
        {
            var xml = @"             
                <w:rPr xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"">
                    <w:lang w:val=""en-US""/>
                </w:rPr>";
            var element = XElement.Parse(xml);
            var result = ItalicElementHelper.IsItalicElementExist(element);

            Assert.False(result);
        }

        [Fact]
        public void CreateItalicElement()
        {
            var expectedXml = "<i xmlns=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\" />";
            var actualXml = ItalicElementHelper.CreateItalicElement().ToString(SaveOptions.DisableFormatting);

            Assert.Equal(expectedXml, actualXml);
        }
    }
}
