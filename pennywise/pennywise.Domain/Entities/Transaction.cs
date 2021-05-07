using pennywise.Domain.Common;
using pennywise.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Domain.Entities
{
    public class Transaction : AuditableBaseEntity
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string TransactionMethod { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public TransactionType TransactionType { get; set; }
        public double UserPreviousBalance { get; set; }
        public double UserCurrentBalance { get; set; }
        public string Reference { get; set; }
        public string InitiateMessage { get; set; }
        public bool SuccessfullyInitiated { get; set; }
        public string VerificationResponse { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public long PlanId { get; set; }
    }
}
