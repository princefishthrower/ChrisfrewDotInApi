using System;
using System.Net.Http;
using System.Threading.Tasks;
using ChrisfrewDotInApi.Infrastructure;
using ChrisfrewDotInApi.Models;
using Newtonsoft.Json;

namespace ChrisfrewDotInApi.Services
{
    public class LinkPreviewService : ILinkPreviewService
    {
        private string apiKey;
        private string endpoint;

        public LinkPreviewService(string endpoint, string apiKey)
        {
            this.apiKey = apiKey;
            this.endpoint = endpoint;
        }

        public async Task<LinkPreview> CallApi(string url)
        {
            var linkPreview = new LinkPreview();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{endpoint}?key={this.apiKey}&q={url}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    linkPreview = JsonConvert.DeserializeObject<LinkPreview>(apiResponse);
                }
            }
            return linkPreview;
        }
    }
}
