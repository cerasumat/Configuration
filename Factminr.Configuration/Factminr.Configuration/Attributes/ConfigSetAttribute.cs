using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factminr.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ConfigSetAttribute : Attribute
    {
        private readonly string _configPath;
        private readonly bool _autoSync;
        private int _syncPeriodInSeconds;
        private string _configRepo;
        private string _configRepoOwner;
        private string _configBranch;
        private string _configNode;

        /// <summary>
        /// 配置信息所在路径信息
        /// </summary>
        public string ConfigPath
        {
            get { return _configPath; }
        }

        /// <summary>
        /// 配置项类是否需要自动同步
        /// </summary>
        public bool AutoSync
        {
            get { return _autoSync; }
        }

        /// <summary>
        /// 配置项类自动同步周期（秒）
        /// </summary>
        public int SyncPeriodInSeconds
        {
            get { return _syncPeriodInSeconds; }
            set { _syncPeriodInSeconds = value; }
        }

        /// <summary>
        /// 配置文件所属repository
        /// </summary>
        public string ConfigRepo
        {
            get { return _configRepo; }
            set { _configRepo = value; }
        }

        /// <summary>
        /// 配置文件所属repository的owner name
        /// </summary>
        public string ConfigRepoOwner
        {
            get { return _configRepoOwner; }
            set { _configRepoOwner = value; }
        }

        /// <summary>
        /// 配置文件所在repository的分支，默认develop
        /// </summary>
        public string ConfigRepoBranch
        {
            get { return _configBranch; }
            set { _configBranch = value; }
        }

        /// <summary>
        /// 配置类在yml中对应的根节点，默认为class名
        /// </summary>
        public string ConfigNode
        {
            get { return _configNode; }
            set { _configNode = value; }
        }

        public ConfigSetAttribute(string configRepo, string configPath, bool autoSync, string configNode = "", int syncPeriodInSeconds = -1, string repoOwner = "factminr", string repositoryBranch = "develop")
        {
            _configPath = configPath;
            _autoSync = autoSync;
            _syncPeriodInSeconds = syncPeriodInSeconds;
            _configRepo = configRepo;
            _configRepoOwner = repoOwner;
            _configBranch = repositoryBranch;
        }
    }
}
