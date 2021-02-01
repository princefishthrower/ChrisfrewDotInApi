using System.Threading.Tasks;
using ChrisfrewDotInApi.Models;

namespace ChrisfrewDotInApi.Infrastructure
{
    public interface ILinkPreviewService
    {
        Task<LinkPreview> CallApi(string url);
    }
}
