using HardaGroup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.Web.Controllers
{
    public class InformationController:BaseController
    {
        public ActionResult Index()
        {
            List<M_Image> bgImages = new List<M_Image>(){
                new M_Image(){VirtualPath ="../images/info.png", Name="info.png"}
            };
            ViewData["bgimages"] = bgImages;
            return View();
        }
    }
}