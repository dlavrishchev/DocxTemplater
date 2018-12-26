using System.Xml.Linq;

namespace DocxTemplater.Helpers
{
    internal static class UnderlineElementHelper
    {
        public static bool IsUnderlineElementExist(XElement element)
        {
            return GetUnderlineElement(element) != null;
        }

        private static XElement GetUnderlineElement(XElement element)
        {
            return element.Element(OpenXmlElementNames.Underline);
        }

        public static XElement CreateUnderlineElement()
        {
            return new XElement(OpenXmlElementNames.Underline, new XAttribute(OpenXmlElementNames.Val, "single"));
        }
    }
}
