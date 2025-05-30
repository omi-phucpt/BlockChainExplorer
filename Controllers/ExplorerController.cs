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

        [HttpGet("get-chains")]
        public async Task<IActionResult> GetChains()
        {
            var supportedChainUrl = _endpoints.Balance.SupportedChain;

            JsonElement okxClient = await _apiClient.GetApiDataAsync(supportedChainUrl);

            var response = OkxApi.ConvertJsonElement<Chain>(okxClient);

            return Ok(response);
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