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

            if (!requestSearchParameter.Contains("#"))
            {
                requestSearchParameter = $"#{requestSearchParameter}";
                request.Search = requestSearchParameter;
            }

            if(request.LastSearch != requestSearchParameter)
            {
                request.LastSearch = requestSearchParameter;
                request.Page = 1;
                request.PageSize = 5;
            }

            var nextViewModel = new ResultViewModel
            {
                LastRequest = request,
                IsSearch = true
            };

            if (ModelState.IsValid && !string.IsNullOrEmpty(request.Search))
            {
                var searchResult = TwitterService.PagedSearch(request);

                if (TwitterService.HasError())
                {
                    nextViewModel.HasError = true;
                    nextViewModel.ErrorMessage = TwitterService.GetError();
                }

                nextViewModel.Tweets = searchResult;

                var pageSize = request.PageSize.Value;

                var skip = (request.Page.Value - 1) * pageSize;

                var take = pageSize;

                nextViewModel.MaxResults = nextViewModel.Tweets.statuses.Count();

                nextViewModel.Tweets.statuses = nextViewModel.Tweets.statuses.Skip(skip).Take(take).ToList();
            }
            
            return View("Index", nextViewModel); 
        }
    }
}