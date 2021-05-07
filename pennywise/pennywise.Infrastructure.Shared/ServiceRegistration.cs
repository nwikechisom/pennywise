using pennywise.Application.Interfaces;
using pennywise.Domain.Settings;
using pennywise.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pennywise.Application.Interfaces.Http;
using pennywise.Infrastructure.Shared.Services.Http;
using System.Net.Http.Headers;

namespace pennywise.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.Configure<PaystackSettings>(_config.GetSection("PaystackSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IExternalAuthService, ExternalAuthService>();

            services.AddTransient<HttpClientLoggingHandler>();
            services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>(c =>
            {
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddHttpMessageHandler<HttpClientLoggingHandler>();
        }
    }
}
