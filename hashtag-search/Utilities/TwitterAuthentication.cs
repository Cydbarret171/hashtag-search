using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace hashtag_search.Utilities
{
    public class TwitterAuthentication
    {
        private string BearerToken { get; set; }

        private const string _consumerKey = "lOeUOX2vWUUt4NRYz99vJ3aJY";

        private const string _consumerSecret = "0JK0Q5gq1IJ1XJAE6zSfevVMqS58OSqOX3TI2gLRLbhe3bw8Dm";

        private const string _url = "https://api.twitter.com/oauth2/token";

        public TwitterAuthentication()
        {
            RefreshBearerToken();
        }

        public string GetBearerToken()
        {
            return BearerToken;
        }

        private string GetBase64RequestToken()
        {
            var concatenated = HttpUtility.UrlEncode(_consumerKey) + ":" + HttpUtility.UrlEncode(_consumerSecret);

            var bytes = System.Text.Encoding.UTF8.GetBytes(concatenated);

            return Convert.ToBase64String(bytes);
        }

        private void RefreshBearerToken()
        {
            using(var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", GetBasicAuthorizationHeader());
                
                var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = client.PostAsync(_url, content).Result;

                var authenticationResponse = response.Content.ReadAsStringAsync().Result;

                var oauth = ParseOauthResponse(authenticationResponse);

                if (oauth != null) BearerToken = oauth.AccessToken;
            }
        }

        private string GetBasicAuthorizationHeader()
        {
            return $"Basic {GetBase64RequestToken()}";
        }

        private AuthenticationResponse ParseOauthResponse(string response)
        {
            if (string.IsNullOrEmpty(response)) return null;

            using (var reader = new StringReader(response))
            {
                var deserializer = JsonSerializer.Create();

                return (AuthenticationResponse)deserializer.Deserialize(reader, typeof(AuthenticationResponse));
            }
        }
    }

    public class AuthenticationResponse
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}