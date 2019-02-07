using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactFluxV3.Logic
{
    public class YouTubeLogic
    {
        private readonly IConfiguration Configuration;

        public YouTubeLogic(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<SearchResult> GetVidsForFeed(string channelId)
        {
            List<SearchResult> newVids;

            var youtTubeApiKey = Configuration["IntegrationSettings:YouTube:ApiKey"];

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = youtTubeApiKey,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");

            searchListRequest.MaxResults = 20;
            searchListRequest.ChannelId = channelId;
            searchListRequest.Order = 0;

            var searchListResponse = searchListRequest.Execute();

            newVids = new List<SearchResult>(searchListResponse.Items);

            return newVids;
        }
    }
}
