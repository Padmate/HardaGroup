using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HardaGroup.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            this.ErrorHandler();
        }

        /// <summary> 
        /// 错误处理 
        /// </summary> 
        private void ErrorHandler()
        {
            Exception error = Server.GetLastError();
            if (error != null)
            {
                string errorPath = "~/500.html";
                int errorCode = 500;

                if(error is HttpException)
                {
                    HttpException httpError = (HttpException)error;
                    // 如果是Http错误，则设置响应的HttpCode
                    int httpCode = httpError.GetHttpCode();

                    if (httpCode == 404)
                    {
                        errorPath = "~/404.html";
                        errorCode = 404;
                    }
                    else
                    {
                        //记录日志
                        WriteMVCLog(HttpContext.Current.Request.Url,error);
                    }
                }
                else
                {
                    //记录日志
                    WriteMVCLog(HttpContext.Current.Request.Url, error);
                }

                //在响应输出之前一定要记得调用 Server.ClearError()方法清除异常。
                //否则会有下面两种情况:
                //如果自定义错误关闭，会显示错误黄页（即详细异常信息页面），
                //如果没关闭则会跳转到自定义错误定义的页面
                Server.ClearError();
                Response.StatusCode = errorCode;
                //Context.Handler = PageParser.GetCompiledPageInstance(path, Server.MapPath(path), Context); 
                Server.Transfer(errorPath);

            }
        } 

        private void WriteMVCLog(Uri url,Exception exception)
        {
            ILog mvclogger = LogManager.GetLogger("MVCLog");
            //日志记录
            mvclogger.Error("URL:" + url, exception);
        }
    }
}
