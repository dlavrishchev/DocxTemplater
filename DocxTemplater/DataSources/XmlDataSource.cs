using System;
using System.IO;
using System.Text;
using System.Xml.XPath;

namespace DocxTemplater.DataSources
{
    public sealed class XmlDataSource : IXPathNavigable
    {
        private readonly XPathDocument _doc;

        private XmlDataSource(XPathDocument doc) => _doc = doc;

        public static XmlDataSource Create(string filePath)
        {
            if(string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            using (var fs = File.OpenRead(filePath))
                return Create(fs);
        }

        public static XmlDataSource Create(Stream stream)
        {
            if(stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (var sr = new StreamReader(stream, Encoding.UTF8))
            {
                return new XmlDataSource(new XPathDocument(sr));
            }
        }

        public XPathNavigator CreateNavigator()
        {
            return _doc.CreateNavigator();
        }
    }
}
