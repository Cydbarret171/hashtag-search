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
        }

        public RequestViewModel LastRequest { get; set; }

        public TwitterSearchResponse Tweets { get; set; }

        public bool HasError { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsSearch { get; set; }

        public int? MaxResults { get; set; }
    }
}