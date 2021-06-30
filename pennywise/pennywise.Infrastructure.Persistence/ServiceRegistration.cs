using pennywise.Application.Interfaces;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Infrastructure.Persistence.Contexts;
using pennywise.Infrastructure.Persistence.Repositories;
using pennywise.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;

namespace pennywise.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region ApplicationDbConnection&HangfireDbConnection
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));
                services.AddHangfire(options => options
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseMemoryStorage());
                // Add the processing server as IHostedService
                services.AddHangfireServer();
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
                services.AddHangfire(options => options
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));
                // Add the processing server as IHostedService
                services.AddHangfireServer();
            }
            #endregion
            
            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IProductRepositoryAsync, ProductRepositoryAsync>();
            services.AddTransient<ITransactionRepositoryAsync, TransactionRepositoryAsync>();
            services.AddTransient<IPaymentPlanRepository, PaymentPlanRepository>();
            services.AddTransient<IBankDetailRepositoryAsync, BankDetailRepositoryAsync>();
            services.AddTransient<IBankRepositoryAsync, BankRespositoryAsync>();
            #endregion
        }
    }
}
