namespace BlockchainExplorer.Models.Features.Account
{
    public class CreateAccount
    {
        public Addresses[] addresses { get; set; }
    }

    public class Addresses
    {
        public string address { get; set; }
        public string chainIndex { get; set; }
    }

    public class CreateAccountResponse
    {
        public string accountId { get; set; }
    }
}
