using System.Threading.Tasks;
using Sirb.CepBrasil_Standard.Models;

namespace Sirb.CepBrasil_Standard.Interfaces
{
    internal interface ICepServiceControl
    {
        /// <summary>
        /// Find location by zip code. Internal usage intended.
        /// </summary>
        /// <param name="cep"></param>
        Task<CepContainer> Find(string cep);
    }
}
