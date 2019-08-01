using System.Xml.XPath;
using DocxTemplater.Exceptions;

namespace DocxTemplater.DataSources
{
    internal class DataProvider
    {
        public string Value => _navigator.Value;
        public string HighlightColor => _navigator.GetAttribute(DataAttributeNames.HighlightColor, string.Empty);
        public string TextColor => _navigator.GetAttribute(DataAttributeNames.TextColor, string.Empty);

        public bool Bold => GetBooleanAttributeValue(DataAttributeNames.Bold);
        public bool Italic => GetBooleanAttributeValue(DataAttributeNames.Italic);
        public bool Underline => GetBooleanAttributeValue(DataAttributeNames.Underline);
        public bool IsTextColorAssigned => !string.IsNullOrWhiteSpace(TextColor);
        public bool IsHighlightColorAssigned => !string.IsNullOrWhiteSpace(HighlightColor);

        private readonly XPathNavigator _navigator;

        public DataProvider(XPathNavigator navigator)
        {
            _navigator = navigator;
        }

        private bool GetBooleanAttributeValue(string attributeName)
        {
            var attr = _navigator.GetAttribute(attributeName, string.Empty);
            return ParseBoolValue(attr);
        }

        private bool ParseBoolValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            if (!bool.TryParse(value, out var result))
                throw new DataSourceException($"Invalid boolean value '{value}'. Value must be 'true' or 'false'.");
            return result;
        }
    }
}