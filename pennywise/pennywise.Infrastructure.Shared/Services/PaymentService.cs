using Microsoft.Extensions.Options;
using pennywise.Application.DTOs.Payment;
using pennywise.Application.Interfaces;
using pennywise.Application.Interfaces.Http;
using pennywise.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pennywise.Infrastructure.Shared.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IExternalAuthService _externalAuthService;
        private readonly IHttpClientWrapper _httpClient;
        private readonly PaystackSettings _paystackSettings;

        public PaymentService(IExternalAuthService externalAuthService, 
            IHttpClientWrapper httpClient, IOptions<PaystackSettings> paystackSettings)
        {
            _externalAuthService = externalAuthService;
            _httpClient = httpClient;
            _paystackSettings = paystackSettings.Value;
        }
        public async Task<PaystackBaseResponse<InitiatePaymentResponse>> InitializePaystackPayment(InitiatePaymentRequest request)
        {
            var token = _externalAuthService.GetPaystackToken();
            var headers = new Dictionary<string, string>();
            headers.Add("Authorization", $"Bearer {token}");
            var response = await _httpClient.PostAsJsonAsync<PaystackBaseResponse<InitiatePaymentResponse>>
                (request, $"{_paystackSettings.BaseUrl}{_paystackSettings.InitializeTransactionUrl}", headers);
            return response;
        }

        public async Task<PaystackBaseResponse<VerifyPaymentResponse>> VerifyPayment(string reference)
        {
            var token = _externalAuthService.GetPaystackToken();
            var headers = new Dictionary<string, string>();
            headers.Add("Authorization", $"Bearer {token}");
            var response = await _httpClient.GetAsync<PaystackBaseResponse<VerifyPaymentResponse>>
                ($"{_paystackSettings.BaseUrl}{_paystackSettings.VerifyTransactionUrl}/{reference}", headers);
            return response;
        }
    }
}
