using MediatR;
using Microsoft.AspNetCore.Mvc;
using SaaS_App.Application.Logic.RssFeed;

namespace SaaS_App.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RssFeedController : BaseController
    {
        public RssFeedController(ILogger<RssFeedController> logger,
            IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Read()
        {
            var result = await _mediator.Send(new ReadQuery.Request());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Write([FromBody] WriteCommand.Request request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
