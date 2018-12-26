using System.Xml.Linq;

namespace DocxTemplater
{
    internal static class OpenXmlElementNames
    {
        private static readonly XNamespace _ns = @"http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        public static readonly XName Sdt =                  _ns + "sdt";
        public static readonly XName SdtProperties =        _ns + "sdtPr";
        public static readonly XName SdtContent =           _ns + "sdtContent";
        public static readonly XName Paragraph =            _ns + "p";
        public static readonly XName ParagraphProperties =  _ns + "pPr";
        public static readonly XName Run =                  _ns + "r";
        public static readonly XName RunProperties =        _ns + "rPr";
        public static readonly XName Text =                 _ns + "t";
        public static readonly XName Tag =                  _ns + "tag";
        public static readonly XName Val =                  _ns + "val";
        public static readonly XName Body =                 _ns + "body";
        public static readonly XName Bold =                 _ns + "b";
        public static readonly XName Italic =               _ns + "i";
        public static readonly XName Underline =            _ns + "u";
        public static readonly XName Color =                _ns + "color";
        public static readonly XName Highlight =            _ns + "highlight";
        public static readonly XName Table =                _ns + "tbl";
        public static readonly XName TableRow =             _ns + "tr";
        public static readonly XName TableRowProperties =   _ns + "trPr";
        public static readonly XName TableCell =            _ns + "tc";
        public static readonly XName TableCellProperties =  _ns + "tcPr";
    }
}
