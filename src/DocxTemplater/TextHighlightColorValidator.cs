using System;
using System.Collections.Generic;
using System.Linq;

namespace DocxTemplater
{
    internal class TextHighlightColorValidator
    {
        private readonly HashSet<string> _supportedColors;

        public TextHighlightColorValidator()
        {
            _supportedColors = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "black",
                "blue",
                "cyan",
                "darkBlue",
                "darkCyan",
                "darkGray",
                "darkGreen",
                "darkMagenta",
                "darkRed",
                "darkYellow",
                "green",
                "lightGray",
                "magenta",
                "red",
                "white",
                "yellow"
            };
        }

        public bool IsSupportedColor(string color)
        {
            return !string.IsNullOrWhiteSpace(color) && _supportedColors.Contains(color.ToLower());
        }

        public IEnumerable<string> GetSupportedColors()
        {
            return _supportedColors.Select(color => color);
        }
    }
}
