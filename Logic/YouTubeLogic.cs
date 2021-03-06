﻿using FactFluxV3.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;


namespace FactFluxV3.Logic
{
    public class YouTubeLogic
    {
        private readonly IConfiguration Configuration;
        private readonly IMemoryCache Cache;


        public YouTubeLogic(IConfiguration configuration, IMemoryCache cache)
        {
            Configuration = configuration;
            Cache = cache;
        }

        public List<Article> CheckNewsEntityForVideos(Rssfeeds feed)
        {
            var videoArticleList = new List<Article>();

            if (feed.VideoLink == null)
            {
                return videoArticleList;
            }

            var videoList = GetVidsForNewsEntity(feed.VideoLink);

            var vidListResult = videoList.Where(x => x.Id.VideoId != null).ToList();

            var newArticleLogic = new ArticleLogic(Cache);

            foreach (var video in vidListResult)
            {
                var newVid = newArticleLogic.CreateArticleFromVideo(feed, video);

                videoArticleList.Add(newVid);

                if (newVid.ArticleTitle.StartsWith("DupeVid"))
                {
                    continue;
                }

                var wordLogLogic = new WordLogLogic();

                wordLogLogic.LogWordsUsed(newVid);
            }

            return videoArticleList;
        }

        public List<SearchResult> GetVidsForNewsEntity(string channelId)
        {
            List<SearchResult> newVids = new List<SearchResult>();

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


            try
            {

                var searchLasistResponse = searchListRequest.Execute();

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }


            var searchListResponse = searchListRequest.Execute();

            newVids = new List<SearchResult>(searchListResponse.Items);

            return newVids;
        }
    }
}
