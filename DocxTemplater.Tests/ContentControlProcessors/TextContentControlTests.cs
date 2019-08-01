using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using DocxTemplater.DataSources;
using DocxTemplater.Exceptions;
using DocxTemplater.WordProcessing;
using Xunit;

namespace DocxTemplater.Tests.ContentControlProcessors
{
    public class TextContentControlTests
    {
        private IXPathNavigable _dataset;

        public TextContentControlTests()
        {
            CreateDataset();
        }

        private void CreateDataset()
        {
            _dataset = new XPathDocument("./files/TextContentControlProcessor/dataset.xml");
        }

        [Fact]
        public void ProcessingTest()
        {
            XElement root;
            XElement sdt;

            using (var s = File.OpenRead("./files/TextContentControlProcessor/source.xml"))
            {
                root = XElement.Load(s);
                sdt = root.Elements(WordprocessingElementNames.Sdt).First();
            }

            var processor = new WordProcessing.Processors.TextContentControlProcessor(sdt);
            var result = processor.Process(_dataset);

            sdt.AddAfterSelf(result);
            sdt.Remove();

            var actualXml = root.ToString();
            var expectedXml = File.ReadAllText("./files/TextContentControlProcessor/expectedResult.xml", Encoding.UTF8);
            Assert.Equal(expectedXml, actualXml);
        }

        [Fact]
        public void ProcessingWithMissedDataPathValueShouldThrowsDocumentProcessingException()
        {
            using (var s = File.OpenRead("./files/TextContentControlProcessor/missedDataPath.xml"))
            {
                var root = XElement.Load(s);
                var sdt = root.Elements(WordprocessingElementNames.Sdt).First();
                var processor = new WordProcessing.Processors.TextContentControlProcessor(sdt);
                Assert.Throws<DocumentProcessingException>(() => processor.Process(_dataset));
            }
        }
    }
}
