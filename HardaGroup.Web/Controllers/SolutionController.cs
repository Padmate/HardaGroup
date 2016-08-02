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
            return View();
        }

        /// <summary>
        /// 研发
        /// </summary>
        /// <returns></returns>
        public ActionResult Research()
        {
            return View();
        }
    }
}