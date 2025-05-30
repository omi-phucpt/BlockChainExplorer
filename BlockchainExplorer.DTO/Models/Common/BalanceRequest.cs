namespace BlockchainExplorer.Models.Common
{
    public class BalanceRequest
    {
        public string Address { get; set; }
        public string? AssetType { get; set; }
        public string? ChainId { get; set; }
        public bool? ExcludeRiskToken { get; set; }
    }
}
