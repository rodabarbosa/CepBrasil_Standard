using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sirb.CepBrasil_Standard.Exceptions;
using Sirb.CepBrasil_Standard.Extensions;
using Sirb.CepBrasil_Standard.Interfaces;
using Sirb.CepBrasil_Standard.Messages;
using Sirb.CepBrasil_Standard.Models;

[assembly: InternalsVisibleTo("Sirb.CepBrasil_StandardTest")]

namespace Sirb.CepBrasil_Standard.Services
{
    internal sealed class CorreiosService : ICepServiceControl
    {
        private const string CorreiosUrl = "https://apphom.correios.com.br/SigepMasterJPA/AtendeClienteService/AtendeCliente";
        private readonly HttpClient _httpClient;

        public CorreiosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CepContainer> Find(string cep)
        {
            var response = await GetFromService(cep.RemoveMask());

            ServiceException.ThrowIf(string.IsNullOrEmpty(response), CepMessage.ExceptionEmptyResponse);
            return ConvertResult(response);
        }

        private async Task<string> GetFromService(string cep)
        {
            using (var request = new HttpRequestMessage { Method = HttpMethod.Post, RequestUri = new Uri(CorreiosUrl) })
            {
                request.Content = GetRequestContent(cep);

                using (var response = await _httpClient.SendAsync(request).ConfigureAwait(false))
                {
                    var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    ServiceException.ThrowIf(!response.IsSuccessStatusCode, GetFaultString(responseString));

                    return responseString;
                }
            }
        }

        private static HttpContent GetRequestContent(string cep)
        {
            return new StringContent(BuildSoapBody(cep), Encoding.UTF8, "application/xml");
        }

        private static string BuildSoapBody(string cep)
        {
            var sb = new StringBuilder("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:cli=\"http://cliente.bean.master.sigep.bsb.correios.com.br/\">")
                .Append("<soapenv:Header/>")
                .Append("<soapenv:Body>")
                .Append("<cli:consultaCEP>")
                .Append($"<cep>{cep}</cep>")
                .Append("</cli:consultaCEP>")
                .Append("</soapenv:Body>")
                .Append("</soapenv:Envelope>");

            return sb.ToString();
        }

        private static string GetTagValue(string rawValue, string tagName, string tagNotFoundMessage = null)
        {
            MatchCollection result = Regex.Matches(rawValue, $"<{tagName}>(.*?)</{tagName}>");
            if (result.Count == 0)
                return tagNotFoundMessage;

            return result[0].Value.Replace($"</{tagName}>", string.Empty)
                .Replace($"<{tagName}>", string.Empty);
        }

        private static string GetFaultString(string response)
        {
            return GetTagValue(response, "faultstring", CepMessage.ExceptionServiceError);
        }

        private static CepContainer ConvertResult(string result)
        {
            return new CepContainer
            {
                Bairro = GetBairroValue(result),
                Cep = GetCepValue(result),
                Cidade = GetCidadeValue(result),
                Complemento = GetComplementoValue(result),
                Logradouro = GetEnderecoValue(result),
                Uf = GetUfValue(result)
            };
        }

        private static string GetBairroValue(string result)
        {
            return GetTagValue(result, "bairro");
        }

        private static string GetCepValue(string result)
        {
            return GetTagValue(result, "cep");
        }

        private static string GetCidadeValue(string result)
        {
            return GetTagValue(result, "cidade");
        }

        private static string GetComplementoValue(string result)
        {
            return GetTagValue(result, "complemento2");
        }

        private static string GetEnderecoValue(string result)
        {
            return GetTagValue(result, "end");
        }

        private static string GetUfValue(string result)
        {
            return GetTagValue(result, "uf");
        }
    }
}
