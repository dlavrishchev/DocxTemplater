using System.Xml.Linq;
using DocxTemplater.DataSources;
using DocxTemplater.Exceptions;
using DocxTemplater.Extensions;

namespace DocxTemplater.WordProcessing
{
    internal class RunFiller
    {
        public void Fill(XElement run, DataProvider dataProvider)
        {
            run.SetRunText(dataProvider.Value);

            var runProperties = run.GetRunProperties();

            if (dataProvider.Bold)
                runProperties.SetTextBold();

            if (dataProvider.Italic)
                runProperties.SetTextItalic();

            if (dataProvider.Underline)
                runProperties.SetTextUnderlineSingle();

            if (dataProvider.IsTextColorAssigned)
            {
                var textColor = dataProvider.TextColor;
                if (!ColorValidator.Instance.IsTextColorValid(textColor))
                    throw new DataSourceException($"Invalid text color: {textColor}");

                runProperties.SetTextColor(textColor);
            }

            if (dataProvider.IsHighlightColorAssigned)
            {
                var highlightColor = dataProvider.HighlightColor;
                if(!ColorValidator.Instance.IsHighlightColorValid(highlightColor))
                    throw new DataSourceException($"Invalid highlight color: {highlightColor}");

                runProperties.SetTextHighlightColor(highlightColor);
            }
        }
    }
}
