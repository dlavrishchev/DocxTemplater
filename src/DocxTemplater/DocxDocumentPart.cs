using System.IO;
using System.Xml.Linq;

namespace DocxTemplater
{
    internal abstract class DocxDocumentPart
    {
        protected XDocument Content { get; }

        protected DocxDocumentPart(Stream partStream)
        {
            Content = XDocument.Load(partStream);
        }

        public void SaveToStream(Stream stream)
        {
            Content.Save(stream, SaveOptions.DisableFormatting);
        }
    }
}
