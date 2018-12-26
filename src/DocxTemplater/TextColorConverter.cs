using System;
using System.Drawing;
using System.Globalization;

namespace DocxTemplater
{
    internal sealed class TextColorConverter
    {
        public Color Convert(string hexValue)
        {
            return string.IsNullOrWhiteSpace(hexValue) ? Color.Empty : CreateColorFromHex(hexValue);
        }

        public static string ColorToHexString(Color color)
        {
            return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        /// <summary>
        /// Create Color instance from hex string.
        /// </summary>
        /// <param name="hexValue">Format: "#AARRGGBB" or "#RRGGBB"</param>
        private Color CreateColorFromHex(string hexValue)
        {
            Guard.NotNullOrWhiteSpace(hexValue, nameof(hexValue));

            var hex = hexValue.TrimStart('#');
            if (hex.Length != 6 && hex.Length != 8)
                throw new ArgumentException("The color code must be 6 or 8 hexadecimal digits. For example: #FF471A", nameof(hexValue));

            try
            {
                return hex.Length == 6 ? CreateRGBColor(hex) : CreateARGBColor(hex);
            }
            catch (FormatException)
            {
                throw new ArgumentException("The color code must be 6 or 8 hexadecimal digits. For example: #FF471A", nameof(hexValue));
            }
        }

        private static Color CreateRGBColor(string hex)
        {
            var red = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            var green = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            var blue = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            return Color.FromArgb(red, green, blue);
        }

        private static Color CreateARGBColor(string hex)
        {
            var alpha = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            var red = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            var green = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            var blue = int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            return Color.FromArgb(alpha, red, green, blue);
        }
    }
}
