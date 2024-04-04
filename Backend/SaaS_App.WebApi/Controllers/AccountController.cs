using MediatR;
using Microsoft.AspNetCore.Mvc;
using SaaS_App.Application.Logic.Account;
using SaaS_App.Infrastructure.Email;

namespace SaaS_App.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IEmailSender _emailSender;

        public AccountController(ILogger<UserController> logger,
            IMediator mediator,
            IEmailSender emailSender) : base(logger, mediator)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentAccount()
        {
            var currentAccountResult = await _mediator.Send(new CurrentAccountQuery.Request());
            return Ok(currentAccountResult);
        }

        [HttpPost]
        public async Task<IActionResult> ActivateAccount([FromBody] ActivateAccountCommand.Request user)
        {
            await _mediator.Send(user);
            return Ok();
        }
    }
}
