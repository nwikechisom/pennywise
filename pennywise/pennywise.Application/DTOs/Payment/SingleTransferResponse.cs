using System;
using Newtonsoft.Json;

namespace pennywise.Application.DTOs.Payment
{
    public class SingleTransferResponse
    {
        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("integration")]
        public long Integration { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("recipient")]
        public long Recipient { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("transfer_code")]
        public string TransferCode { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}