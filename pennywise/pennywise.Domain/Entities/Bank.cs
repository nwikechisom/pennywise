using pennywise.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Domain.Entities
{
    public class Bank : AuditableBaseEntity
    {
        public string BankCode { get; set; }
        public string BankName { get; set; }
    }
}
