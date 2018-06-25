using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Factminr.Configuration.Attributes;
using Factminr.Configuration.Yml;
using Newtonsoft.Json;

namespace Factminr.Configuration
{
    public class GithubLoader : ConfigLoader
    {
        public GithubLoader()
        {
            ConfigBaseUrl = "https://api.github.com/repos";
        }

        /// <summary>
        /// 配置信息所在的远端URL，如github/gitlab/etcd/zookeeper等
        /// </summary>
        public sealed override string ConfigBaseUrl { get; set; }

        /// <summary>
        /// 从github获取配置信息，以字典形式返回
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetConfigurations()
        {
            var confDic = new Dictionary<string, object>();
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            //string url = "https://api.github.com/repos/factminr/webapi/contents/Factminr.WebApi/Factminr.WebApi/Web.config";
            string url = string.Format("{0}/{1}/{2}/contents/{3}", ConfigBaseUrl, ConfigRepoOwner, ConfigRepo,
                ConfigPath.TrimStart('/')).TrimEnd('/', ' ');
            if (!string.IsNullOrEmpty(ConfigRepoBranch) && ConfigRepoBranch.ToLower() != "master")
                url += string.Format("?ref={0}", ConfigRepoBranch);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Anything");

            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.StatusCode != HttpStatusCode.OK) return confDic;
            var responseStr = response.Content.ReadAsStringAsync().Result;
            confDic = ParseGithubResponse(responseStr);
            return confDic;
        }

        private Dictionary<string, object> ParseGithubResponse(string responseStr)
        {
            var githubResp = JsonConvert.DeserializeObject<dynamic>(responseStr);
            var configContent = githubResp.content.ToString();
            if (null == configContent) return new Dictionary<string, object>();
            byte[] bytes = Convert.FromBase64String(configContent);
            var configs = Encoding.UTF8.GetString(bytes);
            var ymlParser = new YmlParser(configs.Split('\n'));
            return ymlParser.GetKvsByParentKey(ConfigNode);
        }
    }
}
