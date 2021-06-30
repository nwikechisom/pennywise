using pennywise.Application.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pennywise.Application.Interfaces
{
    public interface IPaymentService
    {
        public Task<PaystackBaseResponse<InitiatePaymentResponse>> InitializePaystackPayment(InitiatePaymentRequest request);
        public Task<PaystackBaseResponse<VerifyPaymentResponse>> VerifyPayment(string reference);
        public Task<PaystackBaseResponse<ResolveAccountResponse>> ResolveAccountNumber(string accountNumber, string bankCode);
        public Task<PaystackBaseResponse<CreateRecipientResponse>> CreateRecipient(CreateRecipientRequest request);
        public Task<PaystackBaseResponse<SingleTransferResponse>> InitiateSingleTransfer(SingleTransferRequest request);
        public Task<PaystackBaseResponse<BulkTransferResponse>> InitiateBulkTransfer(BulkTransferRequest request);
    }
}
