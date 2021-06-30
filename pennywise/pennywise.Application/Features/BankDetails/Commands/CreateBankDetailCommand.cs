using AutoMapper;
using MediatR;
using pennywise.Application.Interfaces;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using pennywise.Domain.Entities;
using pennywise.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using pennywise.Application.DTOs.Payment;

namespace pennywise.Application.Features.BankDetails.Commands
{
    public class CreateBankDetailCommand : IRequest<Response<string>>
    {
        [Required]
        public string BankName { get; set; }
        [Required]
        public string BankCode { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string AccountName { get; set; }
        [Required]
        public BankOwnershipType BankOwnershipType { get; set; }
    }
    public class CreateBankDetailCommandHandler : IRequestHandler<CreateBankDetailCommand, Response<string>>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IBankDetailRepositoryAsync _bankDetailRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;


        public CreateBankDetailCommandHandler(IAuthenticatedUserService authenticatedUserService,
            IBankDetailRepositoryAsync bankDetailRepositoryAsync, IMapper mapper,
            IPaymentService paymentService)
        {
            _authenticatedUserService = authenticatedUserService;
            _bankDetailRepositoryAsync = bankDetailRepositoryAsync;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        public async Task<Response<string>> Handle(CreateBankDetailCommand request, CancellationToken cancellationToken)
        {
            var createRecipient = await _paymentService.CreateRecipient(
                new CreateRecipientRequest
                {
                    BankCode = request.BankCode,
                    AccountNumber = request.AccountNumber,
                    Name = request.AccountName
                });
            if (!createRecipient.status)
                return new Response<string> {Succeeded = false, Message = createRecipient.message};
            var bankDetail = _mapper.Map<BankDetail>(request);
            bankDetail.UserId = _authenticatedUserService.UserId;
            bankDetail.PaystackRecipientCode = createRecipient.data.RecipientCode;
            var addedDetail = await _bankDetailRepositoryAsync.AddAsync(bankDetail);
            return new Response<string> { Succeeded = true, Message = $"{createRecipient.message} \nBank details added successfully", Data = addedDetail.Id.ToString() };
        }
    }
}
