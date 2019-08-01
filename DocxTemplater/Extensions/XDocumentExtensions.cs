using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DocxTemplater.WordProcessing;

namespace DocxTemplater.Extensions
{
    internal static class XDocumentExtensions
    {
        public static IEnumerable<XElement> GetSdtElementsFromRoot(this XDocument doc)
        {
            return doc.Root.GetSdtElements();
        }

        public static IEnumerable<XElement> GetSdtElementsFromParagraphs(this XDocument doc)
        {
            return doc.GetParagraphs().SelectMany(p => p.GetSdtElements());
        }

        public static IEnumerable<XElement> GetSdtElementsFromRuns(this XDocument doc)
        {
            return doc.GetRuns().SelectMany(r => r.GetSdtElements());
        }

        public static XElement GetBody(this XDocument doc)
        {
            return doc.Root.Element(WordprocessingElementNames.Body);
        }

        private static IEnumerable<XElement> GetParagraphs(this XDocument doc)
        {
            return doc.Elements(WordprocessingElementNames.Paragraph);
        }

        private static IEnumerable<XElement> GetRuns(this XDocument doc)
        {
            return doc.Elements(WordprocessingElementNames.Run);
        }
    }
}
