using System.Xml.Linq;

namespace DocxTemplater.Extensions
{
    internal static class XAttributeExtensions
    {
        public static XAttribute Clone(this XAttribute attribute)
        {
            return new XAttribute(attribute);
        }
    }
}
