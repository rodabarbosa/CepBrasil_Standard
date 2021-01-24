using Sirb.CepBrasil_Standard.Exceptions;
using Sirb.CepBrasil_Standard.Extensions;
using Sirb.CepBrasil_Standard.Interfaces;
using Sirb.CepBrasil_Standard.Messages;
using Sirb.CepBrasil_Standard.Models;
using Sirb.CepBrasil_Standard.Validations;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sirb.CepBrasil_Standard.Services
{
	internal sealed class ViaCepService : ICepServiceControl
	{
		private readonly HttpClient _httpClient;

		public ViaCepService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<CepContainer> Find(string cep)
		{
			CepValidation.Validate(cep);

			string response = await GetFromService(cep.RemoveMask());
			ServiceException.When(string.IsNullOrEmpty(response), CepMessage.ExceptionEmptyResponse);
			return ConverterCepResult(response);
		}

		private async Task<string> GetFromService(string cep)
		{
			string url = BuildRequestUrl(cep);
			using (var request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(url) })
			{
				using (HttpResponseMessage response = await _httpClient.SendAsync(request).ConfigureAwait(false))
				{
					string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					ServiceException.When(!response.IsSuccessStatusCode, CepMessage.ExceptionServiceError);

					return responseString;
				}
			}
		}

		private static string BuildRequestUrl(string cep) => $"https://viacep.com.br/ws/{cep}/json";

		private static CepContainer ConverterCepResult(string response) => response.FromJson<CepContainer>();
	}
}