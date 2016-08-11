using HardaGroup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.Web.Controllers
{
    public class IntelligentManufacturingController:BaseController
    {
        /// <summary>
        /// 智能制造
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(){

            List<M_Image> bgImages = new List<M_Image>(){
                new M_Image(){VirtualPath ="/images/intellmake.png", Name="intellmake.png"}
            };
            ViewData["bgimages"] = bgImages;

            return View();
        }
    }
}