using System.Threading.Tasks;
using Sirb.CepBrasil_Standard.Models;

namespace Sirb.CepBrasil_Standard.Interfaces
{
    public interface ICepService
    {
        /// <summary>
        /// Find location by zip code. Internal usage intended.
        /// Método para buscar Logradouro por CEP
        /// </summary>
        /// <param name="cep"></param>
        Task<CepResult> Find(string cep);
    }
}
