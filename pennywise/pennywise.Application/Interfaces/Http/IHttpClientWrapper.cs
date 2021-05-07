using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pennywise.Application.Interfaces.Http
{
    public interface IHttpClientWrapper
    {
        Task<T> GetAsync<T>(string uri, Dictionary<string, string> headers = null);
        Task PostAsJsonAsync(object data, string uri, Dictionary<string, string> headers = null);
        Task<T> PostAsJsonAsync<T>(object data, string uri, Dictionary<string, string> headers = null);
        Task PutAsJsonAsync(object data, string uri, Dictionary<string, string> headers = null);
        Task<T> PutAsJsonAsync<T>(object data, string uri, Dictionary<string, string> headers = null);
    }
}
