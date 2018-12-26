using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DocxTemplater.ContentControls;
using DocxTemplater.DataSources;
using DocxTemplater.Exceptions;
using DocxTemplater.Helpers;
using Xunit;

namespace DocxTemplater.Tests.ContentControlsTests
{
    public class TextContentControlTests
    {
        private XmlDataSource _xmlDataSource;

        public TextContentControlTests()
        {
            CreateXmlDataSource();
        }

        private void CreateXmlDataSource()
        {
            _xmlDataSource = new XmlDataSource();
            _xmlDataSource.Load("./files/TextContentControl/XmlDataSource.xml");
        }

        [Fact]
        public void Replace_ValidContentControl()
        {
            XElement rootElement;

            using (var s = File.OpenRead("./files/TextContentControl/ValidXml.xml"))
            {
                rootElement = XElement.Load(s);
                var sdtElement = SdtElementHelper.GetSdtElements(rootElement).First();
                var contentControl = new TextContentControl(sdtElement);
                contentControl.Replace(_xmlDataSource);
            }

            var actualXml = rootElement.ToString();
            var expectedXml = File.ReadAllText("./files/TextContentControl/ExpectedXml.xml", Encoding.UTF8);
            Assert.Equal(expectedXml, actualXml);
        }

        [Fact]
        public void Replace_MissedDataPath_ThrowsTemplateFormatException()
        {
            using (var s = File.OpenRead("./files/TextContentControl/MissedDataPath.xml"))
            {
                var sdtElement = SdtElementHelper.GetSdtElements(XElement.Load(s)).First();
                var contentControl = new TextContentControl(sdtElement);
                Assert.Throws<TemplateException>(() => contentControl.Replace(_xmlDataSource));
            }
        }
    }
}
