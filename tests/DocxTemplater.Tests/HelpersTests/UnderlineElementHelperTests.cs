using System.Xml.Linq;
using DocxTemplater.Helpers;
using Xunit;

namespace DocxTemplater.Tests.HelpersTests
{
    public class UnderlineElementHelperTests
    {
        [Fact]
        public void IsUnderlineElementExist_ElementExist_ReturnsTrue()
        {
            var xml = @"             
                <w:rPr xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"">
                    <w:u w:val=""single""/>
                    <w:lang w:val=""en-US""/>
                </w:rPr>";

            var element = XElement.Parse(xml);
            var result = UnderlineElementHelper.IsUnderlineElementExist(element);

            Assert.True(result);
        }

        [Fact]
        public void IsUnderlineElementExist_ElementNotExist_ReturnsFalse()
        {
            var xml = @"             
                <w:rPr xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"">
                    <w:lang w:val=""en-US""/>
                </w:rPr>";
            var element = XElement.Parse(xml);
            var result = UnderlineElementHelper.IsUnderlineElementExist(element);

            Assert.False(result);
        }

        [Fact]
        public void CreateUnderlineElement()
        {
            var expectedXml = @"<u p1:val=""single"" xmlns:p1=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"" xmlns=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"" />";
            var actualXml = UnderlineElementHelper.CreateUnderlineElement().ToString(SaveOptions.DisableFormatting);

            Assert.Equal(expectedXml, actualXml);
        }
    }
}
