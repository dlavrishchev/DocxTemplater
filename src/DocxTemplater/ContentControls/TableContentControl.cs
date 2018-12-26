using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using DocxTemplater.DataSources;
using DocxTemplater.Exceptions;
using DocxTemplater.Extensions;
using DocxTemplater.Helpers;

namespace DocxTemplater.ContentControls
{
    internal sealed class TableContentControl : ContentControl
    {
        public const string Name = "dt.table";

        public TableContentControl(XElement sdtElement) : base(sdtElement) { }
        
        protected override XElement Process(IXmlDataSource dataSource)
        {
            var resultTableElement = CreateResultTableElement();
            var dataRowIndex = GetDataRowIndex();
            var dataRowElement = TableElementHelper.GetRowElement(resultTableElement, dataRowIndex);
            if (dataRowElement == null)
                throw new TemplateException($"Row with index '{dataRowIndex}' not found.", sdtContentElement.ToString());

            var cellElements = RowElementHelper.GetCellElements(dataRowElement).ToArray();
            var cellDataPaths = cellElements.Select(CellElementHelper.GetCellValue).ToArray();

            var dataItemCreator = new DataItemCreator();
            var dataPath = GetDataPath();
            if (string.IsNullOrWhiteSpace(dataPath))
                throw new TemplateException("The data path cannot be empty.", sdtContentElement.ToString());

            foreach (XPathNavigator dataNavigator in dataSource.GetNodesIterator(dataPath))
            {
                var resultRow = CreateResultDataRowElement(dataRowElement, cellElements, cellDataPaths, dataNavigator, dataItemCreator);
                TableElementHelper.AddRow(resultTableElement, resultRow);
            }

            dataRowElement.Remove();
            return resultTableElement;
        }

        private XElement CreateResultTableElement()
        {
            return GetOriginTableElement().DeepCopy();
        }

        private XElement CreateResultDataRowElement(
            XElement originRow,
            XElement[] cellElements,
            string[] cellDataPaths,
            XPathNavigator dataNavigator,
            DataItemCreator dataItemCreator)
        {
            var resultRow = RowElementHelper.CloneRowElement(originRow);
            for (var i = 0; i < cellElements.Length; i++)
            {
                var dataItem = CreateDataItemForDataPath(cellDataPaths[i], dataNavigator, dataItemCreator);
                var cellElement = CellElementHelper.CreateCellElement(cellElements[i], dataItem);
                RowElementHelper.AddCellElement(resultRow, cellElement);
            }

            return resultRow;
        }

        private DataItem CreateDataItemForDataPath(string dataPath, XPathNavigator xPathNavigator, DataItemCreator dataItemCreator)
        {
            if (!string.IsNullOrWhiteSpace(dataPath))
            {
                var nodeNavigator = xPathNavigator.SelectSingleNode(dataPath);
                if (nodeNavigator == null)
                    throw new DataSourceException($"Could not find data for data path '{dataPath}'.");

                return dataItemCreator.Create(nodeNavigator);
            }

            return dataItemCreator.Create();
        }

        private XElement GetOriginTableElement()
        {
            var tableElement = TableElementHelper.GetTableElement(sdtContentElement);
            if (tableElement == null)
                throw new TemplateException("Could not find table.", sdtContentElement.ToString());
            return tableElement;
        }

        private int GetDataRowIndex()
        {
            var sdtElement = GetDataRowIndexSdtElement();
            var indexAsString = SdtElementHelper.GetText(sdtElement);

            if (string.IsNullOrWhiteSpace(indexAsString))
                throw new TemplateException("Data row index value not set. Value cannot be empty.", sdtContentElement.ToString());

            if (!int.TryParse(indexAsString, NumberStyles.Integer, null, out var index))
                throw new TemplateException("Invalid data row index value format. Value must be integer.", sdtContentElement.ToString());

            if (index < 0)
                throw new TemplateException($"Invalid data row index value '{index}'. Value must be >= 0.", sdtContentElement.ToString());

            return index;
        }

        private string GetDataPath()
        {
            var sdtElement = GetDataPathSdtElement();
            return SdtElementHelper.GetText(sdtElement);
        }

        private XElement GetDataRowIndexSdtElement()
        {
            return GetInnerSdtElementByTag("dt.table.dataRowIndex");
        }

        private XElement GetDataPathSdtElement()
        {
            return GetInnerSdtElementByTag("dt.table.dataPath");
        }

        private XElement GetInnerSdtElementByTag(string tag)
        {
            var sdtElement = SdtElementHelper.GetDescendantSdtElementByTag(sdtContentElement, tag);
            if (sdtElement == null)
                throw new TemplateException($"Could not find '{tag}' content control.", sdtContentElement.ToString());

            return sdtElement;
        }

    }
}
