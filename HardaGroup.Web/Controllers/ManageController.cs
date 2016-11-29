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
using System.IO;

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
        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult BackgroundImage(string type)
        {
            ViewData["type"] = type;
            return View();
        }

        /// <summary>
        /// 关于华尔达内容管理
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// 模块管理
        /// Common.Dic_ModuleType
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult Module()
        {
            
            return View();
        }

        /// <summary>
        /// 单个模块编辑页面
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult ModulePage()
        {
            return View();
        }


        /// <summary>
        /// 新闻模块管理
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult NewsScope()
        {
            return View();
        }

        /// <summary>
        /// 新闻管理
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult News()
        {
            B_NewsScope bNewsScope = new B_NewsScope();
            var allNewsScopes = bNewsScope.GetAllZHCNData();

            ViewData["jsonlsNewsScope"] = JsonHandle.ToJson(allNewsScopes);
            ViewData["NewsScope"] = allNewsScopes;
            ViewData["yesno"] = JsonHandle.ToJson(Common.Dic_YesNo);

            return View();
        }


        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult Jobs()
        {
            B_JobScope bJobScope = new B_JobScope();
            var allJobScopes = bJobScope.GetAllZHCNData();

            ViewData["jsonlsJobScope"] = JsonHandle.ToJson(allJobScopes);
            ViewData["JobScope"] = allJobScopes;
            ViewData["yesno"] = JsonHandle.ToJson(Common.Dic_YesNo);
            return View();
        }

        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult JobScope()
        {

            return View();
        }

        [Authorize(Roles = SystemRole.SystemAdmin)]
        public ActionResult UserManage()
        {

            //角色
            ViewData["Roles"] = JsonHandle.ToJson(SystemRole.Dic_Roles);

            //获取当前登录用户
            var currentUser = this.GetCurrentUser();
            ViewData["LoginUser"] = currentUser;
            return View();
        }

        [Authorize(Roles = SystemRole.SystemAdmin)]
        public ActionResult RoleManage()
        {
            //角色
            ViewData["Roles"] = JsonHandle.ToJson(SystemRole.Dic_Roles);

            return View();
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UserInfo()
        {

            //获取当前登录用户
            var loginUser = this.GetCurrentUser();
            B_User bUser = new B_User();
            var user = bUser.GetUserByName(loginUser.UserName);
            ViewData["UserInfo"] = user;
            ViewData["JsonUserInfo"] = JsonHandle.ToJson(user);
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// 系统邮件
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult Mail()
        {
            return View();
        }
        
    }
}

