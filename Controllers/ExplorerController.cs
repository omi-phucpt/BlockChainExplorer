using BlockchainExplorer.Common;
using BlockchainExplorer.Models.Common;
using BlockchainExplorer.Models.Features.Account;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BlockchainExplorer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExplorerController : ControllerBase
    {
        private readonly OkxApi _endpoints = new OkxApi();
        private readonly OkxApiClient _apiClient = new OkxApiClient();

        public ExplorerController(OkxApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet("getChains")]
        public async Task<IActionResult> GetChains()
        {
            var supportedChainUrl = _endpoints.Balance.SupportedChain;

            JsonElement okxClient = await _apiClient.GetApiDataAsync(supportedChainUrl);

            var response = OkxApi.ConvertJsonElement<Chain>(okxClient);

            return Ok(response);
        }

        [HttpPost("getBalances")]
        public async Task<IActionResult> GetBalances(BalanceRequest request)
        {
            if (string.IsNullOrEmpty(request.Address))
            {
                return BadRequest("Address là tham số bắt buộc.");
            }

            var queryParams = new Dictionary<string, string>
            {
                { "address", request.Address },
                { "chains", string.IsNullOrEmpty(request.ChainId) ? "" : request.ChainId },
            };
            if (!string.IsNullOrEmpty(request.AssetType))
            {
                queryParams.Add("assetType", request.AssetType);
            }
            if (request.ExcludeRiskToken.HasValue)
            {
                queryParams.Add("excludeRiskToken", request.ExcludeRiskToken.Value.ToString().ToLower());
            }

            var totalValueUrl = _endpoints.Balance.TotalValue;

            JsonElement okxClient = await _apiClient.GetApiDataAsync(totalValueUrl, queryParams);

            string response = okxClient.GetRawText();

            return Content(response, "application/json");
        }

        [HttpGet("getAssets")]
        public async Task<IActionResult> GetAssets()
        {
            var supportedChainUrl = _endpoints.Balance.SupportedChain;

            JsonElement okxClient = await _apiClient.GetApiDataAsync(supportedChainUrl);

            string response = okxClient.GetRawText();

            return Content(response, "application/json");
        }
    }
}