using hashtag_search.Models;
using System.Collections.Generic;

namespace hashtag_search.Services
{
    public interface ITwitterApiService
    {
        void Initialize();

        TwitterSearchResponse Search(string searchParameter);

        TwitterSearchResponse PagedSearch(RequestViewModel searchRequest);

        string GetError();

        bool HasError();
    }
}