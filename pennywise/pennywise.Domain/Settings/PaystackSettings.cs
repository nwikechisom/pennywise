using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Domain.Settings
{
    public class PaystackSettings
    {
        public string SecretKey { get; set; }
        public string PublicKey { get; set; }
        public string BaseUrl { get; set; }
        public string InitializeTransactionUrl { get; set; }
        public string VerifyTransactionUrl { get; set; }
        public string ResolveAccountUrl { get; set; }
        public string PaymentCallback { get; set; }
        public string CreateTransferRecipientUrl { get; set; }
        public string SingleTransferUrl { get; set; }
        public string BulkTransferUrl { get; set; }
    }
}
