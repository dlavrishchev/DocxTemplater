using System.Drawing;
using System.Xml.Linq;

namespace DocxTemplater.Helpers
{
    internal static class RunPropertiesElementHelper
    {
        public static XElement GetRunPropertiesElement(XElement runElement)
        {
            return runElement.Element(OpenXmlElementNames.RunProperties);
        }

        public static void SetTextBold(XElement runPropertiesElement)
        {
            if (!BoldElementHelper.IsBoldElementExist(runPropertiesElement))
                runPropertiesElement.Add(BoldElementHelper.CreateBoldElement());
        }

        public static void SetTextItalic(XElement runPropertiesElement)
        {
            if (!ItalicElementHelper.IsItalicElementExist(runPropertiesElement))
                runPropertiesElement.Add(ItalicElementHelper.CreateItalicElement());
        }

        public static void SetTextUnderline(XElement runPropertiesElement)
        {
            if (!UnderlineElementHelper.IsUnderlineElementExist(runPropertiesElement))
                runPropertiesElement.Add(UnderlineElementHelper.CreateUnderlineElement());
        }

        public static void SetTextColor(XElement runPropertiesElement, Color color)
        {
            var colorElement = ColorElementHelper.GetColorElement(runPropertiesElement);
            if (colorElement == null)
                runPropertiesElement.Add(ColorElementHelper.CreateColorElement(color));
            else
                ColorElementHelper.SetColor(colorElement, color);
        }

        public static void SetTextHighlightColor(XElement runPropertiesElement, string color)
        {
            var highlightElement = HighlightElementHelper.GetHighlightElement(runPropertiesElement);
            if (highlightElement == null)
                runPropertiesElement.Add(HighlightElementHelper.CreateHighlightElement(color));
            else
                HighlightElementHelper.SetColor(highlightElement, color);
        }
    }
}
