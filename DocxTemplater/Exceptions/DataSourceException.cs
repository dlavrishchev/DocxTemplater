using System;

namespace DocxTemplater.Exceptions
{
    [Serializable]
    public sealed class DataSourceException : Exception
    {
        public DataSourceException(string errorMessage) :
            base(errorMessage)
        {
            
        }
    }
}
