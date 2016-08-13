﻿using HardaGroup.Models;
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
    public class AboutController:BaseController
    {
        /// <summary>
        /// 关于华尔达
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string typecode)
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_AboutBG);
            ViewData["bgimages"] = bgImages;

            return View();
        }



        public ActionResult GetPageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_About model = JsonHandle.UnJson<M_About>(strReqStream);

            B_About bAbout = new B_About();
            var pageData = bAbout.GetPageData(model);
            var totalCount = bAbout.GetPageDataTotalCount(model);

            PageResult<M_About> pageResult = new PageResult<M_About>(totalCount, pageData);
            return Json(pageResult);
        }

        public ActionResult Add()
        {

            return View();
        }

        // POST:
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveAdd()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_About model = JsonHandle.DeserializeJsonToObject<M_About>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            B_About bAbout = new B_About();
            message = bAbout.AddAbout(model);

            return Json(message);
        }


        public ActionResult Edit(string id)
        {
            B_About bAbout = new B_About();

            Int32 aboutId = System.Convert.ToInt32(id);
            var about = bAbout.GetById(aboutId);

            ViewData["About"] = about;

            return View();
        }

        // POST:
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveEdit()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_About model = JsonHandle.DeserializeJsonToObject<M_About>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            B_About bAbout = new B_About();
            message = bAbout.EditAbout(model);

            return Json(message);
        }

        [HttpPost]
        public ActionResult BachDeleteById()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            List<string> aboutIds = JsonHandle.DeserializeJsonToObject<List<string>>(strReqStream);

            List<int> ids = new List<int>();
            foreach (var aboutid in aboutIds)
            {
                ids.Add(System.Convert.ToInt32(aboutid));
            }
            Message message = new Message();
            B_About bAbout = new B_About();
            message = bAbout.BatchDeleteByIds(ids);
            return Json(message);
        }
    }
}