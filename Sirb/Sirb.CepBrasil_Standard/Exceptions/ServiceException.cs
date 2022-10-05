using System;

namespace Sirb.CepBrasil_Standard.Exceptions
{
    [Serializable]
    public sealed class ServiceException : Exception
    {
        [NonSerialized] private const string DefaultMessage = "Unexpected error occurred in the service.";

        public ServiceException() : this(DefaultMessage)
        {
        }

        public ServiceException(string message) : this(message, null)
        {
        }

        public ServiceException(Exception innerException) : this(DefaultMessage, innerException)
        {
        }

        public ServiceException(string message, Exception innerException) : base(DefineMessage(message, DefaultMessage), innerException)
        {
        }

        private static string DefineMessage(string message, string fallbackMessage)
        {
            return !string.IsNullOrEmpty(message?.Trim()) ? message : fallbackMessage;
        }

        [Obsolete("Use ThrowIf instead.")]
        public static void When(bool condition, string message, Exception innerException = null)
        {
            ThrowIf(condition, message, innerException);
        }

        /// <summary>
        /// Throws ServiceException when condition are met.
        /// </summary>
        /// <param name="condition">Condition for exception</param>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public static void ThrowIf(bool condition, string message, Exception innerException = null)
        {
            if (condition)
                throw new ServiceException(message, innerException);
        }
    }
}
