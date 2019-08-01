using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DocxTemplater.Exceptions;
using DocxTemplater.WordProcessing;

namespace DocxTemplater.Extensions
{
    internal static class XElementExtensions
    {
        #region Paragraph

        public static XElement GetParagraph(this XElement element)
        {
            return element.Element(WordprocessingElementNames.Paragraph);
        }

        public static IEnumerable<XElement> GetParagraphs(this XElement element)
        {
            return element.Elements(WordprocessingElementNames.Paragraph);
        }

        public static XElement CreateParagraph()
        {
            return new XElement(WordprocessingElementNames.Paragraph);
        }

        private static XElement GetParagraphProperties(this XElement paragraph)
        {
            return paragraph.Element(WordprocessingElementNames.ParagraphProperties);
        }

        public static XElement CreateParagraphWithEmptyRun(this XElement sourceParagraph)
        {
            var resultParagraph = CreateParagraph();

            var sourceProperties = sourceParagraph.GetParagraphProperties();
            if (sourceProperties != null)
                resultParagraph.Add(sourceProperties.Clone());

            var sourceRun = sourceParagraph.GetRun();
            var resultRun = sourceRun != null ? sourceRun.CreateRunWithoutText() : CreateRun();

            resultParagraph.Add(resultRun);
            return resultParagraph;
        }

        public static string GetParagraphText(this XElement paragraph)
        {
            var runs = paragraph.GetRuns();
            return CombineTextFromRuns(runs);
        }

        #endregion

        #region Run

        public static XElement GetRun(this XElement element)
        {
            return element.Element(WordprocessingElementNames.Run);
        }

        public static XElement CreateRun()
        {
            return new XElement(WordprocessingElementNames.Run);
        }

        public static IEnumerable<XElement> GetRuns(this XElement element)
        {
            return element.Elements(WordprocessingElementNames.Run);
        }

        public static XElement GetRunProperties(this XElement run)
        {
            return run.Element(WordprocessingElementNames.RunProperties);
        }

        public static XElement CreateRunWithoutText(this XElement sourceRun)
        {
            var resultRun = CreateRun();
            var sourceProperties = sourceRun.GetRunProperties();
            if (sourceProperties != null)
                resultRun.Add(sourceProperties.Clone());
            return resultRun;
        }

        public static string GetRunText(this XElement run)
        {
            var text = run.GetTextElement();
            return text?.Value;
        }

        public static void SetRunText(this XElement run, string text)
        {
            var textElement = run.GetTextElement();
            if (textElement == null)
            {
                textElement = CreateTextElement();
                run.Add(textElement);
            }

            textElement.Value = text;
        }

        private static string CombineTextFromRuns(IEnumerable<XElement> runs)
        {
            string text = null;
            foreach (var run in runs)
            {
                var runText = run.GetRunText();
                if (!string.IsNullOrEmpty(runText))
                    text += runText;
            }
            return text;
        }

        #endregion

        #region Sdt

        public static string GetSdtTagValue(this XElement sdt)
        {
            var props = sdt.GetSdtProperties();
            if (props == null)
                return null;
            var tag = props.GetTagElement();
            var attr = tag?.GetAttribute(WordprocessingElementNames.Val.ToString());
            return attr?.Value;
        }

        public static string GetSdtContentText(this XElement sdtContent)
        {
            var paragraph = sdtContent.GetParagraph();
            if (paragraph != null)
                return paragraph.GetParagraphText();

            var runs = sdtContent.GetRuns();
            return CombineTextFromRuns(runs);
        }

        public static XElement GetSdtContent(this XElement sdt)
        {
            return sdt.Element(WordprocessingElementNames.SdtContent);
        }

        public static IEnumerable<XElement> GetSdtElements(this XElement element)
        {
            return element.Elements(WordprocessingElementNames.Sdt);
        }

        public static XElement GetSdtByTag(this XElement sdtContent, string tag)
        {
            return sdtContent.Descendants(WordprocessingElementNames.Sdt).
                FirstOrDefault(x => x.GetSdtTagValue().Equals(tag, StringComparison.OrdinalIgnoreCase));
        }

        private static XElement GetSdtProperties(this XElement sdt)
        {
            return sdt.Element(WordprocessingElementNames.SdtProperties);
        }

        #endregion

        #region Table

        public static XElement GetTable(this XElement element)
        {
            return element.Element(WordprocessingElementNames.Table);
        }

        private static XElement GetTableProperties(this XElement table)
        {
            return table.Element(WordprocessingElementNames.TableProperties);
        }

        private static XElement GetTableGrid(this XElement table)
        {
            return table.Element(WordprocessingElementNames.TableGrid);
        }

        public static XElement CreateTableWithoutRows(this XElement sourceTable)
        {
            var resultTable = CreateTable();
            resultTable.Add(sourceTable.GetTableProperties().Clone());
            resultTable.Add(sourceTable.GetTableGrid().Clone());
            return resultTable;
        }

