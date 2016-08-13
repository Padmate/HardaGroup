using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess
{
    public class D_Image
    {
        HardaDBContext _dbContext = new HardaDBContext();

        /// <summary>
        /// 根据类型获取图片
        /// 按图片顺序从小到大排列
        /// </summary>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public List<Image> GetImagesByType(string imageType)
        {
            var images = _dbContext.Images
                .Where(i => i.Type == imageType)
                .OrderBy(i => i.Sequence)
                .ToList();

            return images;
        }

        /// <summary>
        /// 根据图片类型和当前国际化代码获取图片
        /// 按图片顺序从小到大排列
        /// </summary>
        /// <param name="imageType"></param>
        /// <param name="clture"></param>
        /// <returns></returns>
        public List<Image> GetImagesByTypeAndClture(string imageType,string clture)
        {
            var images = _dbContext.Images
                .Where(i => i.Type == imageType && i.Culture == clture)
                .OrderBy(i => i.Sequence)
                .ToList();

            return images;
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="id"></param>
        public Image GetImageById(int id)
        {
            var image = _dbContext.Images.Where(i => i.Id == id).FirstOrDefault();
            return image;
        }

        /// <summary>
        /// 新增图片
        /// </summary>
        /// <returns></returns>
        public int AddImage(Image image)
        {
            _dbContext.Images.Add(image);
            _dbContext.SaveChanges();
            return image.Id;
        }

        /// <summary>
        /// 更新图片顺序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public int UpdateImageSequence(int id, int sequence)
        {
            var model = _dbContext.Images.Where(i => i.Id == id).FirstOrDefault();
            if (model != null)
            {
                model.Sequence = sequence;
                _dbContext.SaveChanges();
            }
            return model.Id;
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        public void DeleteImage(int id)
        {
            var image = _dbContext.Images.Where(i => i.Id == id).FirstOrDefault();
            if (image != null)
            {
                _dbContext.Images.Remove(image);
                _dbContext.SaveChanges();
            }
        }
    }
}
