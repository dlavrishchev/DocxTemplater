using System.Collections.Generic;
using System.Xml.Linq;
using DocxTemplater.Extensions;

namespace DocxTemplater.Helpers
{
    internal static class RunElementHelper
    {
        public static XElement CreateRunElement(XElement originRunElement, DataItem dataItem)
        {
            var resultRunElement = CloneRunElement(originRunElement);
            var resultRunPropertiesElement = RunPropertiesElementHelper.GetRunPropertiesElement(resultRunElement);

            if (dataItem.Bold)
                RunPropertiesElementHelper.SetTextBold(resultRunPropertiesElement);

            if (dataItem.Italic)
               RunPropertiesElementHelper.SetTextItalic(resultRunPropertiesElement);

            if (dataItem.Underline)
                RunPropertiesElementHelper.SetTextUnderline(resultRunPropertiesElement);

            if (dataItem.IsTextColorAssigned)
                RunPropertiesElementHelper.SetTextColor(resultRunPropertiesElement, dataItem.TextColor);

            if (dataItem.IsTextHighlightColorAssigned)
                RunPropertiesElementHelper.SetTextHighlightColor(resultRunPropertiesElement, dataItem.TextHighlightColor);

            SetText(resultRunElement, dataItem.Value);
            return resultRunElement;
        }

        public static void SetText(XElement runElement, string text)
        {
            var textElement = TextElementHelper.GetTextElement(runElement);
            if (textElement != null)
                TextElementHelper.ChangeText(textElement, text);
            else
                runElement.Add(TextElementHelper.CreateTextElement(text));
        }

        public static XElement GetRunElement(XElement element)
        {
            return element.Element(OpenXmlElementNames.Run);
        }

        public static IEnumerable<XElement> GetRunElements(XElement element)
        {
            return element.Elements(OpenXmlElementNames.Run);
        }

        public static string GetText(XElement runElement)
        {
            var textElement = TextElementHelper.GetTextElement(runElement);
            return textElement == null ? null : TextElementHelper.GetText(textElement);
        }

        public static string CombineText(IEnumerable<XElement> runElements)
        {
            string text = null;
            foreach (var runElement in runElements)
            {
                var runText = GetText(runElement);
                if (!string.IsNullOrEmpty(runText))
                    text += runText;
            }
            return text;
        }

        private static XElement CloneRunElement(XElement originRunElement)
        {
            return new XElement(OpenXmlElementNames.Run, RunPropertiesElementHelper.GetRunPropertiesElement(originRunElement).DeepCopy());
        }
    }
}
