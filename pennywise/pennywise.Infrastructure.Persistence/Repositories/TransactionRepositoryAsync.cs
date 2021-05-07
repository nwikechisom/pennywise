using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using pennywise.Application.DTOs.Payment;
using pennywise.Application.Features.Transact.Commands;
using pennywise.Application.Interfaces;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using pennywise.Domain.Entities;
using pennywise.Domain.Settings;
using pennywise.Infrastructure.Persistence.Contexts;
using pennywise.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pennywise.Infrastructure.Persistence.Repositories
{
    public class TransactionRepositoryAsync : GenericRepositoryAsync<Transaction>, ITransactionRepositoryAsync
    {
        private readonly DbSet<Transaction> _transactions;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly IAccountService _accountService;
        private readonly PaystackSettings _paystackSettings;

        public TransactionRepositoryAsync(ApplicationDbContext dbContext, 
            IPaymentService paymentService, IMapper mapper, IOptions<PaystackSettings> paystackSettings,
            IAuthenticatedUserService authenticatedUser, IAccountService accountService) : base(dbContext)
        {
            _transactions = dbContext.Set<Transaction>();
            _paymentService = paymentService;
            _mapper = mapper;
            _paystackSettings = paystackSettings.Value;
            _authenticatedUser = authenticatedUser;
            _accountService = accountService;
        }

        public async Task<Response<string>> VerifyTransaction(string transactionReference)
        {
            var transaction = await this.FindByParameter(x => x.Reference == transactionReference).FirstOrDefaultAsync();
            if (transaction is null)
                return new Response<string> { Succeeded = false, Message = "Invalid transaction reference" };

            var verifyPaymentResponse = await _paymentService.VerifyPayment(transactionReference);
            //TODO: Update Transaction table with details
            transaction.TransactionMethod = verifyPaymentResponse.data?.channel;
            transaction.TransactionStatus = verifyPaymentResponse.status ? Domain.Enums.TransactionStatus.Successful : Domain.Enums.TransactionStatus.Failed;
            transaction.VerificationResponse = verifyPaymentResponse.data != null ? JsonConvert.SerializeObject(verifyPaymentResponse.data) : null;
            await this.UpdateAsync(transaction);
            return new Response<string> { Succeeded = verifyPaymentResponse.status, Message = verifyPaymentResponse.message };
        }

        public async Task<Response<InitiatePaymentResponse>> StartNewTransaction(InitiateTransactionCommand request)
        {
            var retrieveUser = await _accountService.GetUserById(_authenticatedUser.UserId);
            if (!retrieveUser.Succeeded || retrieveUser.Data is null)
                return new Response<InitiatePaymentResponse> { Succeeded = false, Message = retrieveUser.Message, Errors = retrieveUser.Errors };

            var transaction = _mapper.Map<Transaction>(request);
            transaction.UserId = _authenticatedUser.UserId;
            transaction.Reference = Guid.NewGuid().ToString().Replace("-", string.Empty);
            transaction.TransactionStatus = Domain.Enums.TransactionStatus.Initiated;
            await this.AddAsync(transaction);
            var paymentRequest = new InitiatePaymentRequest
            {
                Amount = (transaction.Amount * 100).ToString(), //toKobo,
                Email = retrieveUser.Data.Email,
                Reference = transaction.Reference,
                Callback_Url = $"{_paystackSettings.PaymentCallback}"

            };
            var response = await _paymentService.InitializePaystackPayment(paymentRequest);
            if (response.status && response.data != null)
            {
                transaction.SuccessfullyInitiated = true;
                transaction.InitiateMessage = response.message;
                await this.UpdateAsync(transaction);
                return new Response<InitiatePaymentResponse>
                {
                    Data = response.data,
                    Message = response.message,
                    Succeeded = response.status
                };
            }
            else
            {
                transaction.SuccessfullyInitiated = false;
                transaction.InitiateMessage = response.message;
                await this.UpdateAsync(transaction);
                return new Response<InitiatePaymentResponse> { Succeeded = response.status, Message = response.message, };
            }
        }
    }
}
