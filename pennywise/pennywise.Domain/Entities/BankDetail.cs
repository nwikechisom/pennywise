using pennywise.Domain.Common;
using pennywise.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Domain.Entities
{
    public class BankDetail : AuditableBaseEntity
    {
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public double Amount { get; set; }
        public double charges { get; set; }
        public double TotalAmount { get; set; }
        public string UserId { get; set; }
        public BankOwnershipType BankOwnershipType { get; set; }
        public string PaystackRecipientCode { get; set; }
    }
}
