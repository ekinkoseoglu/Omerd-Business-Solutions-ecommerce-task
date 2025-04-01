using ecommerce_api_task.Entities;
using Newtonsoft.Json;

namespace ecommerce_api_task.Services
{
    public class TokenManager
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static TokenResponse _cachedToken = null;
        private static readonly object _lock = new object();

        public static async Task<string> GetAccessTokenAsync()
        {
            if (_cachedToken != null && _cachedToken.Expiry > DateTime.UtcNow.AddSeconds(60))
            {
                return _cachedToken.access_token;
            }

            lock (_lock)
            {
                if (_cachedToken != null && _cachedToken.Expiry > DateTime.UtcNow.AddSeconds(60))
                {
                    return _cachedToken.access_token;
                }
            }

            var tokenUrl = "http://example.com/api/token"; // Deneme api'i
            var requestContent = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("client_id", "your_client_id"),
            new KeyValuePair<string, string>("client_secret", "your_client_secret"),
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

            HttpResponseMessage response = await _httpClient.PostAsync(tokenUrl, requestContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Token alınamadı: " + response.StatusCode);
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonContent);

            tokenResponse.Expiry = DateTime.UtcNow.AddSeconds(tokenResponse.expires_in);

            lock (_lock)
            {
                _cachedToken = tokenResponse;
            }
            return tokenResponse.access_token;
        }
    }
}
