using hashtag_search.Models;
using hashtag_search.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hashtag_search.Controllers
{
    public class SearchController : Controller
    {
        public ITwitterApiService TwitterService { get; }

        public SearchController(ITwitterApiService twitterService)
        {
            TwitterService = twitterService;
        }

        public ActionResult Index()
        {
            return View(new ResultViewModel());
        }

        /// <summary>
        /// Take in view model with search term and paging information
        /// 
        /// Default page size is 5
        /// </summary>
        /// 
        /// <returns>
        /// Twitter API results for search criteria. 
        /// 
        /// Returns empty list with page size of zero if blank search is encountered.
        /// </returns>
        [HttpPost]
        public ActionResult Search(RequestViewModel request)
        {
            var requestSearchParameter = request.Search;

            var nextViewModel = new ResultViewModel
            {
                LastRequest = request,
                IsSearch = true,
                NewLastTweetId = request.NextTweetId,
                PreviousPage = request.LastPage.Value,
                PreviousTweetId = request.LastTweetId
            };

            if (ModelState.IsValid)
            {
                var searchResult = new TwitterSearchResponse();

                if (!string.IsNullOrEmpty(requestSearchParameter))
                {
                    var maxId = string.Empty;

                    var sinceId = string.Empty;

                    var shouldCheckMaxId = request.LastPage.HasValue && request.Page.HasValue;

                    if (shouldCheckMaxId && request.LastPage > request.Page.Value) maxId = request.LastTweetId;

                    if (shouldCheckMaxId && request.LastPage < request.Page.Value) sinceId = request.NextTweetId;

                    searchResult = TwitterService.Search(requestSearchParameter, request.PageSize, maxId, sinceId);

                    if (TwitterService.HasError())
                    {
                        nextViewModel.HasError = true;
                        nextViewModel.ErrorMessage = TwitterService.GetError();
                    }

                    if (searchResult != null && searchResult.statuses != null && searchResult.statuses.Any())
                    {
                        var nextMaxId = searchResult.statuses.Last().id_str;

                        if (nextMaxId != request.NextTweetId)
                        {
                            nextViewModel.NewLastTweetId = nextMaxId;
                            nextViewModel.PreviousTweetId = request.NextTweetId;
                            nextViewModel.PreviousPage = request.Page;
                        }
                    }

                    nextViewModel.Tweets = searchResult;
                }
            }
            
            return View("Index", nextViewModel); 
        }
    }
}