using System.Drawing;
using System.Xml.Linq;

namespace DocxTemplater.Helpers
{
    internal static class ColorElementHelper
    {
        public static XElement GetColorElement(XElement element)
        {
            return element.Element(OpenXmlElementNames.Color);
        }

        public static XElement CreateColorElement(Color color)
        {
            var colorElement = new XElement(OpenXmlElementNames.Color);
            colorElement.Add(CreateValAttribute(color));
            return colorElement;
        }

        public static void SetColor(XElement colorElement, Color color)
        {
            var valAttribute = GetValAttribute(colorElement);

            if (valAttribute != null)
                SetValAttributeValue(valAttribute, color);
            else
                colorElement.Add(CreateValAttribute(color));
        }

        private static XAttribute CreateValAttribute(Color color)
        {
            var hexColorString = TextColorConverter.ColorToHexString(color);
            return new XAttribute(OpenXmlElementNames.Val, hexColorString);
        }

        private static XAttribute GetValAttribute(XElement colorElement)
        {
            return colorElement.Attribute(OpenXmlElementNames.Val);
        }

        private static void SetValAttributeValue(XAttribute valAttribute, Color color)
        {
            var hexColorString = TextColorConverter.ColorToHexString(color);
            valAttribute.Value = hexColorString;
        }
    }
}
