using HardaGroup.DataAccess;
using HardaGroup.Entities;
using HardaGroup.Models;
using HardaGroup.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Service
{
    public class B_User
    {
        public B_User()
        {

        }

        M_User _currentUser;
        public B_User(M_User user)
        {
            _currentUser = user;
        }

        D_User _dUser = new D_User();

        public M_User GetUserByName(string userName)
        {

            User user = _dUser.GetUserByName(userName);
            var result = ConverEntityToModel(user);

            return result;
        }

        public M_User GetUserById(string id)
        {
            var user = _dUser.GetUserById(id);
            var result = ConverEntityToModel(user);
            return result;
        }



        public List<M_User> GetAllData()
        {
            var users = _dUser.GetAll();
            var result = users.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<M_User> GetPageData(M_User user)
        {
            User searchModel = new User()
            {
                UserName = user.UserName,
                UserType = user.UserType
            };

            var offset = user.offset;
            var limit = user.limit;


            var pageResult = _dUser.GetPageData(searchModel, offset, limit);
            var result = pageResult.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }


        public List<M_User> GetAdminUsers()
        {
            var users = _dUser.GetAdminUsers();
            var result = users.Select(a=>ConverEntityToModel(a)).ToList();
            return result;
        }

        /// <summary>
        /// 获取分页数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetPageDataTotalCount(M_User user)
        {
            User searchModel = new User()
            {
                UserName = user.UserName,
                UserType = user.UserType

            };

            var totalCount = _dUser.GetPageDataTotalCount(searchModel);
            return totalCount;
        }

        private M_User ConverEntityToModel(User user)
        {
            if (user == null) return null;
            B_Role bRole = new B_Role();
            //是否系统管理员
            bool isSystemAdmin = user.Roles == null?false :
                user.Roles.Any(r => r.Name == SystemRole.SystemAdmin) ? true : false;

            //是否后台管理员
            bool isBackstageAdmin = user.Roles ==null ? false :
                user.Roles.Any(r => r.Name == SystemRole.BackstageAdmin) ? true : false;

            var model = new M_User()
            {
                Id = user.Id.ToString(),
                UserName = user.UserName,
                UserType = user.UserType,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = user.Roles == null ?null : user.Roles.Select(r => bRole.ConverEntityToModel(r)).ToList(),
                IsSystemAdmin = isSystemAdmin,
                IsBackstageAdmin = isBackstageAdmin
            };
            return model;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message AddUser(M_User model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "用户新增成功";

            try
            {
                var searchModel = new User() { UserName = model.UserName };
                var existUser = _dUser.GetByMulitCondition(searchModel);
                if (existUser.Count > 0)
                {
                    message.Success = false;
                    message.Content = "用户'" + model.UserName + "'已存在,不能重复添加。";
                    return message;

                }
                //新增用户
                var user = new User()
                {
                    UserName = model.UserName,
                    UserType = model.UserType,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PasswordHash = model.PasswordHash,
                    Roles = (model.Roles != null && model.Roles.Count > 0) ?
                        model.Roles.Select(r => new Role()
                        {
                            Id = r.Id
                        }).ToList() : null

                };

                message.ReturnStrId = _dUser.AddUser(user);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "用户新增失败，异常：" + e.Message;
            }
            return message;
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Message EditUser(M_User model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "用户信息修改成功";

            try
            {
                var searchModel = new User() { UserName = model.UserName };
                var existUser = _dUser.GetByMulitCondition(searchModel);
                if (existUser.Where(r => r.Id != model.Id).Count() > 0)
                {
                    message.Success = false;
                    message.Content = "用户'" + model.UserName + "'已存在,不能重复添加。";
                    return message;

                }

                var user = new User()
                {
                    UserName = model.UserName,
                    UserType = model.UserType,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PasswordHash = model.PasswordHash,
                    Roles = (model.Roles != null && model.Roles.Count > 0) ?
                        model.Roles.Select(r => new Role()
                        {
                            Id = r.Id
                        }).ToList() : null
                };

                message.ReturnStrId = _dUser.EditUser(model.Id, user);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "用户信息修改失败，异常：" + e.Message;
            }
            return message;
        }

        public Message SetUserInfo(SetUserInfoModel model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "用户信息修改成功";

            try
            {

                var user = new User()
                {
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                message.ReturnStrId = _dUser.SetUserInfo(model.Id, user);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "用户信息修改失败。";
            }
            return message;
        }


        public Message DeleteById(string id)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "用户删除成功";

            try
            {
                _dUser.DeleteUser(id);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "用户删除失败：" + e.Message;
            }

            return message;
        }

        public Message BatchDeleteByIds(List<string> ids)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "用户删除成功";

            try
            {
                _dUser.BatchDeleteUser(ids);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "用户删除失败：" + e.Message;
            }

            return message;
        }
    }
}
