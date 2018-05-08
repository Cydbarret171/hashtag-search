using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace hashtag_search.Models
{
    public class RequestViewModel
    {
        public RequestViewModel()
        {
            PageSize = 5;
            LastPage = 1;
            Page = 1;
        }
        
        public int? Page { get; set; }

        [Range(1, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int? PageSize { get; set; }

        public string Search { get; set; }

        public string LastTweetId { get; set; }

        public string NextTweetId { get; set; }

        public int? LastPage { get; set; }
    }
}