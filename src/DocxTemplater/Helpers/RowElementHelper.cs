using System.Collections.Generic;
using System.Xml.Linq;
using DocxTemplater.Extensions;

namespace DocxTemplater.Helpers
{
    internal static class RowElementHelper
    {
        public static XElement CloneRowElement(XElement originRow)
        {
            var properties = originRow.Element(OpenXmlElementNames.TableRowProperties);
            return properties != null ? new XElement(OpenXmlElementNames.TableRow, properties.DeepCopy()) : new XElement(OpenXmlElementNames.TableRow);
        }

        public static void AddCellElement(XElement rowElement, XElement cellElement)
        {
            rowElement.Add(cellElement);
        }

        public static IEnumerable<XElement> GetCellElements(XElement rowElement)
        {
            return rowElement.Elements(OpenXmlElementNames.TableCell);
        }
    }
}
