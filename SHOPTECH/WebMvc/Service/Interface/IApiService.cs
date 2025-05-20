using System.Net.Http;

namespace WebMvc.Service.Interface
{
    public interface IApiService
    {
        public Task<HttpResponseMessage> GetAsync(string endpoint);
        public Task<HttpResponseMessage> PostAsync(string endpoint, object data);
        public Task<HttpResponseMessage> PutAsync(string endpoint, object data);
        public Task<HttpResponseMessage> DeleteAsync(string endpoint, object data);
    }
}
