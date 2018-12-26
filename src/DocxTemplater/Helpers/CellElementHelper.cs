using System.Xml.Linq;
using DocxTemplater.Extensions;

namespace DocxTemplater.Helpers
{
    internal static class CellElementHelper
    {
        public static XElement CreateCellElement(XElement originCellElement, DataItem dataItem)
        {
            var originParagraphElement = ParagraphElementHelper.GetParagraphElement(originCellElement);
            var cellParagraphElement = CreateCellParagraph(originParagraphElement, dataItem);
            var cellPropertiesElement = GetCellProperties(originCellElement).DeepCopy();

            var cellElement = new XElement(OpenXmlElementNames.TableCell, cellPropertiesElement, cellParagraphElement);
            return cellElement;
        }

        public static string GetCellValue(XElement cellElement)
        {
            var paragraphElement = ParagraphElementHelper.GetParagraphElement(cellElement);
            return ParagraphElementHelper.GetText(paragraphElement);
        }

        private static XElement CreateCellParagraph(XElement originParagraph, DataItem dataItem)
        {
            var originRun = RunElementHelper.GetRunElement(originParagraph);
            var cellParagraph = ParagraphElementHelper.CloneParagraphElement(originParagraph);
            if (originRun != null)
            {
                var cellRun = RunElementHelper.CreateRunElement(originRun, dataItem);
                ParagraphElementHelper.AddElement(cellParagraph, cellRun);
            }
            return cellParagraph;
        }

        private static XElement GetCellProperties(XElement cellElement)
        {
            return cellElement.Element(OpenXmlElementNames.TableCellProperties);
        }
    }
}
