using System.Collections.Generic;
using Newtonsoft.Json;

namespace pennywise.Application.DTOs.Payment
{
    public class BulkTransferResponse
    {
        public List<InitiatedBulkTransfer> InitiatedBulkTransfers { get; set; }
    }

    public class InitiatedBulkTransfer
    {
        [JsonProperty("recipient")]
        public string Recipient { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("transfer_code")]
        public string TransferCode { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}