        private static XElement CreateTable()
        {
            return new XElement(WordprocessingElementNames.Table);
        }

        private static XElement CreateTableRow()
        {
            return new XElement(WordprocessingElementNames.TableRow);
        }

        private static XElement CreateTableCell()
        {
            return new XElement(WordprocessingElementNames.TableCell);
        }

        public static IEnumerable<XElement> GetTableRows(this XElement element)
        {
            return element.Elements(WordprocessingElementNames.TableRow);
        }

        public static IEnumerable<XElement> GetTableCells(this XElement element)
        {
            return element.Elements(WordprocessingElementNames.TableCell);
        }

        public static string GetTableCellValue(this XElement tableCell)
        {
            var paragraph = tableCell.GetParagraph();
            if(paragraph != null)
                return GetParagraphText(paragraph);

            throw new DocumentProcessingException("Could`t find table cell paragraph.", tableCell);
        }

        private static XElement GetTableRowProperties(this XElement tableRow)
        {
            return tableRow.Element(WordprocessingElementNames.TableRowProperties);
        }

        private static XElement GetTableCellProperties(this XElement tableCell)
        {
            return tableCell.Element(WordprocessingElementNames.TableCellProperties);
        }

        public static XElement CreateTableRowWithoutCells(this XElement sourceRow)
        {
            var resultRow = CreateTableRow();
            var sourceRowProperties = sourceRow.GetTableRowProperties();
            if(sourceRowProperties != null)
                resultRow.Add(sourceRowProperties.Clone());
            return resultRow;
        }

        public static XElement CreateTableCellWithoutValue(this XElement sourceCell)
        {
            var resultCell = CreateTableCell();

            var idAttribute = sourceCell.GetAttribute("id");
            if(idAttribute != null)
                resultCell.Add(idAttribute.Clone());

            var sourceCellProperties = sourceCell.GetTableCellProperties();
            if (sourceCellProperties != null)
                resultCell.Add(sourceCellProperties.Clone());

            resultCell.Add(sourceCell.GetParagraph().CreateParagraphWithEmptyRun());

            return resultCell;
        }

        #endregion

        #region RunProperties

        public static void SetTextBold(this XElement runProperties)
        {
            if (!runProperties.HaveElement(WordprocessingElementNames.Bold))
                runProperties.Add(new XElement(WordprocessingElementNames.Bold));
        }

        public static void SetTextItalic(this XElement runProperties)
        {
            if (!runProperties.HaveElement(WordprocessingElementNames.Italic))
                runProperties.Add(new XElement(WordprocessingElementNames.Italic));
        }

        public static void SetTextUnderlineSingle(this XElement runProperties)
        {
            if (!runProperties.HaveElement(WordprocessingElementNames.Underline))
                runProperties.Add(new XElement(WordprocessingElementNames.Underline, new XAttribute(WordprocessingElementNames.Val, "single")));
        }

        public static void SetTextStrike(this XElement runProperties)
        {
            if (!runProperties.HaveElement(WordprocessingElementNames.Strike))
                runProperties.Add(new XElement(WordprocessingElementNames.Strike));
        }

        public static void SetTextColor(this XElement runProperties, string color)
        {
            var colorElement = runProperties.Element(WordprocessingElementNames.Color);
            if (colorElement == null)
            {
                colorElement = new XElement(WordprocessingElementNames.Color);
                runProperties.Add(colorElement);
            }

            ChangeColorElementValue(colorElement, color);
        }

        public static void SetTextHighlightColor(this XElement runProperties, string color)
        {
            var highlight = runProperties.Element(WordprocessingElementNames.Highlight);
            if (highlight == null)
            {
                highlight = new XElement(WordprocessingElementNames.Highlight);
                runProperties.Add(highlight);
            }
            
            ChangeColorElementValue(highlight, color);
        }

        private static void ChangeColorElementValue(XElement element, string value)
        {
            var attr = element.Attribute(WordprocessingElementNames.Val);
            if (attr != null)
                attr.Value = value;
            else
            {
                attr = new XAttribute(WordprocessingElementNames.Val, value);
                element.Add(attr);
            }
        }

        #endregion

        #region Text

        public static XElement GetTextElement(this XElement element)
        {
            return element.Element(WordprocessingElementNames.Text);
        }

        private static XElement CreateTextElement()
        {
            return new XElement(WordprocessingElementNames.Text);
        }

        #endregion

        private static bool HaveElement(this XElement element, XName elementName)
        {
            return element.Element(elementName) != null;
        }

        public static XElement GetTagElement(this XElement element)
        {
            return element.Element(WordprocessingElementNames.Tag);
        }

        public static XElement Clone(this XElement sourceElement)
        {
            return new XElement(sourceElement);
        }

        private static XAttribute GetAttribute(this XElement element, string attributeName)
        {
            return element.Attribute(attributeName);
        }

    }
}
