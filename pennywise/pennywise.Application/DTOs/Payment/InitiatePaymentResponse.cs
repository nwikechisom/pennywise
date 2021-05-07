using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Application.DTOs.Payment
{
    public class InitiatePaymentResponse
    {
        public string authorization_url { get; set; }
        public string access_code { get; set; }
        public string reference { get; set; }
    }
}
