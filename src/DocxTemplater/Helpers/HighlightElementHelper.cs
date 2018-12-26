using System.Xml.Linq;

namespace DocxTemplater.Helpers
{
    internal static class HighlightElementHelper
    {
        public static XElement GetHighlightElement(XElement element)
        {
            return element.Element(OpenXmlElementNames.Highlight);
        }

        public static XElement CreateHighlightElement(string color)
        {
            var highlightElement = new XElement(OpenXmlElementNames.Highlight);
            highlightElement.Add(CreateValAttribute(color));
            return highlightElement;
        }

        public static void SetColor(XElement highlightElement, string color)
        {
            var valAttribute = GetValAttribute(highlightElement);

            if (valAttribute != null)
                SetValAttributeValue(valAttribute, color);
            else
                highlightElement.Add(CreateValAttribute(color));
        }

        private static XAttribute CreateValAttribute(string color)
        {
            return new XAttribute(OpenXmlElementNames.Val, color);
        }

        private static XAttribute GetValAttribute(XElement highlightElement)
        {
            return highlightElement.Attribute(OpenXmlElementNames.Val);
        }

        private static void SetValAttributeValue(XAttribute valAttribute, string color)
        {
            valAttribute.Value = color;
        }
    }
}
