using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Application.DTOs.Payment
{
    public class ResolveAccountResponse
    {
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("bank_id")]
        public int BankId { get; set; }
    }
}
