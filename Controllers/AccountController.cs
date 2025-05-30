using BlockchainExplorer.Common;
using BlockchainExplorer.Models.Common;
using BlockchainExplorer.Models.Features.Account;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BlockchainExplorer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly OkxApi _endpoints = new OkxApi();
        private readonly IConfiguration _configuration;
        private readonly OkxApiClient _apiClient = new OkxApiClient();

        public AccountController(IConfiguration configuration, OkxApiClient apiClient)
        {
            _configuration = configuration;
            _apiClient = apiClient;
        }

        [HttpGet("get-account")]
        public async Task<IActionResult> GetAccount()
        {
            var accountIdUrl = _endpoints.Account.GetAccount;

            JsonElement okxClient = await _apiClient.GetApiDataAsync(accountIdUrl);

            var response = OkxApi.ConvertJsonElementToModel<GetAccount>(okxClient);

            return Ok(response);
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccount request)
        {
            var accountIdUrl = _endpoints.Account.CreateAccount;

            JsonElement okxClient = await _apiClient.GetApiDataAsync(accountIdUrl, null, "POST", JsonSerializer.Serialize(request));

            var response = OkxApi.ConvertJsonElementToModel<CreateAccountResponse>(okxClient);

            return Ok(response);
        }

        [HttpPost("update-account")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccount request)
        {
            var accountIdUrl = _endpoints.Account.UpdateAccount;

            JsonElement okxClient = await _apiClient.GetApiDataAsync(accountIdUrl, null, "POST", JsonSerializer.Serialize(request));

            var response = OkxApi.ConvertJsonElementToModel<CreateAccountResponse>(okxClient);

            return Ok(response);
        }
    }
}
