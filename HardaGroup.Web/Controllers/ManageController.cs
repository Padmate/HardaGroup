using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using HardaGroup.Web.Models;
using System.Collections;
using HardaGroup.Service;
using HardaGroup.Utility;
using HardaGroup.Models;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace HardaGroup.Web.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 背景图片管理
        /// </summary>
        /// <returns></returns>
        public ActionResult BackgroundImage(string type)
        {
            ViewData["type"] = type;
            return View();
        }

        /// <summary>
        /// 关于华尔达内容管理
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }

        #region 智能制造
        /// <summary>
        /// 智能制造简介
        /// </summary>
        /// <returns></returns>
        public ActionResult ManufacturingIntroduction()
        {
            return View();
        }

        /// <summary>
        /// 制造服务范围
        /// </summary>
        /// <returns></returns>
        public ActionResult ManufacturingServiceScope()
        {
            return View();
        }

        /// <summary>
        /// 制造服务特色
        /// </summary>
        /// <returns></returns>
        public ActionResult ManufacturingServiceFeatures()
        {
            return View();
        }

        #endregion
        

        /// <summary>
        /// 新闻模块管理
        /// </summary>
        /// <returns></returns>
        public ActionResult NewsScope()
        {
            return View();
        }

        /// <summary>
        /// 新闻管理
        /// </summary>
        /// <returns></returns>
        public ActionResult News()
        {
            B_NewsScope bNewsScope = new B_NewsScope();
            var allNewsScopes = bNewsScope.GetAllZHCNData();

            ViewData["jsonlsNewsScope"] = JsonHandle.ToJson(allNewsScopes);
            ViewData["NewsScope"] = allNewsScopes;

            return View();
        }


    }
}

