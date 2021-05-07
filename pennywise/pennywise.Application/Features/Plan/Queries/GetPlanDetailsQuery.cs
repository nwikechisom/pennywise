using AutoMapper;
using MediatR;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pennywise.Application.Features.Plan.Queries
{
    public class GetPlanDetailsQuery : IRequest<Response<GetPlanDetailsViewModel>>
    {
        public long PlanId { get; set; }
    }

    public class GetPlanDetailsQueryHandler : IRequestHandler<GetPlanDetailsQuery, Response<GetPlanDetailsViewModel>>
    {
        private readonly IPaymentPlanRepository _paymentPlanRepository;
        private readonly IMapper _mapper;

        public GetPlanDetailsQueryHandler(IPaymentPlanRepository paymentPlanRepository, IMapper mapper)
        {
            _paymentPlanRepository = paymentPlanRepository;
            _mapper = mapper;
        }
        public async Task<Response<GetPlanDetailsViewModel>> Handle(GetPlanDetailsQuery request, CancellationToken cancellationToken)
        {
            var plan = await _paymentPlanRepository.GetByIdAsync(request.PlanId);
            var plandetailsviewmodel = _mapper.Map<GetPlanDetailsViewModel>(plan);
            return new Response<GetPlanDetailsViewModel>(plandetailsviewmodel);
        }
    }
}
