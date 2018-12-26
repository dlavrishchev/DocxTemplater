using System.Drawing;

namespace DocxTemplater
{
    internal sealed class DataItem
    {
        public string Value { get; set; }

        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Underline { get; set; }

        public Color TextColor { get; set; }
        public bool IsTextColorAssigned => TextColor != Color.Empty;

        public string TextHighlightColor { get; set; }
        public bool IsTextHighlightColorAssigned => !string.IsNullOrWhiteSpace(TextHighlightColor);
    }
}