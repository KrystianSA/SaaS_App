using MediatR;
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
    public class AccountController : BaseController
    {
        public AccountController(ILogger<UserController> logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult> GetCurrentAccount() 
        {
            var currentAccountResult = await _mediator.Send(new CurrentAccountQuery.Request());
            return Ok(currentAccountResult);
        }
    }
}
