using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Sirb.CepBrasil.Services;
using Sirb.CepBrasil.Models;

namespace Sirb.CepBrasilTest
{
	public sealed class CepServiceTest
	{
		private readonly HttpClient _httpClient;

		public CepServiceTest()
		{
			_httpClient = new HttpClient();
		}

		[Theory]
		[InlineData("83040-040")]
		[InlineData("80035-020")]
		public async Task GetCepSuccess(string cep)
		{
			var cepService = new CepService(_httpClient);
			CepResult result = await cepService.Find(cep).ConfigureAwait(false);
			Assert.True(result.Success);
		}
	}
}
