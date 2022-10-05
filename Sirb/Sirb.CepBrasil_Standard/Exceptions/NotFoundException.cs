using System;

namespace Sirb.CepBrasil_Standard.Exceptions
{
    public class NotFoundException : Exception
    {
        private const string DefaultMessage = "Not found";

        public NotFoundException() : this(DefaultMessage)
        {
        }

        public NotFoundException(string message) : this(message, null)
        {
        }

        public NotFoundException(Exception innerException) : this(DefaultMessage, innerException)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static void ThrowIf(bool condition, string message, Exception inner = null)
        {
            if (condition)
                throw new NotFoundException(message, inner);
        }
    }
}
