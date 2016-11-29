using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess
{
    public class D_Mail
    {
        HardaDBContext _dbContext = new HardaDBContext();

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<Mail> GetPageData(Mail mail, string sendtagsearch, string readtagsearch, int skip, int limit)
        {
            var query = _dbContext.Mails.Where(a => 1 == 1);

            #region　条件过滤\
            if (!string.IsNullOrEmpty(mail.Subject))
                query = query.Where(a => a.Subject.Contains(mail.Subject));
            if (!string.IsNullOrEmpty(sendtagsearch))
            {
                if (sendtagsearch == "0")
                {
                    query = query.Where(a => a.SendTag == true);

                }
                else
                {
                    query = query.Where(a => a.SendTag == false);

                }
            }
            if (!string.IsNullOrEmpty(readtagsearch))
            {
                if (readtagsearch == "0")
                {
                    query = query.Where(a => a.ReadTag == true);

                }
                else
                {
                    query = query.Where(a => a.ReadTag == false);

                }
            }
            #endregion

            var result = query.OrderByDescending(a => a.CreateDate)
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;
        }

        public int GetPageDataTotalCount(Mail mail, string sendtagsearch, string readtagsearch)
        {
            var query = _dbContext.Mails.Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(mail.Subject))
                query = query.Where(a => a.Subject.Contains(mail.Subject));
            if (!string.IsNullOrEmpty(sendtagsearch))
            {
                if (sendtagsearch == "0")
                {
                    query = query.Where(a => a.SendTag == true);

                }
                else
                {
                    query = query.Where(a => a.SendTag == false);

                }
            }
            if (!string.IsNullOrEmpty(readtagsearch))
            {
                if (readtagsearch == "0")
                {
                    query = query.Where(a => a.ReadTag == true);

                }
                else
                {
                    query = query.Where(a => a.ReadTag == false);

                }
            }
            #endregion

            var result = query.ToList().Count();

            return result;
        }

        /// <summary>
        /// 根据ID查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Mail GetMailById(int id)
        {
            var mail = _dbContext.Mails.FirstOrDefault(a => a.Id == id);
            return mail;
        }

        public List<Mail> GetAll()
        {
            var mails = _dbContext.Mails.ToList();
            return mails;
        }

        /// <summary>
        /// 查询所有未发送的邮件
        /// </summary>
        /// <returns></returns>
        public List<Mail> GetAllUnSendMail()
        {
            var mails = _dbContext.Mails.Where(m => m.SendTag == false).ToList();
            return mails;
        }

        /// <summary>
        /// 查询所有未读取的邮件
        /// </summary>
        /// <returns></returns>
        public List<Mail> GetAllUnReadMail()
        {
            var mails = _dbContext.Mails.Where(m => m.ReadTag == false).ToList();
            return mails;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public int AddMail(Mail mail)
        {
            _dbContext.Mails.Add(mail);
            _dbContext.SaveChanges();
            return mail.Id;
        }

        public int EditMail(int id, Mail model)
        {
            var mail = _dbContext.Mails.FirstOrDefault(a => a.Id == id);

            mail.Subject = model.Subject;
            mail.From = model.From;
            mail.To = model.To;
            mail.Cc = model.Cc;
            mail.Body = model.Body;
            mail.SendDate = model.SendDate;
            mail.SendTag = model.SendTag;
            mail.Modifier = model.Modifier;
            mail.ModifiedDate = model.ModifiedDate;


            _dbContext.SaveChanges();
            return mail.Id;
        }

        /// <summary>
        /// 修改发送状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public int EditSendStatus(int id, bool sendtag)
        {
            var mail = _dbContext.Mails.FirstOrDefault(a => a.Id == id);

            mail.SendDate = DateTime.Now;
            mail.SendTag = sendtag;
            _dbContext.SaveChanges();
            return mail.Id;
        }

        /// <summary>
        /// 修改读取状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public int EditReadStatus(int id, bool readtag)
        {
            var mail = _dbContext.Mails.FirstOrDefault(a => a.Id == id);

            mail.ReadDate = DateTime.Now;
            mail.ReadTag = readtag;

            _dbContext.SaveChanges();
            return mail.Id;
        }


        public void DeleteMail(int id)
        {
            var mail = _dbContext.Mails.Where(i => i.Id == id).FirstOrDefault();
            if (mail != null)
            {
                _dbContext.Mails.Remove(mail);
                _dbContext.SaveChanges();
            }
        }

        public void BatchDeleteMail(List<int> ids)
        {
            var mails = _dbContext.Mails.Where(i => ids.Contains(i.Id)).ToList();
            if (mails.Count > 0)
            {
                _dbContext.Mails.RemoveRange(mails);
                _dbContext.SaveChanges();
            }
        }

    }
}
