using System;

namespace DocxTemplater.Exceptions
{
    public sealed class DataSourceException : Exception
    {
        public DataSourceException(string errorMessage) :
            base(errorMessage)
        {
            
        }

        public DataSourceException(string errorMessage, Exception innerException) :
            base(errorMessage, innerException)
        {
            
        }
    }
}
