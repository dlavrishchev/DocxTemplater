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
    public class TableContentControlTests
    {
        private XmlDataSource _xmlDataSource;

        public TableContentControlTests()
        {
            CreateXmlDataSource();
        }

        private void CreateXmlDataSource()
        {
            _xmlDataSource = new XmlDataSource();
            _xmlDataSource.Load("./files/TableContentControl/XmlDataSource.xml");
        }

        [Fact]
        public void Replace_ValidContentControl()
        {
            XElement rootElement;

            using (var s = File.OpenRead("./files/TableContentControl/ValidXml.xml"))
            {
                rootElement = XElement.Load(s);
                var sdtElement = SdtElementHelper.GetSdtElements(rootElement).First();
                var contentControl = new TableContentControl(sdtElement);
                contentControl.Replace(_xmlDataSource);
            }

            var actualXml = rootElement.ToString();
            var expectedXml = File.ReadAllText("./files/TableContentControl/ExpectedXml.xml", Encoding.UTF8);
            Assert.Equal(expectedXml, actualXml);
        }

        [Fact]
        public void Replace_MissedDataPathValue_ThrowsTemplateFormatException()
        {
            using (var s = File.OpenRead("./files/TableContentControl/MissedDataPathValue.xml"))
            {
                var sdtElement = SdtElementHelper.GetSdtElements(XElement.Load(s)).First();
                var contentControl = new TableContentControl(sdtElement);
                Assert.Throws<TemplateException>(() => contentControl.Replace(_xmlDataSource));
            }
        }

        [Fact]
        public void Replace_MissedDataPathContentControl_ThrowsTemplateFormatException()
        {
            using (var s = File.OpenRead("./files/TableContentControl/MissedDataPathContentControl.xml"))
            {
                var sdtElement = SdtElementHelper.GetSdtElements(XElement.Load(s)).First();
                var contentControl = new TableContentControl(sdtElement);
                Assert.Throws<TemplateException>(() => contentControl.Replace(_xmlDataSource));
            }
        }
    }
}
