using System.Xml.Linq;

namespace DocxTemplater.Extensions
{
    internal static class XElementExtensions
    {
        public static XElement DeepCopy(this XElement e)
        {
            return new XElement(e);
        }
    }
}
