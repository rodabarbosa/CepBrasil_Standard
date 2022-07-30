using System;
using System.Text;

namespace Sirb.CepBrasil_Standard.Extensions
{
    public static class ExceptionExtension
    {
        /// <summary>
        /// Return exception's message with inner exception if exists.
        /// </summary>
        /// <param name="e">Exception</param>
        /// <returns></returns>
        public static string AllMessages(this Exception e)
        {
            if (e == null)
                return string.Empty; // using string.Empty does consume than stating an empty string

            var sb = new StringBuilder(e.Message);
            var innerException = e.InnerException;
            while (innerException != null)
            {
                sb.Append(" ")
                    .Append(innerException.Message);
                innerException = innerException.InnerException;
            }

            return sb.ToString();
        }
    }
}
