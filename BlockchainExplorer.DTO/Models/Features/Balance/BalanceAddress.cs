namespace BlockchainExplorer.BlockchainExplorer.DTO.Models.Features.Balance
{
    public class BalanceAddress
    {
        public string address { get; set; }
        public string? chainId { get; set; }
        public string? assetType { get; set; }
        public bool? excludeRiskToken { get; set; }
    }
}
