using System;
using System.Collections.Generic;

namespace DocxTemplater.WordProcessing
{
    internal class ColorValidator
    {
        private readonly HashSet<string> _highlightColors;
        private static ColorValidator _instance;

        public static ColorValidator Instance => _instance ?? (_instance = new ColorValidator());

        private ColorValidator()
        {
            _highlightColors = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
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

        public bool IsHighlightColorValid(string highlightColor)
        {
            return !string.IsNullOrWhiteSpace(highlightColor) && _highlightColors.Contains(highlightColor.ToLower().Trim());
        }

        public bool IsTextColorValid(string textColor)
        {
            if (string.IsNullOrWhiteSpace(textColor))
                return false;

            var colorCode = GetColorCode(textColor);
            if (colorCode.Length != 6 && colorCode.Length != 8)
                return false;

            return true;
        }

        private static string GetColorCode(string colorCodeString)
        {
            return colorCodeString.TrimStart('#').Trim();
        }
    }
}
