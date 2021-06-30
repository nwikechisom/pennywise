using Hangfire;
using Microsoft.AspNetCore.Builder;
using pennywise.Infrastructure.Persistence.BgJobs;

namespace pennywise.Infrastructure.Persistence
{
    public static class AppRegistration
    {
        public static void UseHangfireExtension(this IApplicationBuilder app)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            Hangfire.RecurringJob.AddOrUpdate<PaymentPlanJob>(x => x.Payout(), Cron.Daily);
        }
    }
}