using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DocxTemplater.Helpers
{
    internal static class SdtElementHelper
    {
        public static string GetTag(XElement sdtElement)
        {
            var props = sdtElement.Element(OpenXmlElementNames.SdtProperties);
            if (props == null)
                return null;
            var tag = props.Element(OpenXmlElementNames.Tag);
            var attr = tag?.Attribute(OpenXmlElementNames.Val);
            return attr?.Value;
        }

        public static string GetText(XElement sdtElement)
        {
            var sdtContentElement = SdtContentElementHelper.GetSdtContentElement(sdtElement);
            return SdtContentElementHelper.GetText(sdtContentElement);
        }

        public static IEnumerable<XElement> GetSdtElements(XElement element)
        {
            return element.Elements(OpenXmlElementNames.Sdt);
        }

        public static XElement GetDescendantSdtElementByTag(XElement element, string tag)
        {
            return element.Descendants(OpenXmlElementNames.Sdt).
                FirstOrDefault(x => GetTag(x).Equals(tag, StringComparison.OrdinalIgnoreCase));
        }
    }
}
