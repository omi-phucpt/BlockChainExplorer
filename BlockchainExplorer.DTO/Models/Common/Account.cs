using System.ComponentModel.DataAnnotations;

namespace BlockchainExplorer.DTO.Models.Common
{
    public class Account
    {
        [Key]
        public string AccountId { get; set; }
        public int AccountType { get; set; }
    }
}
