using System.IO;
using System.Text;
using System.Xml.XPath;

namespace DocxTemplater.DataSources
{
    /// <summary>
    /// Default XML data source.
    /// </summary>
    public sealed class XmlDataSource : IXmlDataSource
    {
        private XPathDocument _doc;
        private XPathNavigator _navigator;

        public void Load(string filePath)
        {
            Guard.NotNullOrWhiteSpace(filePath, nameof(filePath));
            using (var fs = File.OpenRead(filePath))
                Load(fs);
        }

        public void Load(Stream stream)
        {
            Guard.NotNull(stream, nameof(stream));
            using (var sr = new StreamReader(stream, Encoding.UTF8))
            {
                _doc = new XPathDocument(sr);
                _navigator = _doc.CreateNavigator();
            }
        }

        public XPathNodeIterator GetNodesIterator(string xpath)
        {
            Guard.NotNullOrWhiteSpace(xpath, nameof(xpath));
            return _navigator.Select(xpath);
        }

        public XPathNavigator GetNodeNavigator(string xpath)
        {
            Guard.NotNullOrWhiteSpace(xpath, nameof(xpath));
            return _navigator.SelectSingleNode(xpath);
        }
    }
}
