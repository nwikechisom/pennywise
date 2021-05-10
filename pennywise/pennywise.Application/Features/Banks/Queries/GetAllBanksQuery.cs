using AutoMapper;
using MediatR;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pennywise.Application.Features.Banks.Queries
{
    public class GetAllBanksQuery : IRequest<Response<IEnumerable<GetAllBanksViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllBanksQueryHandler : IRequestHandler<GetAllBanksQuery, Response<IEnumerable<GetAllBanksViewModel>>>
    {
        private readonly IBankRepositoryAsync _bankRepository;
        private readonly IMapper _mapper;

        public GetAllBanksQueryHandler(IBankRepositoryAsync bankRepository, IMapper mapper)
        {
            _bankRepository = bankRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<GetAllBanksViewModel>>> Handle(GetAllBanksQuery request, CancellationToken cancellationToken)
        {
            var banks = await _bankRepository.GetAllAsync();
            var banksViewModel = _mapper.Map<IEnumerable<GetAllBanksViewModel>>(banks);
            return new Response<IEnumerable<GetAllBanksViewModel>> { Data = banksViewModel, Succeeded = true, Message = "All banks retrieved successfully" };
        }
    }
}
