using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.Web.Attributes
{
    public class ApplicationHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            //使用log4net或其他记录错误消息
            Exception Error = filterContext.Exception;
            string Message = Error.Message;//错误信息
            string Url = HttpContext.Current.Request.RawUrl;//错误发生地址

            //修改日志存放路径
            //LogHelp.ChangeLoggerFile(LogHelp.MVCLogDirectory);
            //日志记录
            //logger.Error("URL:" + HttpContext.Current.Request.Url, filterContext.Exception);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.Result = new RedirectResult("/Home/Error500/?error=" + Message);//跳转至错误提示页面
        }
    }
}