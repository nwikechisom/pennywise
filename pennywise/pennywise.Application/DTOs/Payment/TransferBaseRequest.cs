using Newtonsoft.Json;

namespace pennywise.Application.DTOs.Payment
{
    public class TransferBaseRequest
    {
        [JsonProperty("source")] public string Source { get; set; } = "balance";
        [JsonProperty("currency")] public string Currency { get; set; } = "NGN";
    }
}