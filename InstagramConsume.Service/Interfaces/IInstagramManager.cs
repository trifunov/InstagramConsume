using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InstagramConsume.Service.Interfaces
{
    public interface IInstagramManager
    {
        Task<JObject> GetAccessTokenAsync(string code);
        Task<JObject> GetData(string accessToken);
        void GetCodeAsync();
    }
}
