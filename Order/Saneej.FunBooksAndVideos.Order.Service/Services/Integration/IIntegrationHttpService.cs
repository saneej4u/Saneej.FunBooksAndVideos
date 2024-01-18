using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Service.Services.Integration
{
    public interface IIntegrationHttpService
    {
        Task<T> GetAsync<T>(string url, string bearerToken = null) where T : class;
        Task<T> PostAsync<T>(string url, object body, string bearerToken = null) where T : class;
        Task<T> PutAsync<T>(string url, object body, string bearerToken = null) where T : class;
    }
}
