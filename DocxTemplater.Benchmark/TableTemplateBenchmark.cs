using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using BenchmarkDotNet.Attributes;

namespace DocxTemplater.Benchmark
{
    public class TableTemplateBenchmark
    {
        private readonly IXPathNavigable _dataset;

        public TableTemplateBenchmark()
        {
            _dataset = CreateDataset(rowsCount: 1000);
            ProcessTemplate();
        }

        private XPathDocument CreateDataset(int rowsCount)
        {
            object[] items = new XElement[rowsCount];
            for (var i = 0; i < rowsCount; i++)
            {
                var item = new XElement("item",
                    new XElement("c1", i, new XAttribute("textColor", "#FF0000")),
                    new XElement("c2", i, new XAttribute("highlightColor", "green")),
                    new XElement("c3", i, new XAttribute("bold", "true")),
                    new XElement("c4", i, new XAttribute("italic", "true")),
                    new XElement("c5", i, new XAttribute("underline", "true")),
                    new XElement("c6", i),
                    new XElement("c7", i),
                    new XElement("c8", i),
                    new XElement("c9", i),
                    new XElement("c10", i));
                items[i] = item;
            }

            using (var ms = new MemoryStream())
            {
                new XElement("items", items).Save(ms);
                ms.Position = 0;
                return new XPathDocument(ms);
            }
        }

        [Benchmark]
        public void ProcessTemplate()
        {
            using (var doc = new WordDocument(@".\templates\table_template.docx"))
            {
                doc.ProcessMainDocumentContentControls(_dataset);
            }
        }
    }
}
