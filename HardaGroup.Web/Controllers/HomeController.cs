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
    public class HomeController : BaseController
    {
        public ActionResult Default()
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_HomeBG);
            ViewData["bgimages"] = bgImages;

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ContactUs()
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_ContactUsBG);
            ViewData["bgimages"] = bgImages;

            return View();
        }

        public ActionResult JoinUs()
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_JoinUsBG);
            ViewData["bgimages"] = bgImages;
            return View();
        }
    }
}