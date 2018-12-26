using System.Xml.Linq;

namespace DocxTemplater.Helpers
{
    internal static class BoldElementHelper
    {
        public static bool IsBoldElementExist(XElement element)
        {
            return GetBoldElement(element) != null;
        }

        private static XElement GetBoldElement(XElement element)
        {
            return element.Element(OpenXmlElementNames.Bold);
        }

        public static XElement CreateBoldElement()
        {
            return new XElement(OpenXmlElementNames.Bold);
        }
    }
}
