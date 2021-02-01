using System.Threading.Tasks;
using ChrisfrewDotInApi.Infrastructure;
using ChrisfrewDotInApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChrisfrewDotInApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LinkPreviewController : ControllerBase
    {

        private readonly ILogger<LinkPreviewController> logger;
        private readonly ILinkPreviewService linkPreviewService;

        public LinkPreviewController(ILogger<LinkPreviewController> logger, ILinkPreviewService linkPreviewService)
        {
            this.logger = logger;
            this.linkPreviewService = linkPreviewService;
        }

        [HttpGet]
        public async Task<LinkPreview> Get(string url)
        {
            return await linkPreviewService.CallApi(url);
        }
    }
}
