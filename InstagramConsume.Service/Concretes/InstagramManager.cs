using InstagramConsume.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InstagramConsume.Service.Concretes
{
    public class InstagramManager : IInstagramManager
    {
        private static HttpClient _currentHttp;
        private static string _instagramUrl; 
        private static string _instagramAppId;
        private static string _instagramAppSecret;

        public InstagramManager(IConfiguration config)
        {
            _instagramUrl = config["InstagramBaseAddress"];
            _instagramAppId = config["InstagramAppId"];
            _instagramAppSecret = config["InstagramAppSecret"];
        }

        private static HttpClient CurrentHttp
        {
            get
            {
                if (_currentHttp == null)
                {
                    _currentHttp = new HttpClient();
                    _currentHttp.DefaultRequestHeaders.Accept.Clear();
                };
                return _currentHttp;
            }
        }

        public async void GetCodeAsync()
        {
            await CurrentHttp.GetAsync("oauth/authorize?client_id=" + _instagramAppId + "&redirect_uri=https://consumeinstagram.azurewebsites.net/api/instagram/getaccesstoken&scope=user_profile,user_media&response_type=code");
        }

        public async Task<JObject> GetAccessTokenAsync(string code)
        {
            var requestDict = new Dictionary<string, string>();
            requestDict.Add("client_id", _instagramAppId);
            requestDict.Add("client_secret", _instagramAppSecret);
            requestDict.Add("grant_type", "authorization_code");
            //requestDict.Add("redirect_uri", "https://consumeinstagram.azurewebsites.net/api/instagram/getaccesstoken");
            requestDict.Add("redirect_uri", "https://localhost:44335/api/instagram/getaccesstoken");
            requestDict.Add("code", code);

            using (var formContent = new FormUrlEncodedContent(requestDict))
            {
                var response = await CurrentHttp.PostAsync(_instagramUrl + "oauth/access_token", formContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
                    return responseContent;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<JObject> GetData(string accessToken)
        {
            var response = await CurrentHttp.GetAsync("https://graph.instagram.com/me/media?fields=id,caption&access_token=" + accessToken);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
                return responseContent;
            }
            else
            {
                return null;
            }
        }
    }
}
