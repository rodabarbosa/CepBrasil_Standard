using Newtonsoft.Json;

namespace Sirb.CepBrasil_Standard.Extensions
{
	public static class JsonExtension
	{
		private static JsonSerializerSettings _settings;

		/// <summary>
		/// Convert object to JSON formatted.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToJson(this object value, JsonSerializerSettings settings = null) => JsonConvert.SerializeObject(value, settings ?? Settings());

		/// <summary>
		/// Convert JSON string to especified class type.
		/// </summary>
		/// <typeparam name="T">Convert to</typeparam>
		/// <param name="value">JSON string</param>
		/// <returns></returns>
		public static T FromJson<T>(this string value, JsonSerializerSettings settings = null) where T : class => JsonConvert.DeserializeObject<T>(value, settings ?? Settings());

		private static JsonSerializerSettings Settings()
		{
			if (_settings == null)
				_settings = new JsonSerializerSettings();

			return _settings;
		}
	}
}