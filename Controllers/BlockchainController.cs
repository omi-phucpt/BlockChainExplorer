//using Microsoft.AspNetCore.Mvc;
//using Nethereum.Web3;
//using Nethereum.Contracts;
//using Nethereum.ABI.FunctionEncoding.Attributes;
//using System.Numerics;
//using System.Threading.Tasks;

//namespace BlockchainExplorer.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ExplorerController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;

//        public ExplorerController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        [HttpPost("balance")]
//        public async Task<IActionResult> GetBalance([FromBody] BalanceRequest request)
//        {
//            var network = GetNetwork(request.ChainId);
//            if (network == null) return BadRequest("Invalid network");

//            var web3 = new Web3(network.RpcUrl);
//            var response = new BalanceResponse();

//            // Get native balance (ETH, BNB, etc.)
//            var balance = await web3.Eth.GetBalance.SendRequestAsync(request.Address);
//            response.NativeBalance = Nethereum.Util.UnitConversion.Convert.FromWei(balance.Value);
//            response.NativeSymbol = network.Symbol;

//            // Get token balance (if provided)
//            if (!string.IsNullOrEmpty(request.TokenContract))
//            {
//                var contract = web3.Eth.GetContract(ERC20_ABI, request.TokenContract);
//                var balanceFunction = contract.GetFunction("balanceOf");
//                var symbolFunction = contract.GetFunction("symbol");

//                var tokenBalance = await balanceFunction.CallAsync<BigInteger>(request.Address);
//                var tokenSymbol = await symbolFunction.CallAsync<string>();

//                response.TokenBalance = Nethereum.Util.UnitConversion.Convert.FromWei(tokenBalance, 18); // Assuming 18 decimals
//                response.TokenSymbol = tokenSymbol;
//            }

//            return Ok(response);
//        }

//        [HttpPost("contract-info")]
//        public async Task<IActionResult> GetContractInfo([FromBody] ContractInfoRequest request)
//        {
//            var network = GetNetwork(request.ChainId);
//            if (network == null) return BadRequest("Invalid network");

//            var web3 = new Web3(network.RpcUrl);
//            var contract = web3.Eth.GetContract(ERC20_ABI, request.ContractAddress);

//            var nameFunction = contract.GetFunction("name");
//            var symbolFunction = contract.GetFunction("symbol");
//            var totalSupplyFunction = contract.GetFunction("totalSupply");

//            var name = await nameFunction.CallAsync<string>();
//            var symbol = await symbolFunction.CallAsync<string>();
//            var totalSupply = await totalSupplyFunction.CallAsync<BigInteger>();

//            return Ok(new
//            {
//                Name = name,
//                Symbol = symbol,
//                TotalSupply = Nethereum.Util.UnitConversion.Convert.FromWei(totalSupply, 18)
//            });
//        }

//        private Network GetNetwork(int chainId)
//        {
//            return chainId switch
//            {
//                1 => new Network { RpcUrl = _configuration["RpcUrls:Ethereum"], Symbol = "ETH" },
//                8453 => new Network { RpcUrl = _configuration["RpcUrls:Base"], Symbol = "ETH" },
//                137 => new Network { RpcUrl = _configuration["RpcUrls:Polygon"], Symbol = "MATIC" },
//                _ => null
//            };
//        }

//        private const string ERC20_ABI = @"[
//            { 'constant': true, 'inputs': [], 'name': 'name', 'outputs': [{ 'name': '', 'type': 'string' }], 'type': 'function' },
//            { 'constant': true, 'inputs': [], 'name': 'symbol', 'outputs': [{ 'name': '', 'type': 'string' }], 'type': 'function' },
//            { 'constant': true, 'inputs': [], 'name': 'totalSupply', 'outputs': [{ 'name': '', 'type': 'uint256' }], 'type': 'function' },
//            { 'constant': true, 'inputs': [{ 'name': '_owner', 'type': 'address' }], 'name': 'balanceOf', 'outputs': [{ 'name': 'balance', 'type': 'uint256' }], 'type': 'function' }
//        ]";
//    }

//    public class BalanceRequest
//    {
//        public string Address { get; set; }
//        public string TokenContract { get; set; }
//        public int ChainId { get; set; }
//    }

//    public class ContractInfoRequest
//    {
//        public string ContractAddress { get; set; }
//        public int ChainId { get; set; }
//    }

//    public class BalanceResponse
//    {
//        public decimal NativeBalance { get; set; }
//        public string NativeSymbol { get; set; }
//        public decimal? TokenBalance { get; set; }
//        public string TokenSymbol { get; set; }
//    }

//    public class Network
//    {
//        public string RpcUrl { get; set; }
//        public string Symbol { get; set; }
//    }
//}