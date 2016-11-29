using HardaGroup.Models;
using HardaGroup.Service;
using HardaGroup.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.Web.Controllers
{
    [Authorize(Roles = SystemRole.SystemAdmin)]
    public class RoleController : BaseController
    {
        [HttpPost]
        public ActionResult GetPageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_Role model = JsonHandle.UnJson<M_Role>(strReqStream);

            B_Role bRole = new B_Role();
            var pageData = bRole.GetPageData(model);
            var totalCount = bRole.GetPageDataTotalCount(model);

            PageResult<M_Role> pageResult = new PageResult<M_Role>(totalCount, pageData);
            return Json(pageResult);
        }


        [HttpPost]
        public ActionResult GetRoleById(string roleid)
        {
            B_Role bRole = new B_Role();

            var role = bRole.GetRoleById(roleid); ;
            return Json(role);
        }

        public ActionResult Detail(string id)
        {
            B_Role bRole = new B_Role();

            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("找不到id为空的数据信息");
            }
            var role = bRole.GetRoleById(id); ;

            ViewData["role"] = role;

            return View();
        }

        public ActionResult Add()
        {

            return View();

        }

        // POST:
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveAdd()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_Role model = JsonHandle.DeserializeJsonToObject<M_Role>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            B_Role bRole = new B_Role();
            message = bRole.AddRole(model);

            return Json(message);
        }

        public ActionResult Edit(string roleId)
        {

            B_Role bRole = new B_Role();
            var role = bRole.GetRoleById(roleId);
            ViewData["role"] = role;


            return View();
        }

        // POST:
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEdit()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_Role model = JsonHandle.DeserializeJsonToObject<M_Role>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            B_Role bRole = new B_Role();
            message = bRole.EditRole(model);

            return Json(message);

        }


        public ActionResult Delete(string RoleId)
        {
            B_Role bRole = new B_Role();
            Message message = bRole.DeleteById(RoleId);

            return Json(message);
        }

        [HttpPost]
        public ActionResult BachDeleteById()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            List<string> contactIds = JsonHandle.DeserializeJsonToObject<List<string>>(strReqStream);

            Message message = new Message();
            B_Role bRole = new B_Role();

            message = bRole.BatchDeleteByIds(contactIds);
            return Json(message);
        }


    }
}