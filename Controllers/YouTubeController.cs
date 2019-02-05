using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FactFluxV3.Models;
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
            var youtTubeApiKey = configuration["IntegrationSettings:YouTube:Default"];

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = youtTubeApiKey,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");

            searchListRequest.MaxResults = 20;
            searchListRequest.ChannelId = channelId;
            searchListRequest.Order = 0;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync().ConfigureAwait(false);

            var listOfVids = new List<SearchResult>(searchListResponse.Items);

            return listOfVids;
        }
    }
}