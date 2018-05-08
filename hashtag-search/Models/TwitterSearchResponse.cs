using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hashtag_search.Models
{
    public class TwitterSearchResponse
    {
        public TwitterSearchResponse()
        {
            statuses = new List<Rootobject>();
        }

        public List<Rootobject> statuses { get; set; }
    }


    public class Rootobject
    {
        public string created_at { get; set; }
        public string id { get; set; }
        public string id_str { get; set; }
        public string full_text { get; set; }
        public bool truncated { get; set; }
        public Entities entities { get; set; }
        public string source { get; set; }
        public User user { get; set; }
        public bool is_quote_status { get; set; }
        public int retweet_count { get; set; }
        public int favorite_count { get; set; }
        public bool favorited { get; set; }
        public bool retweeted { get; set; }
        public string urlToTwitter { get; set; }
    }

    public class Entities
    {
        public Hashtag[] hashtags { get; set; }
        public Url[] urls { get; set; }
    }

    public class Hashtag
    {
        public string text { get; set; }
        public int[] indices { get; set; }
    }

    public class Url
    {
        public string url { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public int[] indices { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string id_str { get; set; }
        public string name { get; set; }
        public string screen_name { get; set; }
        public string url { get; set; }
        public bool verified { get; set; }
        public string profile_background_color { get; set; }
        public string profile_background_image_url { get; set; }
        public string profile_background_image_url_https { get; set; }
        public bool profile_background_tile { get; set; }
        public string profile_image_url { get; set; }
        public string profile_image_url_https { get; set; }
        public string profile_banner_url { get; set; }
        public string profile_link_color { get; set; }
        public string profile_sidebar_border_color { get; set; }
        public string profile_sidebar_fill_color { get; set; }
        public string profile_text_color { get; set; }
        public bool profile_use_background_image { get; set; }
        public bool has_extended_profile { get; set; }
        public bool default_profile { get; set; }
        public bool default_profile_image { get; set; }
    }

    public class Entities1
    {
        public Url1 url { get; set; }
        public Description description { get; set; }
    }

    public class Url1
    {
        public Url2[] urls { get; set; }
    }

    public class Url2
    {
        public string url { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public int[] indices { get; set; }
    }

    public class Description
    {
        public string[] urls { get; set; }
    }
}