using System.Linq;
using System.Xml.Linq;

namespace DocxTemplater.Helpers
{
    internal static class TableElementHelper
    {
        public static XElement GetTableElement(XElement element)
        {
            return element.Element(OpenXmlElementNames.Table);
        }

        public static void AddRow(XElement tableElement, XElement rowElement)
        {
            tableElement.Add(rowElement);
        }

        public static XElement GetRowElement(XElement table, int rowIndex)
        {
            return table.Elements(OpenXmlElementNames.TableRow).ElementAtOrDefault(rowIndex);
        }
    }
}
