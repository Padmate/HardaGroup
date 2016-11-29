using HardaGroup.DataAccess;
using HardaGroup.Entities;
using HardaGroup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Service
{
    public class B_Mail
    {
        D_Mail _dMail = new D_Mail();

        public B_Mail()
        {

        }

        M_User _currentUser;
        string _mapPath;
        public B_Mail(M_User currentUser)
        {
            _currentUser = currentUser;

        }

        public B_Mail(M_User currentUser, string mapPath)
        {
            _currentUser = currentUser;
            _mapPath = mapPath;

        }



        public M_Mail GetMailById(int id)
        {
            B_Image bImage = new B_Image();

            var mail = _dMail.GetMailById(id);
            var result = ConverEntityToModel(mail);
            return result;
        }

        public List<M_Mail> GetAllData()
        {
            var contacts = _dMail.GetAll();
            var result = contacts.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        public List<M_Mail> GetAllUnSendMail()
        {
            var contacts = _dMail.GetAllUnSendMail();
            var result = contacts.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        public List<M_Mail> GetAllUnReadMail()
        {
            var contacts = _dMail.GetAllUnReadMail();
            var result = contacts.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public List<M_Mail> GetPageData(M_Mail contact)
        {
            Mail searchModel = new Mail()
            {
                Subject = contact.Subject
            };

            var offset = contact.offset;
            var limit = contact.limit;


            var pageResult = _dMail.GetPageData(searchModel, contact.SendTagSearch, contact.ReadTagSearch, offset, limit);
            var result = pageResult.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetPageDataTotalCount(M_Mail contact)
        {
            Mail searchModel = new Mail()
            {
                Subject = contact.Subject
            };

            var totalCount = _dMail.GetPageDataTotalCount(searchModel, contact.SendTagSearch, contact.ReadTagSearch);
            return totalCount;
        }

        /// <summary>
        /// 新增邮件
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message AddMail(M_Mail model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "邮件新增成功";

            try
            {
                //新增邮件
                var mail = new Mail()
                {
                    From = model.From,
                    To = model.To,
                    Cc = model.Cc,
                    Subject = model.Subject,
                    Body = model.Body,
                    CreateDate = DateTime.Now,
                    Creator = string.IsNullOrEmpty(model.Creator) ? _currentUser.UserName : model.Creator,
                    SendTag = model.SendTag,
                    SendDate = model.SendDate
                };

                message.ReturnId = _dMail.AddMail(mail);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "邮件新增失败，异常：" + e.Message;
            }
            return message;
        }

        /// <summary>
        /// 修改邮件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Message EditMail(M_Mail model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "邮件修改成功";

            try
            {
                var mail = new Mail()
                {
                    From = model.From,
                    To = model.To,
                    Cc = model.Cc,
                    Subject = model.Subject,
                    Body = model.Body,
                    ModifiedDate = DateTime.Now,
                    Modifier = _currentUser.UserName,
                    SendTag = model.SendTag,
                    SendDate = model.SendDate
                };

                message.ReturnId = _dMail.EditMail(System.Convert.ToInt32(model.Id), mail);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "邮件修改失败，异常：" + e.Message;
            }
            return message;
        }

        public Message EditSendTag(int id, bool sendtag)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "邮件发送状态修改成功";

            try
            {
                _dMail.EditSendStatus(id, sendtag);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "邮件发送状态修改失败，异常：" + e.Message;
            }
            return message;
        }


        public Message EditReadTag(int id, bool readtag)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "邮件读取状态修改成功";

            try
            {
                _dMail.EditReadStatus(id, readtag);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "邮件读取状态修改失败，异常：" + e.Message;
            }
            return message;
        }


        public Message DeleteMail(int id)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "邮件删除成功";
            try
            {
                _dMail.DeleteMail(id);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "邮件删除失败，异常：" + e.Message;
            }
            return message;
        }

        public Message BatchDeleteByIds(List<int> ids)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "邮件删除成功";

            try
            {
                _dMail.BatchDeleteMail(ids);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "邮件删除失败：" + e.Message;
            }

            return message;
        }

        private M_Mail ConverEntityToModel(Mail mail)
        {
            if (mail == null) return null;

            B_Image bImage = new B_Image();

            var model = new M_Mail()
            {
                Id = mail.Id.ToString(),
                From = mail.From,
                To = mail.To,
                Cc = mail.Cc,
                Subject = mail.Subject,
                Body = mail.Body,
                Creator = mail.Creator,
                CreateDate = mail.CreateDate,
                Modifier = mail.Modifier,
                ModifiedDate = mail.ModifiedDate,
                SendTag = mail.SendTag,
                SendDate = mail.SendDate,
                ReadTag = mail.ReadTag,
                ReadDate = mail.ReadDate
            };
            return model;
        }

    }
}
