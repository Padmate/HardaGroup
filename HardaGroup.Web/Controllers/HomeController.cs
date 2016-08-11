using HardaGroup.Models;
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
            List<M_Image> bgImages = new List<M_Image>(){
                new M_Image(){VirtualPath ="/images/bg1.jpg", Name="bg1.jpg"},
                new M_Image(){VirtualPath ="/images/bg2.jpg", Name="bg2.jpg"},
                new M_Image(){VirtualPath ="/images/bg3.jpg", Name="bg3.jpg"}
            };
            ViewData["bgimages"] = bgImages;

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult About()
        {
            List<M_Image> bgImages = new List<M_Image>(){
                new M_Image(){VirtualPath ="/images/about.png", Name="about.png"}
            };
            ViewData["bgimages"] = bgImages;

            return View();
        }

        public ActionResult Contact()
        {
            List<M_Image> bgImages = new List<M_Image>(){
                new M_Image(){VirtualPath ="/images/contact.png", Name="contact.png"}
            };
            ViewData["bgimages"] = bgImages;

            return View();
        }

        public ActionResult JoinUs()
        {
            List<M_Image> bgImages = new List<M_Image>(){
                new M_Image(){VirtualPath ="/images/joinus.png", Name="joinus.png"}
            };
            ViewData["bgimages"] = bgImages;
            return View();
        }
    }
}