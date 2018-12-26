using System;
using System.Drawing;
using System.Xml.XPath;
using DocxTemplater.Exceptions;
using DocxTemplater.Extensions;

namespace DocxTemplater
{
    internal sealed class DataItemCreator
    {
        private readonly TextColorConverter _textColorConverter;
        private readonly TextHighlightColorValidator _textHighlightColorValidator;

        public DataItemCreator()
        {
            _textColorConverter = new TextColorConverter();
            _textHighlightColorValidator = new TextHighlightColorValidator();
        }

        public DataItem Create(XPathNavigator nodeNavigator)
        {
            var dataItem = new DataItem
            {
                Value = nodeNavigator.Value,
                Bold = GetBold(nodeNavigator),
                Italic = GetItalic(nodeNavigator),
                Underline = GetUnderline(nodeNavigator),
                TextColor = GetTextColor(nodeNavigator),
                TextHighlightColor = GetTextHighlightColor(nodeNavigator)
            };
            return dataItem;
        }

        public DataItem Create()
        {
            return new DataItem();
        }

        private bool GetBold(XPathNavigator nodeNavigator)
        {
            return GetBooleanAttributeValue(nodeNavigator, "bold");
        }

        private bool GetItalic(XPathNavigator nodeNavigator)
        {
            return GetBooleanAttributeValue(nodeNavigator, "italic");
        }

        private bool GetUnderline(XPathNavigator nodeNavigator)
        {
            return GetBooleanAttributeValue(nodeNavigator, "underline");
        }

        private Color GetTextColor(XPathNavigator nodeNavigator)
        {
            var attr = nodeNavigator.GetAttribute("textColor", string.Empty);
            try
            {
                return _textColorConverter.Convert(attr);
            }
            catch (ArgumentException ex)
            {
                throw new DataSourceException(ex.Message, ex);
            }
        }

        private string GetTextHighlightColor(XPathNavigator nodeNavigator)
        {
            var attr = nodeNavigator.GetAttribute("textHighlightColor", string.Empty);
            if (string.IsNullOrWhiteSpace(attr))
                return null;

            if (_textHighlightColorValidator.IsSupportedColor(attr))
                return attr;

            var supportedColors = _textHighlightColorValidator.GetSupportedColors().CreateCommaSeparatedString();
            throw new DataSourceException($"Invalid text highlight color '{attr}' value. Supported colors set: {supportedColors}.");
        }

        private bool GetBooleanAttributeValue(XPathNavigator nodeNavigator, string attributeName)
        {
            var attr = nodeNavigator.GetAttribute(attributeName, string.Empty);
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
