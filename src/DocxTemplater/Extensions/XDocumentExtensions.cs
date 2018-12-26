using System.Collections.Generic;
using System.Xml.Linq;
using DocxTemplater.Helpers;

namespace DocxTemplater.Extensions
{
    internal static class XDocumentExtensions
    {
        public static XElement GetBodyElement(this XDocument d)
        {
            return d.Root.Element(OpenXmlElementNames.Body);
        }

        public static IEnumerable<XElement> GetParagraphElements(this XDocument d)
        {
            return ParagraphElementHelper.GetParagraphElements(d.Root);
        }

        public static IEnumerable<XElement> GetRunElements(this XDocument d)
        {
            return RunElementHelper.GetRunElements(d.Root);
        }
    }
}
