using EmployeeCRM.Shared.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EmployeeCRM.MVC.Services
{
    public class BaseApiService
    {
        protected readonly HttpClient _httpClient;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public BaseApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        protected void SetAuthHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        protected async Task<T?> GetAsync<T>(string url)
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return default;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        protected async Task<T?> PostAsync<T>(string url, object body)
        {
            SetAuthHeader();
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode) return default;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
        }

        protected async Task<bool> PutAsync(string url, object body)
        {
            SetAuthHeader();
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        protected async Task<T?> PutAsync<T>(string url, object body)
        {
            SetAuthHeader();
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content);
            if (!response.IsSuccessStatusCode) return default;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
        }

        protected async Task<bool> PatchAsync(string url, object body)
        {
            SetAuthHeader();
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        protected async Task<bool> DeleteAsync(string url)
        {
            SetAuthHeader();
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
