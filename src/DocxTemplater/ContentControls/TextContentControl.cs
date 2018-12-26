using System.Xml.Linq;
using System.Xml.XPath;
using DocxTemplater.DataSources;
using DocxTemplater.Exceptions;
using DocxTemplater.Helpers;

namespace DocxTemplater.ContentControls
{
    internal sealed class TextContentControl : ContentControl
    {
        private IXmlDataSource _dataSource;

        public const string Name = "dt.text";

        public TextContentControl(XElement sdtElement) : base(sdtElement) { }

        protected override XElement Process(IXmlDataSource dataSource)
        {
            _dataSource = dataSource;
            var dataPath = GetDataPath();
            var dataItem = GetDataItem(dataPath);

            var originParagraphElement = ParagraphElementHelper.GetParagraphElement(sdtContentElement);
            if (originParagraphElement != null)
            {
                var originRunElement = RunElementHelper.GetRunElement(originParagraphElement);
                var resultRunElement = RunElementHelper.CreateRunElement(originRunElement, dataItem);
                var resultParagraphElement = ParagraphElementHelper.CloneParagraphElement(originParagraphElement);
                ParagraphElementHelper.AddElement(resultParagraphElement, resultRunElement);
                return resultParagraphElement;
            }
            else
            {
                var originRunElement = RunElementHelper.GetRunElement(sdtContentElement);
                var resultRunElement = RunElementHelper.CreateRunElement(originRunElement, dataItem);
                return resultRunElement;
            } 
        }

        private DataItem GetDataItem(string xPath)
        {
            var creator = new DataItemCreator();
            var dataNavigator = GetDataNavigator(xPath);
            var dataItem = creator.Create(dataNavigator);
            return dataItem;
        }

        private XPathNavigator GetDataNavigator(string xPath)
        {
            var navigator = _dataSource.GetNodeNavigator(xPath);
            if (navigator == null)
                throw new DataSourceException($"Could not find data for '{xPath}' path.");
            return navigator;
        }

        private string GetDataPath()
        {
            var dataPath = SdtContentElementHelper.GetText(sdtContentElement);
            if (string.IsNullOrWhiteSpace(dataPath))
                throw new TemplateException("The data path cannot be empty.", sdtContentElement.ToString());
            return dataPath;
        }
    }
}
