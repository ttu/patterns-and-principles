using System.Net.Http;
using System.Threading.Tasks;

namespace PatternsAndPractices.UML
{
    // Realization / Implementation

    public interface IHttpClient
    {
        Task<HttpResponseMessage> SendData(string data);
    }

    public class MyHttpClient : IHttpClient
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _url;

        public MyHttpClient(string url) => _url = url;

        public async Task<HttpResponseMessage> SendData(string data)
        {
            return await _client.PostAsync(_url, new StringContent(data));
        }
    }
}