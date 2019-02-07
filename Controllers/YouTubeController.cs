using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using Microsoft.IdentityModel.Protocols;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;

namespace FactFluxV3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YouTubeController : ControllerBase
    {
        readonly IConfiguration configuration;

        public YouTubeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<List<SearchResult>> GetVids(string channelId)
        {
            return await GetVidsForFeed(channelId);
        }

        private async Task<List<SearchResult>> GetVidsForFeed(string channelId)
        {
            List<SearchResult> newVids;

            var youtTubeApiKey = configuration["IntegrationSettings:YouTube:ApiKey"];

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = youtTubeApiKey,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");

            searchListRequest.MaxResults = 20;
            searchListRequest.ChannelId = channelId;
            searchListRequest.Order = 0;

            var searchListResponse = await searchListRequest.ExecuteAsync().ConfigureAwait(false);

            newVids = new List<SearchResult>(searchListResponse.Items);

            return newVids;
        }
    }
}