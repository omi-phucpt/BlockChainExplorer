namespace BlockchainExplorer.Models.Features.Account
{
    public class UpdateAccount
    {
        public string AccountId { get; set; }
        public string UpdateType { get; set; } // "add", "delete"
        public Addresses[] Addresses { get; set; }
    }
}
