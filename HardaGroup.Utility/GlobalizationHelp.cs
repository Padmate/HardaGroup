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
        /// </summary>
        /// <returns></returns>
        public static string GetCltureUri()
        {
            string cltureUri = string.Empty;
            var currentCluture = Thread.CurrentThread.CurrentCulture;
            if (currentCluture != null && currentCluture.ToString().ToLower() !="zh-cn"
                && currentCluture.ToString().ToLower() !="zh")
            {
                
                cltureUri = "/" + currentCluture;
            }

            return cltureUri;
        }
        
    }
}
