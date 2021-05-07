using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using pennywise.Infrastructure.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pennywise.Infrastructure.Shared.Services.Http
{
    public class HttpClientLoggingHandler : DelegatingHandler
    {
        private readonly ILogger<HttpClientLoggingHandler> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public HttpClientLoggingHandler(ILogger<HttpClientLoggingHandler> logger, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponseMessage = default;

            try
            {
                if (_hostEnvironment.IsDevelopment() && request.Content != null)
                {
                    _logger.LogInformation(CustomEventIds.SendItem, await request.Content.ReadAsStringAsync());
                }

                //var sw = Stopwatch.StartNew();
                _logger.LogInformation("Starting request");
                httpResponseMessage = await base.SendAsync(request, cancellationToken);
                //_logger.LogInformation($"Finished request in {sw.ElapsedMilliseconds}ms");
                httpResponseMessage.EnsureSuccessStatusCode();

                if (_hostEnvironment.IsDevelopment())
                {
                    if (httpResponseMessage != null && httpResponseMessage.Content != null)
                    {
                        _logger.LogInformation(CustomEventIds.Exception, await httpResponseMessage.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                if (httpResponseMessage != null && httpResponseMessage.Content != null)
                {
                    _logger.LogInformation(CustomEventIds.Exception, await httpResponseMessage.Content.ReadAsStringAsync());
                }

                _logger.LogError(ex, "Failed to run http query {RequestUri}", request.RequestUri);
            }

            return httpResponseMessage;
        }
    }
}
