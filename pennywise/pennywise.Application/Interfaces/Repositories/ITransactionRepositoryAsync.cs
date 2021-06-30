using pennywise.Application.DTOs.Payment;
using pennywise.Application.Features.Transact.Commands;
using pennywise.Application.Wrappers;
using pennywise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pennywise.Application.Interfaces.Repositories
{
    public interface ITransactionRepositoryAsync : IGenericRepositoryAsync<Transaction>
    {
        Task<Response<InitiatePaymentResponse>> StartNewTransaction(InitiateTransactionCommand request);
        Task<Response<string>> VerifyTransaction(string transactionReference);
        Task<Response<string>> PayoutTransaction(List<PaymentPlan> request);
    }
}
