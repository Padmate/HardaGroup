using HardaGroup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.Web.Controllers
{
    public class SolutionController:BaseController
    {
        /// <summary>
        /// 组件产品
        /// </summary>
        /// <returns></returns>
        public ActionResult ComponentProduct()
        {
            List<M_Image> bgImages = new List<M_Image>(){
                new M_Image(){VirtualPath ="/images/solution.png", Name="solution.png"}
            };
            ViewData["bgimages"] = bgImages;
            return View();
        }

        /// <summary>
        /// 研发
        /// </summary>
        /// <returns></returns>
        public ActionResult Research()
        {
            List<M_Image> bgImages = new List<M_Image>(){
                new M_Image(){VirtualPath ="/images/solution.png", Name="solution.png"}
            };
            ViewData["bgimages"] = bgImages;
            return View();
        }
    }
}