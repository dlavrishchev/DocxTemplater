using System;

namespace DocxTemplater
{
    internal static class Guard
    {
        public static void NotNull<T>(T value, string argumentName) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(argumentName);
        }

        public static void NotNullOrWhiteSpace(string value, string argumentName)
        {
            NotNull(value, argumentName);

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Argument must not be empty or whitespace.", argumentName);
        }
    }
}
