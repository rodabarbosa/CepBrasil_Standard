using Sirb.CepBrasil_Standard.Exceptions;
using Sirb.CepBrasil_Standard.Extensions;
using Sirb.CepBrasil_Standard.Messages;

namespace Sirb.CepBrasil_Standard.Validations
{
	internal class CepValidation
	{
		private const int ZipCodeLength = 8;

		/// <summary>
		/// Validate brazilian zip code to its minimum value standard.
		/// </summary>
		/// <param name="zipCode"></param>
		public static void Validate(string zipCode)
		{
			string value = zipCode?.RemoveMask();
			ServiceException.When((value?.Length ?? 0) != ZipCodeLength, CepMessage.ZipCodeInvalidMessage);
		}
	}
}