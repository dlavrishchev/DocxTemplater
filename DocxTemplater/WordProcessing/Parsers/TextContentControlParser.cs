using System.Xml.Linq;
using DocxTemplater.Extensions;

namespace DocxTemplater.WordProcessing.Parsers
{
    internal class TextContentControlParser : SdtContentParser
    {
        public string Path { get; private set; }

        public TextContentControlParser(XElement sdtContent) : base(sdtContent)
        {
        }

        public override void Parse()
        {
            Path = sdtContent.GetSdtContentText();
        }
    };
}
