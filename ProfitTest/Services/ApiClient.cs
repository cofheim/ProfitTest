using Newtonsoft.Json;
using ProfitTest.Contracts.Requests.Auth;
using ProfitTest.Contracts.Responses.Auth;
using ProfitTest.Contracts.Requests.Products;
using ProfitTest.Contracts.Responses.Products;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProfitTest.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5006"; // TODO: Move to config

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{BaseUrl}/api/auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка входа: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LoginResponse>(responseContent);
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{BaseUrl}/api/auth/register", content);

            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/api/products");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var productList = JsonConvert.DeserializeObject<ProductListResponse>(responseContent);
            return productList.Items;
        }

        public async Task<IEnumerable<ProductResponse>> SearchByNameAsync(string nameQuery)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/api/products/search?nameQuery={nameQuery}");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var productList = JsonConvert.DeserializeObject<ProductListResponse>(responseContent);
            return productList.Items;
        }

        public async Task<IEnumerable<ProductResponse>> FilterByPeriodAsync(DateTime? start, DateTime? end)
        {
            var utcStart = start?.ToUniversalTime();
            var utcEnd = end?.ToUniversalTime();
            var queryString = $"start={utcStart:o}&end={utcEnd:o}";
            var response = await _httpClient.GetAsync($"{BaseUrl}/api/products/filter?{queryString}");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var productList = JsonConvert.DeserializeObject<ProductListResponse>(responseContent);
            return productList.Items;
        }

        public async Task CreateProductAsync(CreateProductRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{BaseUrl}/api/products", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка при создании товара: {response.StatusCode} - {errorContent}");
            }
        }

        public async Task UpdateProductAsync(Guid id, UpdateProductRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{BaseUrl}/api/products/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/api/products/{id}");
            response.EnsureSuccessStatusCode();
        }

        public void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void ClearAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}