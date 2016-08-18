using HardaGroup.Models;
using HardaGroup.Service;
using HardaGroup.Web.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.Web.Controllers
{
    public class BaseController:Controller
    {
        public string ModelStateError()
        {
            string errorMessageStr = string.Empty;

            var errorMessages = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

            var errorList = errorMessages.ToList();

            string errorMessage = string.Empty;
            foreach (string error in errorList)
            {
                errorMessage += error + "<br/>";
            }

            return errorMessage;
        }

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        public M_User GetCurrentUser()
        {
            M_User user = null;
            if (User.Identity != null)
            {
                B_User bUser = new B_User();
                user = new M_User();
                user = bUser.GetUserByName(User.Identity.Name);

            }

            return user;
        }
    }
}