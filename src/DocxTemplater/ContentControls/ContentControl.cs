using System.Xml.Linq;
using DocxTemplater.DataSources;
using DocxTemplater.Helpers;

namespace DocxTemplater.ContentControls
{
    internal abstract class ContentControl
    {
        private readonly XElement _sdtElement;
        protected XElement sdtContentElement;

        protected ContentControl(XElement sdtElement)
        {
            _sdtElement = sdtElement;
            sdtContentElement = SdtContentElementHelper.GetSdtContentElement(_sdtElement);
        }

        public void Replace(IXmlDataSource dataSource)
        {
            var result = Process(dataSource);
            _sdtElement.AddAfterSelf(result);
            _sdtElement.Remove();
        }

        protected abstract XElement Process(IXmlDataSource dataSource);
    }
}
