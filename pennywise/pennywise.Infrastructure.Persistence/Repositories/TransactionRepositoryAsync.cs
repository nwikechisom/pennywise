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
using pennywise.Domain.Enums;

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
            var transaction = await this.GetByParameter(x => x.Reference == transactionReference).FirstOrDefaultAsync();
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

        public async Task<Response<string>> PayoutTransaction(List<PaymentPlan> request)
        {
            var bulkTransfer = new BulkTransferRequest();
            foreach (var plan in request)   
            {
                foreach (var detail in plan.Receivers)
                {
                    //TODO: see how you can retrieve transaction reference from bulk transfer foreach transaction created.
                    //TODO: it's the reason the block below is commented out.
                    // bulkTransfer.Transfers.Add(
                    //     new BulkTransfer
                    //     {
                    //         Amount = plan.Amount.ToString(),
                    //         Reason = plan.Title,
                    //         Recipient = detail.PaystackRecipientCode
                    //     });
                    var transaction = new Transaction
                    {
                        UserId = plan.UserId,
                        Title = plan.Title,
                        Description = plan.Description,
                        Amount = plan.Amount,
                        TransactionStatus = TransactionStatus.Initiated,
                        TransactionType = TransactionType.Withdrawal,
                        UserPreviousBalance = 0,
                        UserCurrentBalance = 0,
                        PlanId = plan.Id
                    };
                    await this.AddAsync(transaction);
                    var singleTransferRequest = new SingleTransferRequest
                    {
                        //Paystack accepts amount in kobo, please do not change except standards have changed.
                        Amount = (plan.Amount* 100).ToString(),
                        Reason = plan.Title,
                        Recipient = detail.PaystackRecipientCode
                    };
                    var initiateSingleTransfer = 
                        await _paymentService.InitiateSingleTransfer(singleTransferRequest);
                    if (initiateSingleTransfer.status)
                    {
                        transaction.Reference = initiateSingleTransfer.data.Reference;
                        transaction.TransactionStatus = TransactionStatus.Pending;
                        transaction.SuccessfullyInitiated = true;
                    }
                    else
                    {
                        transaction.TransactionStatus = TransactionStatus.Failed;
                    }

                    await this.UpdateAsync(transaction);
                }
                //verify transactions //TODO: leaving this task to webhook or a different cronjob that'll do the check;
                //TODO: this means you'll need to add a flag to each transaction or TransactionStatus might suffice
                
                //send out notifications
                //TODO: do this on success; i.e leave it to the webhook handler
                
                //TODO: Do everything for one plan at a time and save success or failure tied to that plan in a different response body
            }
            
            return new Response<string>("done.");
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
                //paystack accepts amount in kobo, please do not change except standards have changed
                Amount = (transaction.Amount * 100).ToString(), 
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
            transaction.SuccessfullyInitiated = false;
            transaction.InitiateMessage = response.message;
            await this.UpdateAsync(transaction);
            return new Response<InitiatePaymentResponse> { Succeeded = response.status, Message = response.message, };
        }
    }
}
