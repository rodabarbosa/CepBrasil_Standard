using Newtonsoft.Json;

namespace Sirb.CepBrasil.Models
{
	/// <summary>
	/// Container para o resultado da busca do CEP
	/// </summary>
	public sealed class CepContainer
	{
		/// <summary>
		/// State abbreviation
		///
		/// Sigla do estado
		/// </summary>
		[JsonProperty("uf")]
		public string Uf { get; set; }

		/// <summary>
		/// City name
		///
		/// Nome da cidade
		/// </summary>
		[JsonProperty("localidade")]
		public string Cidade { get; set; }

		/// <summary>
		/// Neighborhood name
		///
		/// Nome do bairro
		/// </summary>
		[JsonProperty("bairro")]
		public string Bairro { get; set; }

		/// <summary>
		/// Extra information
		///
		/// Complemento
		/// </summary>
		[JsonProperty("complemento")]
		public string Complemento { get; set; }

		/// <summary>
		/// Street name
		///
		/// Logradouro
		/// </summary>
		[JsonProperty("logradouro")]
		public string Logradouro { get; set; }

		/// <summary>
		/// Zip code
		///
		/// CEP
		/// </summary>
		[JsonProperty("cep")]
		public string Cep { get; set; }
	}
}