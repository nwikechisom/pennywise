using Newtonsoft.Json;

namespace pennywise.Application.DTOs.Payment
{
    public class SingleTransferRequest : TransferBaseRequest
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("recipient")]
        public string Recipient { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}