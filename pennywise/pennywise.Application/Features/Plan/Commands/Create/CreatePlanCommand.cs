using AutoMapper;
using MediatR;
using pennywise.Application.Interfaces;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using pennywise.Domain.Entities;
using pennywise.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pennywise.Application.Features.Plan.Commands.Create
{
    public class CreatePlanCommand : IRequest<Response<object>>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool isActivated { get; set; }
        public DateTime? ActivationDate { get; set; }
        public double Amount { get; set; }
        public bool IsLockedAmount { get; set; }
        public PlanSchedule Schedule { get; set; }
    }

    public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, Response<object>>
    {
        private readonly IPaymentPlanRepository _planRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public CreatePlanCommandHandler(IPaymentPlanRepository planRepository, IMapper mapper,
            IAuthenticatedUserService authenticatedUser)
        {
            _planRepository = planRepository;
            _mapper = mapper;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<object>> Handle(CreatePlanCommand command, CancellationToken cancellationToken)
        {
            var plan = _mapper.Map<PaymentPlan>(command);
            plan.UserId = _authenticatedUser.UserId;
            if(command.BeginDate == DateTime.Today.Date)
            plan.NextDueDate = command.BeginDate;
            var response = await _planRepository.CreatePlan(plan);
            return response;
        }
    }
}
