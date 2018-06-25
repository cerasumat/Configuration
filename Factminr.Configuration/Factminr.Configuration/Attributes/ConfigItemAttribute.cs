using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factminr.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Field , AllowMultiple = false, Inherited = false)]
    public class ConfigItemAttribute : Attribute
    {
        private readonly bool _isExclude;
        private string _itemNode;

        /// <summary>
        /// 是否将配置项从配置集同步计划中排除
        /// </summary>
        public bool Exclude
        {
            get { return _isExclude; }
        }

        /// <summary>
        /// 配置项在yml中对应的节点名，默认为field名
        /// </summary>
        public string ItemNode
        {
            get { return _itemNode; }
            set { _itemNode = value; }
        }

        public ConfigItemAttribute(string itemNode="", bool exclude = false)
        {
            _isExclude = exclude;
            _itemNode = itemNode;
        }
    }
}
