using HardaGroup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.Web.Controllers
{
    public class InnovationController:BaseController
    {
        public ActionResult Index()
        {
            List<M_Image> bgImages = new List<M_Image>(){
                new M_Image(){VirtualPath ="../images/creative.png", Name="creative.png"}
            };
            ViewData["bgimages"] = bgImages;
            return View();
        }
    }
}