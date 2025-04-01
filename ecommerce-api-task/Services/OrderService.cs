using System.Net.Http.Headers;

namespace ecommerce_api_task.Services
{
    public class OrderService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

    
        public async Task<string> GetOrdersAsync()
        {
            string token = await TokenManager.GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var ordersUrl = "http://example.com/api/orders"; // Deneme api'i
            var response = await _httpClient.GetAsync(ordersUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Siparişler alınamadı: " + response.StatusCode);
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
