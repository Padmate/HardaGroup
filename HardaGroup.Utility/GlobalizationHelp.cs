using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HardaGroup.Utility
{
    /// <summary>
    /// 全球化帮助类
    /// </summary>
    public class GlobalizationHelp
    {
        /// <summary>
        /// 根据当前clture，获取属于当前clture的uri
        /// 只能在URL结束请求前获取
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentThreadCultureUri()
        {
            string cltureUri = string.Empty;
            var currentCluture = Thread.CurrentThread.CurrentCulture;
            if (currentCluture != null && currentCluture.ToString().ToLower() !="zh-cn")
            {
                
                cltureUri = "/" + currentCluture;
            }

            return cltureUri;
        }

        /// <summary>
        /// 获取当前国际化代码
        /// 如果没有设置，则默认为zh-cn
        ///  只能在URL结束请求前获取
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentThreadCultureCode()
        {
            var currentCluture = Thread.CurrentThread.CurrentCulture;
            string code = "zh-cn";
            if (currentCluture != null)
            {
                code = currentCluture.ToString().ToLower();
            }

            return code;
        }
        
    }
}
