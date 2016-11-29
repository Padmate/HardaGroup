using HardaGroup.Models;
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
    public class ImagesController:BaseController
    {
        #region 背景图片管理

        //允许的图片类型
        public List<string> AllowBGImageExtensions = new List<string>() { ".gif", ".jpg", ".jpeg", ".bmp", ".png" };

        /// <summary>
        /// 获取背景图片
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBGImagesByType(string imageType)
        {
            B_Image _bImage = new B_Image();
            var bgImages = _bImage.GetBGImagesByType(imageType);

            return Json(bgImages);

        }

        /// <summary>
        /// 获取当前国际化对应的背景图片
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCurrentCltureBGImagesByType(string imageType,string culture)
        {
            B_Image _bImage = new B_Image();
            var bgImages = _bImage.GetBGImagesByTypeAndClture(imageType,culture);

            return Json(bgImages);

        }

        /// <summary>
        /// 更新图片顺序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateImageSequence()
        {

            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            List<M_Image> images = JsonHandle.DeserializeJsonToObject<List<M_Image>>(strReqStream);

            B_Image _bImage = new B_Image();
            if (images != null)
            {
                foreach (var image in images)
                {
                    Message message = _bImage.UpdateImageSequence(image.Id, image.Sequence);
                }
            }

            return Json(true);
        }


        /// <summary>
        /// 更新图片链接
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateLinkHref(M_Image image)
        {

            Message message = new Message();
            B_Image _bImage = new B_Image();
            try
            {
                message = _bImage.UpdateLinkHref(image.Id, image.LinkHref);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "链接更新失败,异常：" + e.Message;
            }

            return Json(message);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteImage(int Id)
        {
            Message message = new Message();
            B_Image bImage = new B_Image();

            message = bImage.DeleteImage(Id);

            return Json(message);
        }


        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="model"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = SystemRole.SystemAdmin + "," + SystemRole.BackstageAdmin)]
        public ActionResult UploadBGImageByType(HttpPostedFileBase file,string imageType,string culture)
        {
            //背景图片虚拟目录
            string _bgImageVirtualDirectory = SystemConfig.Init.PathConfiguration["bgImageVirtualDirectory"].ToString();

            Message message = new Message();
            message.Success = true;

            if (file == null)
            {
                message.Success = false;
                message.Content = "未获取到文件信息,请重新尝试";
                return Json(message);
            }
            FileInfo fileInfo = new FileInfo(file.FileName);
            var extension = fileInfo.Extension;
            if (AllowBGImageExtensions.All(i => i != extension))
            {
                message.Success = false;
                message.Content = "图片格式不正确。只支持以下类型:.gif,.jpg,.jpeg,.bmp,.png";
                return Json(message);
            }

            B_Image _bImage = new B_Image();
            message = _bImage.AddCurrentCltureImage(file, _bgImageVirtualDirectory, imageType,culture);

            return Json(message);

        }

        #endregion
    }
}