using Microsoft.EntityFrameworkCore;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using pennywise.Domain.Entities;
using pennywise.Infrastructure.Persistence.Contexts;
using pennywise.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pennywise.Infrastructure.Persistence.Repositories
{
    public class PaymentPlanRepository : GenericRepositoryAsync<PaymentPlan>, IPaymentPlanRepository
    {
        private readonly DbSet<PaymentPlan> _plans;

        public PaymentPlanRepository(ApplicationDbContext context) : base(context)
        {
            _plans = context.Set<PaymentPlan>();
        }
        public async Task<Response<object>> CreatePlan(PaymentPlan plan)
        {
            await _plans.AddAsync(plan);
            return new Response<object>();
        }
    }
}
