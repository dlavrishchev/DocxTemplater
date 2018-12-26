using System.Xml.Linq;

namespace DocxTemplater.Helpers
{
    internal static class ItalicElementHelper
    {
        public static bool IsItalicElementExist(XElement element)
        {
            return GetItalicElement(element) != null;
        }

        private static XElement GetItalicElement(XElement element)
        {
            return element.Element(OpenXmlElementNames.Italic);
        }

        public static XElement CreateItalicElement()
        {
            return new XElement(OpenXmlElementNames.Italic);
        }
    }
}
