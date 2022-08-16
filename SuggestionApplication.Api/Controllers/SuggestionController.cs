using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuggestionApplication.Application.Exceptions;
using SuggestionApplication.Application.Infrastructure;
using SuggestionApplication.Domain.Entities;

namespace SuggestionApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuggestionController : ControllerBase
    {
        private readonly ILogger<SuggestionController> _logger;
        private readonly ISuggestionService _suggestionService;

        public SuggestionController(ILogger<SuggestionController> logger, ISuggestionService suggestionService)
        {
            _logger = logger;
            _suggestionService = suggestionService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSuggestion(Suggestion suggestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _suggestionService.CreateSuggestionAsync(suggestion);

            return Ok();
        }

        [Authorize]
        [HttpPost("upvote/{suggestionId}/{userId}")]
        public async Task<IActionResult> UpvoteSuggestion(Guid suggestionId, Guid userId)
        {
            if (suggestionId == default(Guid) || userId == default(Guid))
            {
                throw new BadRequestException($"{nameof(suggestionId)} or {nameof(userId)} is invalid");
            }

            _logger.LogInformation("User with id={0} is trying to upvote for suggestion with id={1}", userId, suggestionId);

            await _suggestionService.UpvoteSuggestionAsync(suggestionId, userId);

            return Ok();
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateSuggestion([FromBody] Suggestion suggestion)
        {
            await _suggestionService.UpdateSuggestionAsync(suggestion);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSuggestion(Guid id)
        {
            var suggestion = await _suggestionService.GetSuggestionAsync(id);

            return Ok(suggestion);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllSuggestions()
        {
            var suggestions = await _suggestionService.GetAllSuggestionsAsync();

            return Ok(suggestions);
        }

        [HttpGet("getAllNotApproved")]
        public async Task<IActionResult> GetAllSuggestionsWaitingForApproval()
        {
            var suggestions = await _suggestionService.GetAllSuggestionsWaitingForApprovalAsync();

            return Ok(suggestions);
        }

        [HttpGet("getAllApproved")]
        public async Task<IActionResult> GetAllSuggestionsApproved()
        {
            var suggestions = await _suggestionService.GetAllApprovedSuggestionsAsync();

            return Ok(suggestions);
        }

        [HttpGet("getUsersSuggestions")]
        public async Task<IActionResult> GetUsersSuggestions(Guid userId)
        {
            var suggestions = await _suggestionService.GetUsersSuggestionsAsync(userId);

            return Ok(suggestions);
        }
    }
}
