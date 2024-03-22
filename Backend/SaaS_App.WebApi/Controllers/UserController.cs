using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SaaS_App.Application.Logic.User;
using SaaS_App.Infrastructure.Auth;
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

        public UserController(ILogger<UserController> logger, IMediator mediator,
            IOptions<CookieSettings> cookieSettings,
            JwtManager jwtManager,
            IAntiforgery antiforgery) : base(logger, mediator)
        {
            _cookieSettings = cookieSettings != null ? cookieSettings.Value : null;
            _jwtManager = jwtManager;
            _antiforgery = antiforgery;
        }
        [HttpGet]
        public async Task<ActionResult> AntiforgeryToken()
        {
            var token = _antiforgery.GetAndStoreTokens(HttpContext);
            return Ok(token.RequestToken);
        }

        //[HttpPost]
        //public async Task<ActionResult> LinkToResetPassword(LinkToResetPasswordCommand.Request user)
        //{
        //    var linkToResetPasswordResult = await _mediator.Send(user);
        //    return Ok(linkToResetPasswordResult);
        //}

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword.Request user)
        {
            var changePasswordResult = await _mediator.Send(user); 
            return Ok(changePasswordResult);
        }


        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> CreateUserWithAccount([FromBody] CreateUserWithAccountCommand.Request user)
        {
            var createAccountResult = await _mediator.Send(user);
            var token = _jwtManager.GenerateJwtToken(createAccountResult.UserId);
            SetTokenCookie(token);
            return Ok(new JwtToken() { AccessToken = token });
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
            var LoggedInUserResult = await _mediator.Send(new CurrentAccountQuery.Request());
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
