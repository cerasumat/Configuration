using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Factminr.Configuration.Attributes;

namespace Factminr.Configuration
{
    public abstract class ConfigLoader
    {
        /// <summary>
        /// 配置信息的基础路径，如github/gitlab的url，或etcd/zookeeper的url等，必须由具体的实现类重写
        /// </summary>
        public abstract string ConfigBaseUrl { get; set; }

        protected string ConfigRepo { get; set; }
        protected string ConfigPath { get; set; }
        protected int SyncPeriod { get; set; }
        protected string ConfigNode { get; set; }
        protected string ConfigRepoOwner { get; set; }
        protected string ConfigRepoBranch { get; set; }

        public void Load(object configObj)
        {
            //获取配置信息类类型
            Type t = configObj.GetType();
            //获取配置信息类的class attribute
            //if (t.IsDefined(typeof(ConfigSetAttribute), false)) return;
            var setAttr = t.GetCustomAttributes(false).FirstOrDefault() as ConfigSetAttribute;
            //如果未配置class attribute则返回
            if (null == setAttr || string.IsNullOrEmpty(setAttr.ConfigPath))
                return;
            ConfigRepo = setAttr.ConfigRepo;
            ConfigPath = setAttr.ConfigPath;
            SyncPeriod = setAttr.SyncPeriodInSeconds;
            ConfigNode = string.IsNullOrEmpty(setAttr.ConfigNode)? t.Name:setAttr.ConfigNode;
            ConfigRepoOwner = setAttr.ConfigRepoOwner;
            ConfigRepoBranch = setAttr.ConfigRepoBranch;
            if (LoadConfig(configObj) && setAttr.AutoSync)
            {
                // 开启配置信息自动同步
                if (SyncPeriod < 60)
                    SyncPeriod = 60;
                var syncThread = new Thread(new ParameterizedThreadStart(StartSync)) { IsBackground = true };
                syncThread.Start(configObj);
            }
        }

        /// <summary>
        /// 从path指定的github路径获取配置信息，并更新配置信息对象
        /// </summary>
        /// <param name="configObj"></param>
        /// <returns>更新是否成功</returns>
        private bool LoadConfig(object configObj)
        {
            Type t = configObj.GetType();
            var fields = t.GetFields();

            var gitConfigurations = GetConfigurations();
            if (gitConfigurations.Count == 0) return false;

            foreach (var field in fields)
            {
                // 配置信息必须显式配置为排除，才不从远端数据源获取值
                var itemAttr = field.GetCustomAttributes(typeof(ConfigItemAttribute), false).FirstOrDefault() as ConfigItemAttribute;
                if (itemAttr!=null && itemAttr.Exclude) continue;
                var name = (itemAttr == null || string.IsNullOrEmpty(itemAttr.ItemNode))
                    ? field.Name
                    : itemAttr.ItemNode;
                if (!gitConfigurations.ContainsKey(name)) continue;
                var value = gitConfigurations.First(c => c.Key == name).Value;
                if (null == value) field.SetValue(configObj, null);
                field.SetValue(configObj, Convert.ChangeType(value,field.FieldType));
            }
            return true;
        }

        /// <summary>
        /// 以预定义的同步间隔时间开始同步配置信息
        /// </summary>
        /// <param name="configObj"></param>
        private void StartSync(object configObj)
        {
            while (true)
            {
                Thread.Sleep(SyncPeriod * 1000);
                LoadConfig(configObj);
            }
        }

        /// <summary>
        /// 从特定数据源获取配置信息字典
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, object> GetConfigurations();
    }
}
