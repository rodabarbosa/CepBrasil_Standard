using System;
using System.Net.Http;
using System.Threading.Tasks;
using Sirb.CepBrasil_Standard.Exceptions;
using Sirb.CepBrasil_Standard.Extensions;
using Sirb.CepBrasil_Standard.Interfaces;
using Sirb.CepBrasil_Standard.Messages;
using Sirb.CepBrasil_Standard.Models;
using Sirb.CepBrasil_Standard.Validations;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Sirb.CepBrasil_StandardTest")]

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

            var response = await GetFromService(cep.RemoveMask());

            ServiceException.ThrowIf(string.IsNullOrEmpty(response), CepMessage.ExceptionEmptyResponse);
            return ConverterCepResult(response);
        }

        private async Task<string> GetFromService(string cep)
        {
            var url = BuildRequestUrl(cep);
            using (var request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(url) })
            {
                using (var response = await _httpClient.SendAsync(request).ConfigureAwait(false))
                {
                    var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    ServiceException.ThrowIf(!response.IsSuccessStatusCode, CepMessage.ExceptionServiceError);

                    return responseString;
                }
            }
        }

        private static string BuildRequestUrl(string cep)
        {
            return $"https://viacep.com.br/ws/{cep}/json";
        }

        private static CepContainer ConverterCepResult(string response)
        {
            return response.FromJson<CepContainer>();
        }
    }
}
