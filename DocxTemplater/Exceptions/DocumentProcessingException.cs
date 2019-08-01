using System;
using System.Xml.Linq;

namespace DocxTemplater.Exceptions
{
    [Serializable]
    public sealed class DocumentProcessingException : Exception
    {
        public XElement CurrentElement { get; }

        public DocumentProcessingException(string errorMessage, XElement currentElement) : base(errorMessage)
        {
            CurrentElement = currentElement;
        }
    }
}
