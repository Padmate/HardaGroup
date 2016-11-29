using HardaGroup.Models;
using HardaGroup.Service;
using HardaGroup.Utility;
using HardaGroup.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.Web.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        [HttpPost]
        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult GetPageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_User model = JsonHandle.UnJson<M_User>(strReqStream);

            //获取当前登录用户
            var currentUser = this.GetCurrentUser();

            B_User bUser = new B_User(currentUser);
            var pageData = bUser.GetPageData(model);
            var totalCount = bUser.GetPageDataTotalCount(model);

            PageResult<M_User> pageResult = new PageResult<M_User>(totalCount, pageData);
            return Json(pageResult);
        }


        [HttpPost]
        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult GetUserById(string userid)
        {
            B_User bUser = new B_User();

            var user = bUser.GetUserById(userid); ;
            return Json(user);
        }


        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult Add()
        {
            //找出所有角色
            B_Role bRole = new B_Role();
            var roles = bRole.GetAllData();
            ViewData["roles"] = roles;


            return View();

        }

        // POST:
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult SaveAdd()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_User model = JsonHandle.DeserializeJsonToObject<M_User>(strReqStream);

            //默认密码为123456
            var defaultPawd = "123456";
            var passwordHash = new PasswordHasher().HashPassword(defaultPawd);
            model.PasswordHash = passwordHash;

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            B_User bUser = new B_User();
            message = bUser.AddUser(model);

            return Json(message);
        }

        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult Edit(string userId)
        {

            B_User bUser = new B_User();
            var user = bUser.GetUserById(userId);
            ViewData["user"] = user;

            //找出所有角色
            B_Role bRole = new B_Role();
            var roles = bRole.GetAllData();
            ViewData["roles"] = roles;


            return View();
        }

        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult Detail(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("找不到id为空的数据信息");
            }


            B_User bUser = new B_User();
            var user = bUser.GetUserById(id);
            ViewData["user"] = user;

            
            //找出所有角色
            B_Role bRole = new B_Role();
            var roles = bRole.GetAllData();
            ViewData["roles"] = roles;


            return View();
        }

        // POST:
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult SaveEdit()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_User model = JsonHandle.DeserializeJsonToObject<M_User>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            B_User bUser = new B_User();
            message = bUser.EditUser(model);

            return Json(message);

        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetUserInfo()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            SetUserInfoModel model = JsonHandle.DeserializeJsonToObject<SetUserInfoModel>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            B_User bUser = new B_User();
            message = bUser.SetUserInfo(model);

            return Json(message);

        }

        [HttpPost]
        public async Task<ActionResult> SetPassword()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            ChangePasswordViewModel model = JsonHandle.DeserializeJsonToObject<ChangePasswordViewModel>(strReqStream);

            Message message = new Message();
            message.Success = true;

            try
            {
                message = model.validate();
                if (!message.Success)
                    return Json(message);
                if (!model.NewPassword.Equals(model.ConfirmPassword))
                {
                    message.Success = false;
                    message.Content = "新密码与确认密码不匹配。";
                    return Json(message);

                }
                var userid = User.Identity.GetUserId();
                var result = await UserManager.ChangePasswordAsync(userid, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        //清除登录cookie信息
                        HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                    }
                }
                else
                {
                    message.Success = false;
                    message.Content = result.Errors.First().ToString();
                }
            }
            catch
            {

                message.Success = false;
                message.Content = "密码更改失败。";

            }
            return Json(message);
        }

        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult Delete(string UserId)
        {
            B_User bUser = new B_User();
            Message message = bUser.DeleteById(UserId);

            return Json(message);
        }

        [HttpPost]
        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult BachDeleteById()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            List<string> contactIds = JsonHandle.DeserializeJsonToObject<List<string>>(strReqStream);

            Message message = new Message();
            B_User bUser = new B_User();

            message = bUser.BatchDeleteByIds(contactIds);
            return Json(message);
        }

       
        /// <summary>
        /// 根据用户Id，加载用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadUserInfoByUserId(string UserId)
        {
            Message message = new Message();
            message.Success = true;
            if (string.IsNullOrEmpty(UserId))
            {
                message.Success = false;
                message.Content = "获取用户信息失败，加载数据失败";
                return Json(message);
            }

            B_User bUser = new B_User();
            M_User user = bUser.GetUserById(UserId);

            message.Content = JsonHandle.ToJson(user);

            return Json(message);
        }

       
    }
}