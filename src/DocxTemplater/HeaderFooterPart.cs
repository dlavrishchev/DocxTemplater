using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DocxTemplater.Extensions;
using DocxTemplater.Helpers;

namespace DocxTemplater
{
    internal sealed class HeaderFooterPart : DocxDocumentPart
    {
        public string PartName { get; set; }

        public HeaderFooterPart(Stream partStream) : base(partStream)
        {

        }

        public IEnumerable<XElement> GetSdtElements()
        {
            var rootSdtElements = SdtElementHelper.GetSdtElements(Content.Root);
            var paragraphSdtElements = Content.GetParagraphElements().SelectMany(SdtElementHelper.GetSdtElements);
            var runSdtElements = Content.GetRunElements().SelectMany(SdtElementHelper.GetSdtElements);
            return rootSdtElements.Concat(paragraphSdtElements).Concat(runSdtElements);
        }
    }
}
