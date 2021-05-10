using MediatR;
using pennywise.Application.Interfaces;
using pennywise.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pennywise.Application.Features.Banks.Commands
{
    public class ResolveAccountNumberCommand : IRequest<Response<string>>
    {
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
    }

    public class ValidateAccountNumberCommandHandler : IRequestHandler<ResolveAccountNumberCommand, Response<string>>
    {
        private readonly IPaymentService _paymentService;

        public ValidateAccountNumberCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<Response<string>> Handle(ResolveAccountNumberCommand request, CancellationToken cancellationToken)
        {
            var resolveAccountResponse = await _paymentService.ResolveAccountNumber(request.AccountNumber, request.BankCode);
            if (resolveAccountResponse.status && resolveAccountResponse.data != null)
                return new Response<string> { Data = resolveAccountResponse.data.AccountName, Succeeded = true, Message = "Account name retrieved successfully" };
            return new Response<string> { Message = resolveAccountResponse.message, Succeeded = false };
        }
    }
}
