using System.Text.Json;

namespace Porfolio.Code
{
    public class Helper
    {
    }

    public static class HelperExtensions
    {

        public static JsonSerializerOptions SetJsonSerializerOptions(this JsonSerializerOptions value)
        {
            value = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default,
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = false,

            };

            return value;
        }


        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
