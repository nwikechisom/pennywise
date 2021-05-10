using MediatR;
using pennywise.Application.Exceptions;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using pennywise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pennywise.Application.Features.BankDetails.Queries
{
    public class GetBankDetailByIdQuery : IRequest<Response<BankDetail>>
    {
        public long Id { get; set; }
        public class GetBankDetailByIdQueryHandler : IRequestHandler<GetBankDetailByIdQuery, Response<BankDetail>>
        {
            private readonly IBankDetailRepositoryAsync _bankDetailRepository;
            public GetBankDetailByIdQueryHandler(IBankDetailRepositoryAsync bankDetailRepository)
            {
                _bankDetailRepository = bankDetailRepository;
            }
            public async Task<Response<BankDetail>> Handle(GetBankDetailByIdQuery query, CancellationToken cancellationToken)
            {
                var bankDetail = await _bankDetailRepository.GetByIdAsync(query.Id);
                if (bankDetail == null) throw new ApiException($"Bank detail not found.");
                return new Response<BankDetail>(bankDetail);
            }
        }
    }
}
