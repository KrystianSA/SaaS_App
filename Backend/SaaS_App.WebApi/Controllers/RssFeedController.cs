using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

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
        public async Task<IActionResult> Rss()
        {
            var feed = new SyndicationFeed("Title","Descirption",new Uri("http://www.voidgeeks.com"),"RSSUrl",DateTime.UtcNow);
            var item = new List<SyndicationItem>();
            item.Add(new SyndicationItem("title","content",new Uri("http://www.voidgeeks.com/tutorial/How-to-Export-Data-into-PDF-file-in-ASPNET-Core/16")));
            feed.Items = item;
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = true,
                Indent = true
            };
            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, settings))
                {
                    var rssFormatter = new Rss20FeedFormatter(feed, false);
                    rssFormatter.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                }
                return File(stream.ToArray(), "application/rss+xml; charset=utf-8");
            }
        }
    }
}
