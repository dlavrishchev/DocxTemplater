using System.Xml.Linq;
using System.Xml.XPath;

namespace DocxTemplater.WordProcessing.Processors
{
    internal abstract class ContentControlProcessor
    {
        protected XElement sdtContent;

        protected ContentControlProcessor(XElement sdt)
        {
            sdtContent = sdt.Element(WordprocessingElementNames.SdtContent);
        }

        public abstract XElement Process(IXPathNavigable dataSource);
    }
}
