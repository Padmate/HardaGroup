using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Routing;

namespace HardaGroup.Web.Globalization
{
    public class CultureAwareHttpModule : IHttpModule
    {
        private CultureInfo currentCulture;
        private CultureInfo currentUICulture;

        public void Dispose() { }
        public void Init(HttpApplication context)
        {
            context.BeginRequest += SetCurrentCulture;
            context.EndRequest += RecoverCulture;
        }

        /// <summary>
        /// 每次请求都会进入该方法两次，最后一次的url中language总是为null，导致请求完成后culture总是zh-CN?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SetCurrentCulture(object sender, EventArgs args)
        {
            currentCulture = Thread.CurrentThread.CurrentCulture;
            currentUICulture = Thread.CurrentThread.CurrentUICulture;
            HttpContextBase contextWrapper = new HttpContextWrapper(HttpContext.Current);
            RouteData routeData = RouteTable.Routes.GetRouteData(contextWrapper);
            if (routeData == null)
            {
                return;
            }
            object culture;
            //获取当前url中包含的culture信息,如果没有，则赋默认值
            if (routeData.Values.TryGetValue("language", out culture))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(culture.ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture.ToString());
            }

            //获取当前请求路径
            var url = HttpContext.Current.Request.Url;
            //获取上次请求路径
            var urlRefferrer =HttpContext.Current.Request.UrlReferrer;
            //刷新当前页面
            //HttpContext.Current.Response.Redirect("");
        }

        /// <summary>
        /// 请求完后恢复culture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void RecoverCulture(object sender, EventArgs args)
        {
            Thread.CurrentThread.CurrentCulture = currentCulture;
            Thread.CurrentThread.CurrentUICulture = currentUICulture;
        }
    }
    
}