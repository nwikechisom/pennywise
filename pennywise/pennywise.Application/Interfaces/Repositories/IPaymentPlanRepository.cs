using pennywise.Application.Wrappers;
using pennywise.Domain.Entities;
using System.Threading.Tasks;

namespace pennywise.Application.Interfaces.Repositories
{
    public interface IPaymentPlanRepository : IGenericRepositoryAsync<PaymentPlan>
    {
        Task<Response<object>> CreatePlan(PaymentPlan plan);
    }
}
