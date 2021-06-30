using System;
using pennywise.Application.Interfaces.Repositories;

namespace pennywise.Infrastructure.Persistence.BgJobs
{
    public class PaymentPlanJob
    {
        private readonly IPaymentPlanRepository _planRepository;

        public PaymentPlanJob(IPaymentPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }
        public void Payout()
        {
            throw new NotImplementedException();
        }
    }
}