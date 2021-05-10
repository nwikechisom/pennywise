using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Application.Features.BankDetails.Queries
{
    public class GetAllBankDetailsViewModel
    {
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string UserId { get; set; }
    }
}
