using BlockchainExplorer.BlockchainExplorer.DTO.Models.Features.Balance;
using BlockchainExplorer.Common;
using BlockchainExplorer.Extensions;
using BlockchainExplorer.Models.Common;
using BlockchainExplorer.Models.Features.Account;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BlockchainExplorer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetController : ControllerBase
    {
        private readonly OkxApi _endpoints = new OkxApi();
        private readonly OkxApiClient _apiClient = new OkxApiClient();

        public AssetController(OkxApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpPost("get-asset-address")]
        public async Task<IActionResult> GetBalancesAddress(AssetAddress request)
        {
            if (string.IsNullOrEmpty(request.address))
                return BadRequest("Address is required.");

            var queryParams = new Dictionary<string, string>
            {
                ["address"] = request.address,
                ["chains"] = request.chainId
            }
            .AddIfNotNull("filter", request.filter.ToString());

            var address = _endpoints.Balance.AllTokenAddress;

            JsonElement okxClient = await _apiClient.GetApiDataAsync(address, queryParams);

            string response = okxClient.GetRawText();

            return Content(response, "application/json");
        }

        [HttpPost("get-asset-account")]
        public async Task<IActionResult> GetBalances(AssetAccountId request)
        {
            if (string.IsNullOrEmpty(request.accountId))
                return BadRequest("AccountId is required.");

            var queryParams = new Dictionary<string, string>
            {
                { "accountId", request.accountId },
                { "chains", string.IsNullOrEmpty(request.chains) ? "" : request.chains },
            }
            .AddIfNotNull("filter", request.filter.ToString());

            var account = _endpoints.Balance.AllTokenAccount;

            JsonElement okxClient = await _apiClient.GetApiDataAsync(account, queryParams);

            string response = okxClient.GetRawText();

            return Content(response, "application/json");
        }
    }
}