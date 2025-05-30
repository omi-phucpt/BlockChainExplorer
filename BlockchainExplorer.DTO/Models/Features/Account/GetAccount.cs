namespace BlockchainExplorer.Models.Features.Account
{
    public class GetAccount
    {
        public string cursor { get; set; }
        public List<Accounts> accounts { get; set; }
    }

    public class Accounts
    {
        public string accountId { get; set; }
        public string accountType { get; set; }
    }
}
