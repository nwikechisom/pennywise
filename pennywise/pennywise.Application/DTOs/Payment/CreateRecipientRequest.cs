using Newtonsoft.Json;

namespace pennywise.Application.DTOs.Payment
{
    public class CreateRecipientRequest
    {
        [JsonProperty("type")] public string Type { get; set; } = "nuban";

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        [JsonProperty("currency")] public string Currency { get; set; } = "NGN";
    }
}