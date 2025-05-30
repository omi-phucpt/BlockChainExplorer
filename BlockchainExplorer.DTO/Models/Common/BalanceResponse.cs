namespace BlockchainExplorer.Models.Common
{
    public class BalanceResponse
    {
        public decimal NativeBalance { get; set; }
        public string NativeSymbol { get; set; }
        public decimal? TokenBalance { get; set; }
        public string TokenSymbol { get; set; }
    }
}
