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

        public AccountController(ILogger<AccountController> logger,
            IMediator mediator,
            IEmailSender emailSender) : base(logger, mediator)
        {
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> BlockAccount([FromBody] BlockAccountCommand.Request request)
        {
            await _mediator.Send(request);
            var email = await _mediator.Send(new CreateEmailAboutBlockCommand.Request() { Email = request.Email});
            _emailSender.SendEmail(email.emailData);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentAccount()
        {
            var currentAccountResult = await _mediator.Send(new CurrentAccountQuery.Request());
            return Ok(currentAccountResult);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ActivateAccount([FromBody] ActivateAccountCommand.Request user)
        {
            await _mediator.Send(user);
            return Ok();
        }
    }
}
