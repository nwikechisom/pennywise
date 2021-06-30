using System;
using Newtonsoft.Json;

namespace pennywise.Application.DTOs.Payment
{
    public class CreateRecipientResponse
    {
        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("integration")]
        public long Integration { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("recipient_code")]
        public string RecipientCode { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("details")]
        public RecipientDetails Details { get; set; }
    }

    public class RecipientDetails
    {
        [JsonProperty("authorization_code")]
        public object AuthorizationCode { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("account_name")]
        public object AccountName { get; set; }

        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        [JsonProperty("bank_name")]
        public string BankName { get; set; }
    }
}