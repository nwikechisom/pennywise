using pennywise.Application.Interfaces.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace pennywise.Infrastructure.Shared.Services.Http
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private HttpClient Client { get; }

        public HttpClientWrapper(HttpClient client)
        {
            Client = client;
        }

        // this is just to demonstrate a simple reuse technique. you can do it in other ways. (singleton, DI, static)
        //public HttpClient Client => _client ?? (_client = _httpClientFactory.CreateClient("FlutterWaveClient"));

        public Task<T> GetAsync<T>(string uri, Dictionary<string, string> headers = null)
        {
            if(headers!=null)
            {
                foreach (var header in headers)
                {
                    Client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            return InvokeAsync<T>(
               client => client.GetAsync(uri),
               response => response.Content.ReadAsAsync<T>());
        }

        public Task<T> PostAsJsonAsync<T>(object data, string uri, Dictionary<string, string> headers = null)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    Client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            return InvokeAsync<T>(
               client => client.PostAsJsonAsync(uri, data),
               response => response.Content.ReadAsAsync<T>());
        }

        public Task PostAsJsonAsync(object data, string uri, Dictionary<string, string> headers = null)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    Client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            return InvokeAsync<object>(
                client => client.PostAsJsonAsync(uri, data));
        }

        public Task PutAsJsonAsync(object data, string uri, Dictionary<string, string> headers = null)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    Client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            return InvokeAsync<object>(
                client => client.PutAsJsonAsync(uri, data));
        }

        public Task<T> PutAsJsonAsync<T>(object data, string uri, Dictionary<string, string> headers = null)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    Client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            return InvokeAsync<T>(
                client => client.PutAsJsonAsync(uri, data),
                response => response.Content.ReadAsAsync<T>());
        }

        private async Task<T> InvokeAsync<T>(
            Func<HttpClient, Task<HttpResponseMessage>> operation,
            Func<HttpResponseMessage, Task<T>> actionOnResponse = null)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var response = await operation(Client).ConfigureAwait(false);

            //if (!response.IsSuccessStatusCode)
            //{
            //    var exception = new Exception($"Resource server returned an error. StatusCode : {response.StatusCode}");
            //    exception.Data.Add("StatusCode", response.StatusCode);
            //    throw exception;
            //}
            if (actionOnResponse != null)
            {
                return await actionOnResponse(response).ConfigureAwait(false);
            }
            else
            {
                return default(T);
            }
        }
    }
}
