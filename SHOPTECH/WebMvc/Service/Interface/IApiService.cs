using System.Net.Http;

namespace WebMvc.Service.Interface
{
    public interface IApiService
    {
        public Task<T> GetFromApiAsync<T>(string endpoint);
        public Task<T> PostToApiAsync<T>(string endpoint, object data);
    }
}
