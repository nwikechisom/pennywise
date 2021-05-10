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
    public class BankDetailRepositoryAsync : GenericRepositoryAsync<BankDetail>, IBankDetailRepositoryAsync
    {
        private DbSet<BankDetail> _bankDetails;

        public BankDetailRepositoryAsync(ApplicationDbContext context) : base(context)
        {
            _bankDetails = context.Set<BankDetail>();
        }
    }
}
