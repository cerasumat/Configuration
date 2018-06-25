using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Factminr.Configuration.Attributes;

namespace Factminr.Configuration.Demo
{
    [ConfigSet(configRepo: "DistLock", configPath: "src/DistLock.Locker/testConfig.yml", autoSync: true, ConfigRepoOwner = "cerasumat", ConfigNode = "MyConfig2")]
    public class MyConfig
    {
        [ConfigItem(ItemNode = "MySqlConn1")]
        public string MySqlConn = "1";
        [ConfigItem(exclude: true)]
        public string EsConn = "192.168.30.11";
        [ConfigItem(ItemNode = "RedisExpireTime3")]
        public int RedisExpireTime = -1;
    }

    //[ConfigSet(configRepo: "DistLock", configPath: "src/DistLock.Locker/testConfig.yml", autoSync: true)]
    //public class MyConfig
    //{
    //    public string MySqlConn = "1";
    //    public string EsConn = "192.168.30.11";
    //    public int RedisExpireTime = -1;
    //}
}
