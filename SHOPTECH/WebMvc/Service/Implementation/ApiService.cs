using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
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

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            var request = CreateRequestWithAuth(HttpMethod.Get, endpoint);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, object data)
        {
            var request = CreateRequestWithAuth(HttpMethod.Post, endpoint);

            if (data != null)
            {
                if (data is MultipartFormDataContent multipartContent)
                {
                    request.Content = multipartContent;
                }
                else
                {
                    var json = JsonConvert.SerializeObject(data);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
            }

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PutAsync(string endpoint, object data)
        {
            var request = CreateRequestWithAuth(HttpMethod.Put, endpoint);

            if (data != null)
            {
                if (data is MultipartFormDataContent multipartContent)
                {
                    request.Content = multipartContent;
                }
                else
                {
                    var json = JsonConvert.SerializeObject(data);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
            }

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint, object data)
        {
            var request = CreateRequestWithAuth(HttpMethod.Delete, endpoint);

            if (data != null)
            {
                if (data is MultipartFormDataContent multipartContent)
                {
                    request.Content = multipartContent;
                }
                else
                {
                    var json = JsonConvert.SerializeObject(data);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
            }

            return await _httpClient.SendAsync(request);
        }

        private HttpRequestMessage CreateRequestWithAuth(HttpMethod method, string endpoint)
        {
            var request = new HttpRequestMessage(method, endpoint);
            // Tự động thêm JWT từ cookie vào Authorization header
            if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("Jwt", out var token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return request;
        }
    }
}