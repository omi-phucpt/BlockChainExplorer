namespace BlockchainExplorer.BlockchainExplorer.DTO.Models.Features.Balance
{
    public class AssetResponse
    {
        public decimal NativeBalance { get; set; }
        public string NativeSymbol { get; set; }
        public decimal? TokenBalance { get; set; }
        public string TokenSymbol { get; set; }
    }
}
