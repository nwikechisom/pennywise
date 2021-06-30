using System;
using System.Linq;
using System.Threading.Tasks;
using pennywise.Application.Interfaces.Repositories;

namespace pennywise.Application.CronJobs
{
    public class PayoutCronJob
    {
        private readonly IPaymentPlanRepository _planRepository;
        private readonly ITransactionRepositoryAsync _transactionRepositoryAsync;

        public PayoutCronJob(IPaymentPlanRepository planRepository, ITransactionRepositoryAsync transactionRepositoryAsync)
        {
            _planRepository = planRepository;
            _transactionRepositoryAsync = transactionRepositoryAsync;
        }

        public async Task Payout()
        {
            var duePlans = _planRepository.GetByParameter(x => x.NextDueDate == DateTime.Today);
            if (duePlans != null && duePlans.Any())
            {
                await _transactionRepositoryAsync.PayoutTransaction(duePlans.ToList());
            }
        }
    }
}