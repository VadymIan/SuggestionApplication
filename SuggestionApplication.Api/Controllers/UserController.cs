using Microsoft.AspNetCore.Mvc;
using SuggestionApplication.Application.DTO;
using SuggestionApplication.Application.Infrastructure;
using SuggestionApplication.Application.Models;

namespace SuggestionApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[ServiceFilter(typeof(PerformanceFilterAsync))] Apply filter if it was registered as a service
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registeredUser = await _userService.Register(user);

            var callbackUrl = $"{Request.Scheme}:{Request.Host}/api/user/confirmEmail?userId={registeredUser.Id}";
            var email = new Email
            {
                To = user.EmailAddress,
                Subject = "Email address confirmation",
                Body = $"Please confirm user creation by clicking this url: {callbackUrl}"
            };

            await _emailService.SendEmail(email);

            return Ok();
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _userService.Login(loginUser);

            return Ok(token);
        }

        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmUserEmail(Guid userId)
        {
            await _userService.ComfirmEmail(userId);

            return Ok();
        }
    }
}
