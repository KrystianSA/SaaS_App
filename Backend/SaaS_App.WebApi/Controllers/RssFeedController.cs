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

        [HttpPost]
        public async Task<IActionResult> FeedReader([FromBody] ReadCommand.Request request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
