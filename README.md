# hashtag-search
1. Clone repository.
2. Open in Visual Studio 2017.
3. Run in IIS Express.

Note: Twitter Search API only indexes the most recent popular tweets in the last 6-9 days. It says it supports up to 100 tweets returned from one search request but due to the nature of the data it usually only returns at most 15. The twitter search API does not index every tweet of a particular hashtag. This particular app has a limitation of only searching the last 100 instances of a hashtag in a tweet indexed for this API over 6-9 days. To truly search all of Twitter, the output of a streaming API would need to be captured and indexed for a search engine to browse.

To Read More: https://developer.twitter.com/en/docs/tweets/search/guides/standard-operators 
