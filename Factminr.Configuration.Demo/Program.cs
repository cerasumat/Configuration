using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Factminr.Configuration.Yml;

namespace Factminr.Configuration.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            MyConfig cfg = new MyConfig();          //配置信息类
            var cfgLoader = new GithubLoader();     //Github Loader 
            cfgLoader.Load(cfg);                    //加载配置信息


            Console.WriteLine(cfg.MySqlConn);
            Console.ReadLine();
        }
    }
}
