using Microsoft.AspNetCore.Mvc;
using OnPOS.Models.DTOs;
using OnPOS.Services;
using System.Threading.Tasks;

namespace OnPOS.Controllers.API
{
    [Route("api/stock")]
    [ApiController] // This attribute is important for an API controller
    public class StockApiController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockApiController(IStockService stockService)
        {
            _stockService = stockService;
        }

        // POST: api/stock/receive
        [HttpPost("receive")]
        public async Task<IActionResult> ReceiveStock([FromBody] StockReceiveApiRequest request)
        {
            if (!ModelState.IsValid || request.Items == null)
            {
                return BadRequest("Invalid request data.");
            }

            // This is where you would validate an API Key or JWT token for security
            // if(!IsApiKeyValid(Request.Headers["X-API-KEY"])) { return Unauthorized(); }

            var (success, errorMessage) = await _stockService.ReceiveStockAsync(request.StoreId, request.Items);

            if (success)
            {
                return Ok(new { message = "Stock received successfully." });
            }
            else
            {
                return BadRequest(new { error = errorMessage });
            }
        }
    }
}
