using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Utility
{
    public class SystemRole
    {
        /// <summary>
        /// 系统管理员(所有权限)
        /// </summary>
        public const string SystemAdmin = "SystemAdmin";

        /// <summary>
        /// 后台管理员
        /// </summary>
        public const string BackstageAdmin = "BackstageAdmin";


        public static Dictionary<string, string> Dic_Roles = new Dictionary<string, string>(){
            {SystemAdmin,"系统管理员"},
            {BackstageAdmin,"后台管理员"}
        };

    }
}
