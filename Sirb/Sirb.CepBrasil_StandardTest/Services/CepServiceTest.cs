using Sirb.CepBrasil_Standard.Services;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sirb.CepBrasil_StandardTest.Services
{
    public sealed class CepServiceTest
    {
        [Theory]
        [InlineData("83040-040")]
        [InlineData("80035-020")]
        [InlineData("00000-000")]
        public async Task GetCepSuccess_NoHttp(string cep)
        {
            using (var service = new CepService())
            {
                var result = await service.Find(cep).ConfigureAwait(false);
                Assert.True(result.Success);
            }
        }

        [Theory]
        [InlineData("83040-040")]
        [InlineData("80035-020")]
        public async Task GetCepSuccess_WithHttp(string cep)
        {
            using (var httpClient = new HttpClient())
            {
                using (var service = new CepService(httpClient))
                {
                    var result = await service.Find(cep).ConfigureAwait(false);
                    Assert.True(result.Success);
                }
            }
        }

        [Theory]
        [InlineData("83040-040")]
        [InlineData("80035-020")]
        public async Task CorreioService_Test(string cep)
        {
            using (var httpClient = new HttpClient())
            {
                var service = new CorreiosService(httpClient);

                var result = await service.Find(cep).ConfigureAwait(false);
                Assert.NotNull(result);
            }
        }

        [Theory]
        [InlineData("83040-040")]
        [InlineData("80035-020")]
        public async Task ViaCepService_Test(string cep)
        {
            using (var httpClient = new HttpClient())
            {
                var service = new ViaCepService(httpClient);
                var result = await service.Find(cep).ConfigureAwait(false);
                Assert.NotNull(result);
            }
        }
    }
}
