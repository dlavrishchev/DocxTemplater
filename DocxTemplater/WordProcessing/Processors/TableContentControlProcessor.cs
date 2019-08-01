using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using DocxTemplater.DataSources;
using DocxTemplater.Exceptions;
using DocxTemplater.Extensions;
using DocxTemplater.WordProcessing.Parsers;

namespace DocxTemplater.WordProcessing.Processors
{
    internal sealed class TableContentControlProcessor : ContentControlProcessor
    {
        public TableContentControlProcessor(XElement sdtElement)
            : base(sdtElement)
        {
            
        }

        public override XElement Process(IXPathNavigable dataSource)
        {
            var parser = new TableContentControlParser(sdtContent);
            parser.Parse();

            if (parser.DataRowElement == null)
                throw new DocumentProcessingException($"Row with index '{parser.DataRowIndex}' not found.", sdtContent);

            if (string.IsNullOrWhiteSpace(parser.Path))
                throw new DocumentProcessingException("The data path cannot be empty.", sdtContent);


            var table = CreateResultTable(parser.TableElement, parser.DataRowIndex);
            var cells = parser.DataRowElement.GetTableCells().ToArray();
            var cellDataPaths = cells.Select(c => c.GetTableCellValue()).ToArray();

            var nodeIterator = dataSource.CreateNavigator().Select(parser.Path);
            foreach (XPathNavigator nodeNavigator in nodeIterator)
            {
                var row = parser.DataRowElement.CreateTableRowWithoutCells();
                FillRow(row, cells, cellDataPaths, nodeNavigator);
                table.Add(row);
            }

            return table;
        }

        private XElement CreateResultTable(XElement templateTable, int dataRowIndex)
        {
            var resultTable = templateTable.CreateTableWithoutRows();
            if (dataRowIndex > 0)
            {
                var titleRow = templateTable.GetTableRows().ElementAt(0);
                resultTable.Add(titleRow.Clone());
            }

            return resultTable;
        }

        private void FillRow(
            XElement row,
            XElement[] cells,
            string[] cellDataPaths,
            XPathNavigator nodeNavigator)
        {
            var runFiller = new RunFiller();
            for (var i = 0; i < cells.Length; i++)
            {
                var dataProvider = CreateDataProvider(cellDataPaths[i], nodeNavigator);
                var cell = CreateCell(cells[i], dataProvider, runFiller);
                
                row.Add(cell);
            }
        }

        private XElement CreateCell(XElement sourceCell, DataProvider dataProvider, RunFiller runFiller)
        {
            var resultCell = sourceCell.CreateTableCellWithoutValue();
            runFiller.Fill(resultCell.GetParagraph().GetRun(), dataProvider);
            return resultCell;
        }

        private DataProvider CreateDataProvider(string dataPath, XPathNavigator parentNodeNavigator)
        {
            var childNodeNavigator = parentNodeNavigator.SelectSingleNode(dataPath);
            if (childNodeNavigator == null)
                throw new DataSourceException($"Could not find data for data path '{dataPath}'.");

            return new DataProvider(childNodeNavigator);
        }

    }
}
