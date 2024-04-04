using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SaaS_App.Application.Logic.Account;
using SaaS_App.Application.Logic.User;
using SaaS_App.Application.Models.Email;
using SaaS_App.Infrastructure.Auth;
using SaaS_App.Infrastructure.Email;
using SaaS_App.WebApi.Application.Auth;
using SaaS_App.WebApi.Application.Response;

namespace SaaS_App.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly CookieSettings? _cookieSettings;
        private readonly JwtManager _jwtManager;
        private readonly IAntiforgery _antiforgery;
        private readonly IEmailSender _emailSender;

        public UserController(ILogger<UserController> logger, IMediator mediator,
            IOptions<CookieSettings> cookieSettings,
            JwtManager jwtManager,
            IAntiforgery antiforgery,
            IEmailSender emailSender) : base(logger, mediator)
        {
            _cookieSettings = cookieSettings != null ? cookieSettings.Value : null;
            _jwtManager = jwtManager;
            _antiforgery = antiforgery;
            _emailSender = emailSender;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> SendResetLink([FromBody] CreateRequirementsToResetPasswordCommand.Request request)
        {
            await _mediator.Send(request);
            var email = await _mediator.Send(new CreateEmailToRestPasswordCommand.Request() { Email = request.Email});
            _emailSender.SendEmail(email.emailData);
            return Accepted();
        }

        [HttpGet]
        public async Task<ActionResult> AntiforgeryToken()
        {
            var token = _antiforgery.GetAndStoreTokens(HttpContext);
            return Ok(token.RequestToken);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand.Request user)
        {
            await _mediator.Send(user); 
            return Ok();
        }


        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> CreateUserWithAccount([FromBody] CreateUserWithAccountCommand.Request user)
        {
            var result = await _mediator.Send(user);
            var email = await _mediator.Send(new CreateEmailToActivateAccountCommand.Request() { AccountId = result.AccountId });
            _emailSender.SendEmail(email.emailData);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginCommand.Request user)
        {
            var loginResult = await _mediator.Send(user);
            var token = _jwtManager.GenerateJwtToken(loginResult.UserId);
            SetTokenCookie(token);
            return Ok(new JwtToken() { AccessToken = token });
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            var logoutResult = await _mediator.Send(new LogoutCommand.Request());
            DeleteTokenCookie();
            return Ok(logoutResult);
        }

        [HttpGet]
        public async Task<ActionResult> GetLoggedInUser()
        {
            var LoggedInUserResult = await _mediator.Send(new SaaS_App.Application.Logic.Account.CurrentAccountQuery.Request());
            return Ok(LoggedInUserResult);
        }

        private void SetTokenCookie(string token)
        {
            var cookieSettings = new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(30),
                Secure = true,
                SameSite = SameSiteMode.Lax
            };

            if (_cookieSettings != null)
            {
                cookieSettings = new CookieOptions()
                {
                    HttpOnly = cookieSettings.HttpOnly,
                    Expires = cookieSettings.Expires,
                    Secure = _cookieSettings.Secure,
                    SameSite = _cookieSettings.SameSiteMode
                };
            }

            Response.Cookies.Append(CookieSettings.CookieName, token, cookieSettings);
        }

        private void DeleteTokenCookie()
        {
            Response.Cookies.Delete(CookieSettings.CookieName, new CookieOptions()
            {
                HttpOnly = true,
            });
        }
    }
}
