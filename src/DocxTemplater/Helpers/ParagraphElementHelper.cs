using System.Collections.Generic;
using System.Xml.Linq;
using DocxTemplater.Extensions;

namespace DocxTemplater.Helpers
{
    internal static class ParagraphElementHelper
    {
        public static XElement CloneParagraphElement(XElement originParagraphElement)
        {
            return new XElement(OpenXmlElementNames.Paragraph, GetParagraphProperties(originParagraphElement).DeepCopy());
        }

        public static void AddElement(XElement paragraphElement, XElement newElement)
        {
            paragraphElement.Add(newElement);
        }

        public static XElement GetParagraphProperties(XElement paragraph)
        {
            return paragraph.Element(OpenXmlElementNames.ParagraphProperties);
        }

        public static IEnumerable<XElement> GetParagraphElements(XElement element)
        {
            return element.Elements(OpenXmlElementNames.Paragraph);
        }

        public static XElement GetParagraphElement(XElement element)
        {
            return element.Element(OpenXmlElementNames.Paragraph);
        }

        public static string GetText(XElement paragraphElement)
        {
            var runElements = RunElementHelper.GetRunElements(paragraphElement);
            return RunElementHelper.CombineText(runElements);
        }
    }
}
