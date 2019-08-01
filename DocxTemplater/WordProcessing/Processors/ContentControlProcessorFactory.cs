using System.Xml.Linq;

namespace DocxTemplater.WordProcessing.Processors
{
    internal sealed class ContentControlProcessorFactory
    {
        public ContentControlProcessor Create(string tag, XElement sdtElement)
        {
            switch (tag.ToLower())
            {
                case ContentControlNames.Text:
                    return new TextContentControlProcessor(sdtElement);

                case ContentControlNames.Table:
                    return new TableContentControlProcessor(sdtElement);

                default:
                    return null;
            }
        }
        
    }
}
