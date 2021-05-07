using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Domain.Enums
{
    public enum TransactionMethod
    {
        NonSelected,BankTransfer,AtmCard,Others
    }

    public enum TransactionStatus
    {
        Initiated, Pending, Successful, Failed
    }

    public enum TransactionType
    {
        Deposit, Withdrawal
    }

    public enum PlanSchedule
    {
        Daily, Weekly, Every2Weeks, Monthly, Yearly 
    }
}
