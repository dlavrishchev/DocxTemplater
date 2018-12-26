using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DocxTemplater
{
    internal sealed class ContentTypesPart : DocxDocumentPart
    {
        public IEnumerable<string> HeaderPartNames => GetPartNamesForContentType("application/vnd.openxmlformats-officedocument.wordprocessingml.header+xml");
        public IEnumerable<string> FooterPartNames => GetPartNamesForContentType("application/vnd.openxmlformats-officedocument.wordprocessingml.footer+xml");

        public ContentTypesPart(Stream partStream) : base(partStream)
        {

        }

        private IEnumerable<string> GetPartNamesForContentType(string contentType)
        {
            var ns = Content.Root.Name.Namespace;
            return Content.Root.Elements(ns + "Override").
                Where(e => e.Attribute("ContentType").Value.Equals(contentType, StringComparison.OrdinalIgnoreCase)).
                Select(e =>
                {
                    var attrVal = e.Attribute("PartName").Value;
                    return attrVal[0].Equals('/') ? attrVal.Substring(1) : attrVal;
                });
        }
    }
}
