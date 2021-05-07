using pennywise.Domain.Common;
using pennywise.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Domain.Entities
{
    public class PaymentPlan : AuditableBaseEntity
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool isActivated { get; set; }
        public DateTime? ActivationDate { get; set; }
        public double Amount { get; set; }
        public bool IsLockedAmount { get; set; }
        public DateTime NextDueDate { get; set; }
        public PlanSchedule Schedule { get; set; }
    }
}
