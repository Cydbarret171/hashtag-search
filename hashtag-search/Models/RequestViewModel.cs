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
            Page = 1;
        }
        
        public int? Page { get; set; }
        
        public int? PageSize { get; set; }

        public string Search { get; set; }

        public string LastSearch { get; set; }
    }
}