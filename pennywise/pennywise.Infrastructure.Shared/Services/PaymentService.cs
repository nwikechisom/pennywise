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
            var headers = new Dictionary<string, string> {{"Authorization", $"Bearer {token}"}};
            var response = await _httpClient.PostAsJsonAsync<PaystackBaseResponse<InitiatePaymentResponse>>
                (request, $"{_paystackSettings.BaseUrl}{_paystackSettings.InitializeTransactionUrl}", headers);
            return response;
        }

        public async Task<PaystackBaseResponse<ResolveAccountResponse>> ResolveAccountNumber(string accountNumber, string bankCode)
        {
            var token = _externalAuthService.GetPaystackToken();
            var headers = new Dictionary<string, string> {{"Authorization", $"Bearer {token}"}};
            var response = await _httpClient.GetAsync<PaystackBaseResponse<ResolveAccountResponse>>
                ($"{_paystackSettings.BaseUrl}{_paystackSettings.ResolveAccountUrl}?account_number={accountNumber}&bank_code={bankCode}", headers);
            return response;
        }

        public async Task<PaystackBaseResponse<CreateRecipientResponse>> CreateRecipient(CreateRecipientRequest request)
        {
            var token = _externalAuthService.GetPaystackToken();
            var headers = new Dictionary<string, string>();
            headers.Add("Authorization", $"Bearer {token}");
            var response = await _httpClient.PostAsJsonAsync<PaystackBaseResponse<CreateRecipientResponse>>
                (request, $"{_paystackSettings.BaseUrl}{_paystackSettings.CreateTransferRecipientUrl}", headers);
            return response;
        }

        public async Task<PaystackBaseResponse<SingleTransferResponse>> InitiateSingleTransfer(SingleTransferRequest request)
        {
            var token = _externalAuthService.GetPaystackToken();
            var headers = new Dictionary<string, string> {{"Authorization", $"Bearer {token}"}};
            var response = await _httpClient.PostAsJsonAsync<PaystackBaseResponse<SingleTransferResponse>>
                (request, $"{_paystackSettings.BaseUrl}{_paystackSettings.SingleTransferUrl}", headers);
            return response;
        }

        public async Task<PaystackBaseResponse<BulkTransferResponse>> InitiateBulkTransfer(BulkTransferRequest request)
        {
            var token = _externalAuthService.GetPaystackToken();
            var headers = new Dictionary<string, string> {{"Authorization", $"Bearer {token}"}};
            var response = await _httpClient.PostAsJsonAsync<PaystackBaseResponse<BulkTransferResponse>>
                (request, $"{_paystackSettings.BaseUrl}{_paystackSettings.BulkTransferUrl}", headers);
            return response;
        }

        public async Task<PaystackBaseResponse<VerifyPaymentResponse>> VerifyPayment(string reference)
        {
            var token = _externalAuthService.GetPaystackToken();
            var headers = new Dictionary<string, string> {{"Authorization", $"Bearer {token}"}};
            var response = await _httpClient.GetAsync<PaystackBaseResponse<VerifyPaymentResponse>>
                ($"{_paystackSettings.BaseUrl}{_paystackSettings.VerifyTransactionUrl}/{reference}", headers);
            return response;
        }
    }
}
