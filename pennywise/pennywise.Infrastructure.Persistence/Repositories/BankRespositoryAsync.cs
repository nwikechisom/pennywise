using Microsoft.EntityFrameworkCore;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Domain.Entities;
using pennywise.Infrastructure.Persistence.Contexts;
using pennywise.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Infrastructure.Persistence.Repositories
{
    public class BankRespositoryAsync : GenericRepositoryAsync<Bank>, IBankRepositoryAsync
    {
        private readonly DbSet<Bank> _banks;

        public BankRespositoryAsync(ApplicationDbContext context) : base(context)
        {
            _banks = context.Set<Bank>();
        }
    }
}
