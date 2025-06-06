namespace BlockchainExplorer.BlockchainExplorer.DTO.Models.Features.Balance
{
    public class BalanceAccountId
    {
        public string accountId { get; set; }
        public string? chains { get; set; }
        public string? assetType { get; set; }
        public bool? excludeRiskToken { get; set; }
    }
}
