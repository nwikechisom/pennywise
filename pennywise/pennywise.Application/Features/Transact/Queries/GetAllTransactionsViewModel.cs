using pennywise.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Application.Features.Transact.Queries
{
    public class GetAllTransactionsViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string TransactionMethod { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Reference { get; set; }
    }
}
