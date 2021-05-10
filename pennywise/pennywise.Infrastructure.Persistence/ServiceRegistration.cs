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

namespace pennywise.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
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
