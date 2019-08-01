using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using DocxTemplater.Exceptions;
using DocxTemplater.WordProcessing;
using Xunit;

namespace DocxTemplater.Tests.ContentControlProcessors
{
    public class TableContentControlProcessorTests
    {
        private IXPathNavigable _dataset;

        public TableContentControlProcessorTests()
        {
            CreateDataset();
        }

        private void CreateDataset()
        {
            _dataset = new XPathDocument("./files/TableContentControlProcessor/dataset.xml");
        }

        [Fact]
        public void ProcessingTest()
        {
            XElement root;
            XElement sdt;

            using (var s = File.OpenRead("./files/TableContentControlProcessor/source.xml"))
            {
                root = XElement.Load(s);
                sdt = root.Elements(WordprocessingElementNames.Sdt).First();
            }

            var processor = new WordProcessing.Processors.TableContentControlProcessor(sdt);
            var result = processor.Process(_dataset);

            sdt.AddAfterSelf(result);
            sdt.Remove();
            
            var actualXml = root.ToString();
            var expectedXml = File.ReadAllText("./files/TableContentControlProcessor/expectedResult.xml", Encoding.UTF8);
            Assert.Equal(expectedXml, actualXml);
        }

        [Fact]
        public void ProcessingWithMissedDataPathValueShouldThrowsDocumentProcessingException()
        {
            using (var s = File.OpenRead("./files/TableContentControlProcessor/MissedDataPathValue.xml"))
            {
                var root = XElement.Load(s);
                var sdt = root.Elements(WordprocessingElementNames.Sdt).First();
                var processor = new WordProcessing.Processors.TableContentControlProcessor(sdt);
                Assert.Throws<DocumentProcessingException>(() => processor.Process(_dataset));
            }
        }

        [Fact]
        public void ProcessingWithMissedDataPathContentControlShouldThrowsDocumentProcessingException()
        {
            using (var s = File.OpenRead("./files/TableContentControlProcessor/MissedDataPathContentControl.xml"))
            {
                var root = XElement.Load(s);
                var sdt = root.Elements(WordprocessingElementNames.Sdt).First();
                var contentControl = new WordProcessing.Processors.TableContentControlProcessor(sdt);
                Assert.Throws<DocumentProcessingException>(() => contentControl.Process(_dataset));
            }
        }
    }
}
