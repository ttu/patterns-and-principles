using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PatternsAndPinciples.Patterns.GoF.Structural
{
    /*
     * Provide a surrogate or placeholder for another object to control access to it.
     * The purpose of the proxy pattern is to create a stand-in for a real resource.
     */

    public interface IHttpRequester
    {
        Task<string> GetAsync(string url);
    }

    public class HttpRequester : IHttpRequester
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<string> GetAsync(string url)
        {
            var result = await _client.GetAsync(url);
            return await result.Content.ReadAsStringAsync();
        }
    }

    public class Proxy : IHttpRequester
    {
        private readonly Dictionary<string, string> _cache = new Dictionary<string, string>();
        private readonly HttpRequester _subject;

        public Proxy(HttpRequester subject) => _subject = subject;

        public async Task<string> GetAsync(string url)
        {
            if (!_cache.TryGetValue(url, out string result))
            {
                result = await _subject.GetAsync(url);
                _cache.Add(url, result);
            }

            return result;
        }
    }

    public class ProxyTest
    {
        [Fact]
        public async Task Proxy()
        {
            var httpRequest = new Proxy(new HttpRequester());
            var result = await httpRequest.GetAsync("http://www.google.com");
            var result2 = await httpRequest.GetAsync("http://www.google.com");

            Assert.Equal(result, result2);
        }
    }
}