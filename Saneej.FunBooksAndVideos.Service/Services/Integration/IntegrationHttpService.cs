using Newtonsoft.Json;
using System.Text;

namespace Saneej.FunBooksAndVideos.Service.Services.Integration
{
    public class IntegrationHttpService : IIntegrationHttpService
    {
        private HttpClient _httpClient;
        public IntegrationHttpService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        public async Task<T> PostAsync<T>(string url, object body, string bearerToken = null) where T : class
        {
            // TODO :  Add Authentication Header
            var response = await _httpClient.PostAsync(url, SerializeBody(body));
            return await DeserializeResponse<T>(response);
        }

        public async Task<T> PutAsync<T>(string url, object body, string bearerToken = null) where T : class
        {
            // TODO :  Add Authentication Header
            var response = await _httpClient.PutAsync(url, SerializeBody(body));
            return await DeserializeResponse<T>(response);
        }

        public async Task<T> GetAsync<T>(string url, string bearerToken = null) where T : class
        {
            // TODO :  Add Authentication Header
            var response = await _httpClient.GetAsync(url);
            return await DeserializeResponse<T>(response);
        }

        private StringContent SerializeBody(object body)
        {
            if (body is null)
                throw new ArgumentNullException("Body is required");

            var json = JsonConvert.SerializeObject(body);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var deserializedObject = JsonConvert.DeserializeObject<T>(content);
            return deserializedObject;
        }
    }
}
