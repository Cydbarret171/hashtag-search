using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hashtag_search.Models
{
    public class ResultViewModel
    {
        public ResultViewModel()
        {
            LastRequest = new RequestViewModel();
            Tweets = new TwitterSearchResponse();
            HasError = false;
            IsSearch = false;
            PreviousPage = 1;
        }

        public RequestViewModel LastRequest { get; set; }

        public TwitterSearchResponse Tweets { get; set; }

        public bool HasError { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsSearch { get; set; }

        public string NewLastTweetId { get; set; }

        public string PreviousTweetId { get; set; }

        public int? PreviousPage { get; set; }
    }
}