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

        public CreateBankDetailCommandHandler(IAuthenticatedUserService authenticatedUserService,
            IBankDetailRepositoryAsync bankDetailRepositoryAsync, IMapper mapper)
        {
            _authenticatedUserService = authenticatedUserService;
            _bankDetailRepositoryAsync = bankDetailRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(CreateBankDetailCommand request, CancellationToken cancellationToken)
        {
            var bankdetail = _mapper.Map<BankDetail>(request);
            bankdetail.UserId = _authenticatedUserService.UserId;
            var addedDetail = await _bankDetailRepositoryAsync.AddAsync(bankdetail);
            return new Response<string> { Succeeded = true, Message = "Bank details added succesfully", Data = addedDetail.Id.ToString()  };
        }
    }
}
