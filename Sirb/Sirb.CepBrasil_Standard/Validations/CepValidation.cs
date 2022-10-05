using Sirb.CepBrasil_Standard.Exceptions;
using Sirb.CepBrasil_Standard.Extensions;
using Sirb.CepBrasil_Standard.Messages;

namespace Sirb.CepBrasil_Standard.Validations
{
    internal static class CepValidation
    {
        private const int ZipCodeLength = 8;

        /// <summary>
        /// Validate brazilian zip code to its minimum value standard.
        /// </summary>
        /// <param name="zipCode"></param>
        public static void Validate(string zipCode)
        {
            var value = zipCode?.RemoveMask();
            var valueLength = value?.Length ?? 0;
            ServiceException.ThrowIf(valueLength != ZipCodeLength, CepMessage.ZipCodeInvalidMessage);
        }
    }
}
