using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PatternsAndPinciples.Patterns.GoF.Structural
{
    /*
     * Wraps an object to control access to it
     *
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
            async Task<string> GetGoogle(IHttpRequester http)
            {
                return await http.GetAsync("http://www.google.com");
            }

            var httpRequest = new HttpRequester();
            var proxy = new Proxy(httpRequest);

            var result = await GetGoogle(proxy);
            var result2 = await GetGoogle(proxy);
            var result3 = await GetGoogle(httpRequest);

            Assert.Same(result, result2);
            Assert.NotSame(result, result3);
        }
    }
}