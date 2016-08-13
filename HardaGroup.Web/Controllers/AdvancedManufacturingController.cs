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
    public class AdvancedManufacturingController:BaseController
    {
        /// <summary>
        /// 智能制造
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(){

            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_AdvancedManufacturingBG);
            ViewData["bgimages"] = bgImages;

            return View();
        }
    }
}