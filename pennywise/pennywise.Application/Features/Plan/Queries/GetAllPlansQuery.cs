using AutoMapper;
using MediatR;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using pennywise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pennywise.Application.Features.Plan.Queries
{
    public class GetAllPlansQuery : IRequest<PagedResponse<IEnumerable<GetAllPlansViewModel>>>
    {
        public string UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllPlansQuery, PagedResponse<IEnumerable<GetAllPlansViewModel>>>
    {
        private readonly IPaymentPlanRepository _paymentPlanRepository;
        private readonly IMapper _mapper;
        public GetAllProductsQueryHandler(IPaymentPlanRepository paymentPlanRepository, IMapper mapper)
        {
            _paymentPlanRepository = paymentPlanRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllPlansViewModel>>> Handle(GetAllPlansQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllPlansParameter>(request);
            var product = await _paymentPlanRepository.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            if (!string.IsNullOrEmpty(request.UserId))
                product = (IReadOnlyList<PaymentPlan>)product.Where(_ => _.UserId == request.UserId);
            var planViewModel = _mapper.Map<IEnumerable<GetAllPlansViewModel>>(product);
            return new PagedResponse<IEnumerable<GetAllPlansViewModel>>(planViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
