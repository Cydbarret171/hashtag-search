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

        //base 64 encoded so plain key not stored
        private const string _consumerKeyBase64 = "bE9lVU9YMnZXVVV0NE5SWXo5OXZKM2FKWQ==";  

        //base 64 encoded so plain secret not stored
        private const string _consumerSecret64 = "MEpLMFE1Z3ExSUoxWEpBRTZ6U2ZldlZNcVM1OE9TcU9YM1RJMmdMUkxiaGUzYnc4RG0=";

        private const string _url = "https://api.twitter.com/oauth2/token";

        public TwitterAuthentication()
        {
            RefreshBearerToken();
        }

        public string GetBearerToken()
        {
            return BearerToken;
        }

        private string Base64DecodeString(string encodedString)
        {
            var base64EncodedBytes = Convert.FromBase64String(encodedString);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private string GetBase64RequestToken()
        {
            var concatenated = HttpUtility.UrlEncode(Base64DecodeString(_consumerKeyBase64)) + ":" + HttpUtility.UrlEncode(Base64DecodeString(_consumerSecret64));

            var bytes = Encoding.UTF8.GetBytes(concatenated);

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