using System.Xml.Linq;

namespace DocxTemplater.Helpers
{
    internal static class SdtContentElementHelper
    {
        public static XElement GetSdtContentElement(XElement sdtElement)
        {
            return sdtElement.Element(OpenXmlElementNames.SdtContent);
        }

        public static string GetText(XElement sdtContentElement)
        {
            var paragraph = ParagraphElementHelper.GetParagraphElement(sdtContentElement);
            if (paragraph != null)
                return ParagraphElementHelper.GetText(paragraph);

            var runElements = RunElementHelper.GetRunElements(sdtContentElement);
            return RunElementHelper.CombineText(runElements);
        }
    }
}
