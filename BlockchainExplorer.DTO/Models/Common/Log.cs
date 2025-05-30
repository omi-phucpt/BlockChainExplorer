using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainExplorer.DTO.Models.Common
{
    public class Log
    {
        [Key]
        public string Sign { get; set; }
        public string TimeStamp { get; set; }
        public string ApiUrl { get; set; }
        public string Response { get; set; }
        public string Body { get; set; }
        public string Params { get; set; }
    }
}
