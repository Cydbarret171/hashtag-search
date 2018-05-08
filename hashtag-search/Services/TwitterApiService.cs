using hashtag_search.Models;
using hashtag_search.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace hashtag_search.Services
{
    public class TwitterApiService : ITwitterApiService
    {
        private const string _url = "https://api.twitter.com/1.1/search/tweets.json?q={0}&result_type=popular";

        private TwitterAuthentication Authentication { get; set; }

        private string LastError { get; set; }

        public TwitterApiService()
        {
            Initialize();
        }

        public void Initialize()
        {
            Authentication = new TwitterAuthentication();
        }

        public TwitterSearchResponse Search(string searchParameter, int? pageSize, string maxId = "")
        {
            var requestUrl = string.Format(_url, searchParameter);

            if (pageSize.HasValue)
            {
                requestUrl = $"{requestUrl}&count={HttpUtility.UrlEncode(pageSize.Value.ToString())}";
            }

            if (!string.IsNullOrEmpty(maxId))
            {
                requestUrl = $"{requestUrl}&max_id={HttpUtility.UrlEncode(maxId)}";
            }

            return MakeSearchRequest($"{requestUrl}&tweet_mode=extended");
        }
        
        public string GetError()
        {
            return LastError;
        }

        public bool HasError()
        {
            return !string.IsNullOrEmpty(LastError);
        }

        private void SetError(string message)
        {
            LastError = message;
        }

        private TwitterSearchResponse MakeSearchRequest(string url)
        {
            var searchResponse = new TwitterSearchResponse();

            using(var client = new HttpClient())
            {
                var authorizationHeader = GetAuthorizationHeader();

                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    SetError("Bearer Token Not Available");
                }
                else
                {
                    client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);

                    try
                    {
                        var response = client.GetAsync(url).Result;

                        var content = response.Content.ReadAsStringAsync().Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            searchResponse = (ParseSearchJson(content));

                            HandleUrlInTweetText(searchResponse);
                        }
                        else
                        {
                            SetError($"{response.StatusCode.ToString()}: {content}");
                        }
                    }
                    catch(Exception e)
                    {
                        SetError($"Unable to perform search: {e}");
                    }
                }
            }

            return searchResponse;
        }

        private void HandleUrlInTweetText(TwitterSearchResponse response)
        {
            foreach (var tweet in response.statuses)
            {
                var fullText = tweet.full_text;

                var indexOfTweetUrl = fullText.IndexOf("https://t.co");

                if (indexOfTweetUrl == -1) continue;

                tweet.full_text = fullText.Replace(fullText.Substring(indexOfTweetUrl), string.Empty);
            }
        }

        private TwitterSearchResponse ParseSearchJson(string content){

            using(var reader = new StringReader(content))
            {
                var deserializer = JsonSerializer.Create();

                return (TwitterSearchResponse)deserializer.Deserialize(reader, typeof(TwitterSearchResponse));
            }
        }

        private string GetAuthorizationHeader()
        {
            return $"Bearer {Authentication.GetBearerToken()}";
        }
    }
}