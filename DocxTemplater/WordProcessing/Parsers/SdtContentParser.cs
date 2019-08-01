using System.Xml.Linq;

namespace DocxTemplater.WordProcessing.Parsers
{
    internal abstract class SdtContentParser
    {
        protected XElement sdtContent;

        protected SdtContentParser(XElement sdtContent)
        {
            this.sdtContent = sdtContent;
        }

        public abstract void Parse();
    }
}
