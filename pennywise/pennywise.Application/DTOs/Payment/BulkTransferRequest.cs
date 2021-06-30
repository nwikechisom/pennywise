using System.Collections.Generic;
using Newtonsoft.Json;

namespace pennywise.Application.DTOs.Payment
{
    public class BulkTransferRequest : TransferBaseRequest
    {
        [JsonProperty("transfers")] public List<BulkTransfer> Transfers { get; set; } = new List<BulkTransfer>();
    }

    public class BulkTransfer
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("recipient")]
        public string Recipient { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}