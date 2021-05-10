using AutoMapper;
using MediatR;
using pennywise.Application.Interfaces;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using pennywise.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pennywise.Application.Features.BankDetails.Queries
{
    public class GetAllBankDetailsQuery : IRequest<PagedResponse<IEnumerable<GetAllBankDetailsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public BankOwnershipType BankOwnershipType { get; set; }
    }
    public class GetAllBankDetailsQueryHandler : IRequestHandler<GetAllBankDetailsQuery, PagedResponse<IEnumerable<GetAllBankDetailsViewModel>>>
    {
        private readonly IBankDetailRepositoryAsync _bankDetailRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public GetAllBankDetailsQueryHandler(IBankDetailRepositoryAsync bankDetailRepository, IMapper mapper,
            IAuthenticatedUserService authenticatedUserService)
        {
            _bankDetailRepository = bankDetailRepository;
            _mapper = mapper;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<PagedResponse<IEnumerable<GetAllBankDetailsViewModel>>> Handle(GetAllBankDetailsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllBankDetailsParameter>(request);
            var bankDetails = await _bankDetailRepository.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var bankDetailsList = bankDetails.Where(_ => _.BankOwnershipType == request.BankOwnershipType && _.UserId == _authenticatedUserService.UserId);
            var bankdetailViewModel = _mapper.Map<IEnumerable<GetAllBankDetailsViewModel>>(bankDetailsList);
            return new PagedResponse<IEnumerable<GetAllBankDetailsViewModel>>(bankdetailViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
