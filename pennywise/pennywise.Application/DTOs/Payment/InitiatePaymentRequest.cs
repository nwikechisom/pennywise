using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Application.DTOs.Payment
{
    public class InitiatePaymentRequest
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; } = "1";
        
        [JsonProperty("callback_url")]
        public string Callback_Url { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; } = "NGN";
        [JsonProperty("channels")]
        public List<string> Channels { get; set; } = new List<string> { "card", "bank", "ussd", "qr", "mobile_money", "bank_transfer" };
    }
}
