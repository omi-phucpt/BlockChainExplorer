using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

namespace BlockchainExplorer.Common
{
    public class OkxApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "4267a8c7-b0dd-4066-8540-620980e306bd";
        private readonly string _projectId = "efdec8c9624e1ee58d9776e51008a4fd";
        private readonly string _passphrase = "Nghiacho2612ab@.";
        private readonly string _secretKey = "D2D6964D6A9882EFF1CC3CD24C4DBC3E";

        public OkxApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://web3.okx.com")
            };
        }

        public async Task<JsonElement> GetApiDataAsync(string url, 
            Dictionary<string, string> queryParams = null, 
            string method = "GET", string body = "")
        {
            try
            {
                var queryString = queryParams != null ? BuildQueryString(queryParams) : "";
                var requestPath = new Uri(url, UriKind.Absolute).PathAndQuery;
                if (!string.IsNullOrEmpty(queryString))
                {
                    requestPath += $"?{queryString}";
                }

                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                var signature = GenerateSignature(timestamp, method, requestPath, body);

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-PROJECT", _projectId);
                _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-KEY", _apiKey);
                _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-SIGN", signature);
                _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-TIMESTAMP", timestamp);
                _httpClient.DefaultRequestHeaders.Add("OK-ACCESS-PASSPHRASE", _passphrase);

                var response = string.Empty;
                switch (method.ToUpper())
                {
                    case "GET":
                        response = await _httpClient.GetStringAsync(requestPath);
                        break;
                    case "POST":
                        var content = new StringContent(body, Encoding.UTF8, "application/json");
                        var httpResponse = await _httpClient.PostAsync(requestPath, content).ConfigureAwait(false);
                        response = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        break;
                }

                using var document = JsonDocument.Parse(response);
                var root = document.RootElement;

                //if (root.ValueKind == JsonValueKind.Object)
                //{
                //    if (root.TryGetProperty("data", out var dataElement))
                //    {
                //        if (dataElement.ToString() == "[]")
                //        {
                //            return root.TryGetProperty("msg", out var msg) ? msg : throw new JsonException("Field 'data' not found.");
                //        }
                //        else
                //        {
                //            if (dataElement.ValueKind == JsonValueKind.Array)
                //            {
                //                return dataElement.Clone();
                //            }
                //            else
                //            {
                //                throw new JsonException("Field 'data' không phải mảng.");
                //            }
                //        }
                //    }
                //    throw new JsonException("Field 'data' không phải mảng.");
                //}
                //else
                //{
                //    throw new JsonException("The 'data' field is missing or not an object.");
                //}
                if (root.ValueKind != JsonValueKind.Object)
                    throw new JsonException("The 'data' field is missing or not an object.");

                if (!root.TryGetProperty("data", out var dataElement))
                    throw new JsonException("Field 'data' not found.");

                return dataElement.ToString() == "[]"
                    ? root.TryGetProperty("msg", out var msg) ? msg : throw new JsonException("Field 'msg' not found.")
                    : dataElement.ValueKind == JsonValueKind.Array
                        ? dataElement.Clone()
                        : throw new JsonException("Field 'data' not array.");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to fetch data: {ex.Message}", ex);
            }
        }

        private string BuildQueryString(Dictionary<string, string> queryParams)
        {
            var query = new List<string>();
            foreach (var param in queryParams)
            {
                if (!string.IsNullOrEmpty(param.Value))
                {
                    query.Add($"accountId={HttpUtility.UrlEncode(param.Value)}");
                }
            }
            
            if (queryParams["chains"] == "")
            {
                query.Add($"chains=1");
            }

            return string.Join("&", query);
        }
        private string GenerateSignature(string timestamp, string method, string requestPath, string body)
        {
            var message = method == "GET" ? $"{timestamp}{method}{requestPath}" : $"{timestamp}{method}{requestPath}{body}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));

            return Convert.ToBase64String(hash);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
