using System.Xml.Linq;

namespace DocxTemplater.Helpers
{
    internal static class TextElementHelper
    {
        public static XElement CreateTextElement(string text)
        {
            return new XElement(OpenXmlElementNames.Text, text);
        }

        public static void ChangeText(XElement textElement, string text)
        {
            textElement.Value = text;
        }

        public static XElement GetTextElement(XElement element)
        {
            return element.Element(OpenXmlElementNames.Text);
        }

        public static string GetText(XElement textElement)
        {
            return textElement.Value;
        }
    }
}
