using Microsoft.EntityFrameworkCore;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using pennywise.Domain.Entities;
using pennywise.Infrastructure.Persistence.Contexts;
using pennywise.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using pennywise.Application.Features.Transact.Commands;
using pennywise.Domain.Enums;

namespace pennywise.Infrastructure.Persistence.Repositories
{
    public class PaymentPlanRepository : GenericRepositoryAsync<PaymentPlan>, IPaymentPlanRepository
    {
        private readonly DbSet<PaymentPlan> _plans;
        private readonly ILogger<PaymentPlanRepository> _logger;
        private readonly ITransactionRepositoryAsync _transactionRepositoryAsync;

        public PaymentPlanRepository(ApplicationDbContext context, ILogger<PaymentPlanRepository> logger,
            ITransactionRepositoryAsync transactionRepositoryAsync) : base(context)
        {
            _plans = context.Set<PaymentPlan>();
            _logger = logger;
            _transactionRepositoryAsync = transactionRepositoryAsync;
        }
        public async Task<Response<object>> CreatePlan(PaymentPlan plan)
        {
            var createdPlan = await _plans.AddAsync(plan);
            return new Response<object>{ Data = createdPlan.Entity.Id, Message = $"{plan.Title} created"};
        }

        public async Task PayoutSchedule()
        {
            var todaysPayouts = this.GetByParameter(_ => _.NextDueDate.Date == DateTime.Today.Date);
            if (todaysPayouts == null || !todaysPayouts.Any())
            {
                _logger.LogInformation("No Payouts", todaysPayouts);
                return;
            }
            foreach (var plan in todaysPayouts)
            {
                await _transactionRepositoryAsync.PayoutTransaction(todaysPayouts.ToList());
            }
        }

        public double CalculateTotalPlanCost(PaymentPlan plan)
        {
            var timesToPay = (plan.EndDate - plan.BeginDate).TotalDays;
            switch (plan.Schedule)
            {
                case PlanSchedule.Daily:
                    return plan.Amount * timesToPay;
                case PlanSchedule.Weekly:
                    timesToPay = timesToPay / 7; //7days in a week
                    return plan.Amount * timesToPay;
                case PlanSchedule.BiWeekly:
                    timesToPay = timesToPay / 14; //14 days in 2weeks
                    return plan.Amount * timesToPay;
                case PlanSchedule.Monthly:
                    timesToPay = GetMonthDifference(plan.BeginDate, plan.EndDate);
                    return plan.Amount * timesToPay;
                case PlanSchedule.Yearly:
                    timesToPay = GetDifferenceInYears(plan.BeginDate, plan.EndDate);
                    return plan.Amount * timesToPay;
                default:
                    return 0;
            }
        }
        
        public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {
            //Excel documentation says "COMPLETE calendar years in between dates"
            int years = endDate.Year - startDate.Year;

            if (startDate.Month == endDate.Month &&// if the start month and the end month are the same
                endDate.Day < startDate.Day// AND the end day is less than the start day
                || endDate.Month < startDate.Month)// OR if the end month is less than the start month
            {
                years--;
            }

            return years;
        }
        private static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
    }
}
