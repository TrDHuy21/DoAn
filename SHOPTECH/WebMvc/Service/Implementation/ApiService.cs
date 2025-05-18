using WebMvc.Service.Interface;

namespace WebMvc.Service.Implementation
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<T> GetFromApiAsync<T>(string endpoint)
        {
            var request = CreateRequestWithAuth(HttpMethod.Get, endpoint);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T> PostToApiAsync<T>(string endpoint, object data)
        {
            var request = CreateRequestWithAuth(HttpMethod.Post, endpoint);
            request.Content = JsonContent.Create(data);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        private HttpRequestMessage CreateRequestWithAuth(HttpMethod method, string endpoint)
        {
            var request = new HttpRequestMessage(method, endpoint);

            // Tự động thêm JWT từ cookie vào Authorization header
            if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("JwtToken", out var token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return request;
        }
    }
}
