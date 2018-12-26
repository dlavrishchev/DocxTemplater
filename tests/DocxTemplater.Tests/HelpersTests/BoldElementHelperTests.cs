using System.Xml.Linq;
using DocxTemplater.Helpers;
using Xunit;

namespace DocxTemplater.Tests.HelpersTests
{
    public class BoldElementHelperTests
    {
        [Fact]
        public void IsBoldElementExist_ElementExist_ReturnsTrue()
        {
            var xml = @"             
                <w:rPr xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"">
                    <w:b/>
                    <w:lang w:val=""en-US""/>
                </w:rPr>";

            var element = XElement.Parse(xml);
            var result = BoldElementHelper.IsBoldElementExist(element);

            Assert.True(result);
        }

        [Fact]
        public void IsBoldElementExist_ElementNotExist_ReturnsFalse()
        {
            var xml = @"             
                <w:rPr xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"">
                    <w:lang w:val=""en-US""/>
                </w:rPr>";
            var element = XElement.Parse(xml);
            var result = BoldElementHelper.IsBoldElementExist(element);

            Assert.False(result);
        }

        [Fact]
        public void CreateBoldElement()
        {
            var expectedXml = "<b xmlns=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\" />";
            var actualXml = BoldElementHelper.CreateBoldElement().ToString(SaveOptions.DisableFormatting);

            Assert.Equal(expectedXml, actualXml);
        }
    }
}
