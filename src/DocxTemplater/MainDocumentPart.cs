using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DocxTemplater.Extensions;
using DocxTemplater.Helpers;

namespace DocxTemplater
{
    internal sealed class MainDocumentPart : DocxDocumentPart
    {
        public MainDocumentPart(Stream partStream) : base(partStream)
        {
            
        }

        public IEnumerable<XElement> GetSdtElements()
        {
            var body = Content.GetBodyElement();
            var bodySdtElements = SdtElementHelper.GetSdtElements(body);
            var paragraphSdtElements = ParagraphElementHelper.GetParagraphElements(body).SelectMany(SdtElementHelper.GetSdtElements);
            return bodySdtElements.Concat(paragraphSdtElements);
        }
    }
}
