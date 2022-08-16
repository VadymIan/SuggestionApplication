using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuggestionApplication.Application.Infrastructure;
using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateStatusAsync(Status status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _statusService.CreateStatusAsync(status);

            return Ok();
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllStatusesAsync()
        {
            var statuses = await _statusService.GetAllStatusesAsync();

            return Ok(statuses);
        }
    }
}
