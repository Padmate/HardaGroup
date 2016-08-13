using HardaGroup.Models;
using HardaGroup.Service;
using HardaGroup.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.Web.Controllers
{
    public class SolutionsController:BaseController
    {
        /// <summary>
        /// 组件产品
        /// </summary>
        /// <returns></returns>
        public ActionResult PartsAndModules ()
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_SolutionsBG_PM);
            ViewData["bgimages"] = bgImages;
            return View();
        }

        /// <summary>
        /// 研发
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchAndDevelopment()
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_SolutionsBG_RD);
            ViewData["bgimages"] = bgImages;
            return View();
        }
    }
}