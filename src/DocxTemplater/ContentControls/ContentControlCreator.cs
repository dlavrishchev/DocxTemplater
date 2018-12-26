using System.Xml.Linq;
using DocxTemplater.Exceptions;
using DocxTemplater.Helpers;

namespace DocxTemplater.ContentControls
{
    internal sealed class ContentControlCreator
    {
        public ContentControl Create(XElement sdtElement)
        {
            var sdtTag = GetTag(sdtElement);

            switch (sdtTag.ToLower())
            {
                case TextContentControl.Name:
                    return new TextContentControl(sdtElement);

                case TableContentControl.Name:
                    return new TableContentControl(sdtElement);

                default:
                    throw new TemplateException($"Unknown content control tag '{sdtTag}'.", sdtElement.ToString());
            }
        }

        private string GetTag(XElement sdtElement)
        {
            string sdtTag = SdtElementHelper.GetTag(sdtElement);
            if (sdtTag == null)
                throw new TemplateException("Could not find content control tag.", sdtElement.ToString());
            if (string.IsNullOrWhiteSpace(sdtTag))
                throw new TemplateException("Invalid content control tag. Tag cannot be empty.", sdtElement.ToString());

            return sdtTag;
        }
    }
}
