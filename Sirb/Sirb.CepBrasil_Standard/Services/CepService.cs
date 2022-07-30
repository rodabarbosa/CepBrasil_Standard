using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Sirb.CepBrasil_Standard.Extensions;
using Sirb.CepBrasil_Standard.Interfaces;
using Sirb.CepBrasil_Standard.Models;

namespace Sirb.CepBrasil_Standard.Services
{
    public sealed class CepService : ICepService, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly bool _httpClientSelfCreated;
        private readonly List<ICepServiceControl> _services;

        private CepService(HttpClient httpClient, bool httpClientSelfCreated)
        {
            _services = new List<ICepServiceControl>();
            _httpClientSelfCreated = httpClientSelfCreated;
            _httpClient = httpClient;

            StartServices();
        }

        public CepService() : this(new HttpClient(), true)
        {
        }

        public CepService(HttpClient httpClient) : this(httpClient, false)
        {
        }

        public async Task<CepResult> Find(string cep)
        {
            var result = new CepResult();
            foreach (ICepServiceControl service in _services)
            {
                await FindAsync(result, service, cep);

                if (result.Success)
                    return result;
            }

            return result;
        }

        public void Dispose()
        {
            if (_httpClientSelfCreated)
                _httpClient?.Dispose();
        }

        private async Task FindAsync(CepResult result, ICepServiceControl service, string cep)
        {
            try
            {
                result.CepContainer = await service.Find(cep);
                result.Success = true;
            }
            catch (Exception e)
            {
                if (result.Exceptions == null)
                    result.Exceptions = new List<Exception>();

                result.Exceptions.Add(e);

                string value = result.Message ?? string.Empty;
                result.Message = $"{value}{e.AllMessages() ?? string.Empty} ";
            }
        }

        private void StartServices()
        {
            _services.Add(new CorreiosService(_httpClient));
            _services.Add(new ViaCepService(_httpClient));
        }
    }
}
