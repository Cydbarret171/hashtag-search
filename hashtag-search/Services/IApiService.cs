using hashtag_search.Models;
using System.Collections.Generic;

namespace hashtag_search.Services
{
    public interface ITwitterApiService
    {
        void Initialize();

        TwitterSearchResponse Search(string searchParameter, int? pageSize, string maxId = "");

        string GetError();

        bool HasError();
    }
}