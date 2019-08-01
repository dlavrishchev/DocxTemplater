using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using DocxTemplater.Exceptions;
using DocxTemplater.Extensions;

namespace DocxTemplater.WordProcessing.Parsers
{
    internal class TableContentControlParser : SdtContentParser
    {
        public XElement TableElement { get; private set; }
        public XElement DataRowElement { get; private set; }
        public int DataRowIndex { get; private set; }
        public string Path { get; private set; }

        public TableContentControlParser(XElement sdtContent) : base(sdtContent)
        {
            
        }

        public override void Parse()
        {
            TableElement = GetTableElement();
            DataRowIndex = GetDataRowIndex();
            DataRowElement = GetDataRowElement(TableElement, DataRowIndex);
            Path = GetDataPath();
        }

        private XElement GetTableElement()
        {
            var tableElement = sdtContent.GetTable();
            if (tableElement == null)
                throw new DocumentProcessingException("Could not find table element.", sdtContent);
            return tableElement;
        }

        private int GetDataRowIndex()
        {
            var sdt = sdtContent.GetSdtByTag("dt.table.dataRowIndex");
            if (sdt == null)
                throw new DocumentProcessingException("Could not find table's data row index content control.", sdtContent);

            var indexAsString = sdt.GetSdtContent().GetSdtContentText();

            if (string.IsNullOrWhiteSpace(indexAsString))
                throw new DocumentProcessingException("Data row index value not set. Value cannot be empty.", sdtContent);

            if (!int.TryParse(indexAsString, NumberStyles.Integer, null, out var index))
                throw new DocumentProcessingException("Invalid data row index value format. Value must be integer.", sdtContent);

            if (index < 0)
                throw new DocumentProcessingException($"Invalid data row index value '{index}'. Value must be >= 0.", sdtContent);

            return index;
        }

        private XElement GetDataRowElement(XElement tableElement, int rowIndex)
        {
            return tableElement.GetTableRows().ElementAtOrDefault(rowIndex);
        }

        private string GetDataPath()
        {
            var sdt = sdtContent.GetSdtByTag("dt.table.dataPath");
            if(sdt == null)
                throw new DocumentProcessingException("Could not find table's data path content control.", sdtContent);

            return sdt.Element(WordprocessingElementNames.SdtContent).GetSdtContentText();
        }
    }
}
