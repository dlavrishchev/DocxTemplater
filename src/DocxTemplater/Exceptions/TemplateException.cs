using System;

namespace DocxTemplater.Exceptions
{
    public sealed class TemplateException : Exception
    {
        public string Context { get; }

        public TemplateException(string errorMessage, string context):base(errorMessage)
        {
            Context = context;
        }

    }
}
