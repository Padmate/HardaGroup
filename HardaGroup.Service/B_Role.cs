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
    public class B_Role
    {
        D_Role _dRole = new D_Role();

        public M_Role GetRoleById(string id)
        {
            var role = _dRole.GetRoleById(id);
            var result = ConverEntityToModel(role);
            return result;
        }



        public List<M_Role> GetAllData()
        {
            var roles = _dRole.GetAll();
            var result = roles.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public List<M_Role> GetPageData(M_Role role)
        {
            Role searchModel = new Role()
            {
                Name = role.Name
            };

            var offset = role.offset;
            var limit = role.limit;


            var pageResult = _dRole.GetPageData(searchModel, offset, limit);
            var result = pageResult.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetPageDataTotalCount(M_Role role)
        {
            Role searchModel = new Role()
            {
                Name = role.Name
            };

            var totalCount = _dRole.GetPageDataTotalCount(searchModel);
            return totalCount;
        }

        public M_Role ConverEntityToModel(Role role)
        {
            if (role == null) return null;

            var model = new M_Role()
            {
                Id = role.Id.ToString(),
                Name = role.Name

            };
            return model;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message AddRole(M_Role model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "角色新增成功";

            try
            {
                var searchModel = new Role() { Name = model.Name };
                var existRole = _dRole.GetByMulitCondition(searchModel);
                if (existRole.Count > 0)
                {
                    message.Success = false;
                    message.Content = "角色'" + model.Name + "'已存在,不能重复添加。";
                    return message;

                }
                //新增角色
                var role = new Role()
                {
                    Name = model.Name

                };

                message.ReturnStrId = _dRole.AddRole(role);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "角色新增失败，异常：" + e.Message;
            }
            return message;
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Message EditRole(M_Role model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "角色修改成功";

            try
            {
                var searchModel = new Role() { Name = model.Name };
                var existRole = _dRole.GetByMulitCondition(searchModel);
                if (existRole.Where(r => r.Id != model.Id).Count() > 0)
                {
                    message.Success = false;
                    message.Content = "角色'" + model.Name + "'已存在,不能重复添加。";
                    return message;

                }

                var role = new Role()
                {
                    Name = model.Name
                };

                message.ReturnStrId = _dRole.EditRole(model.Id, role);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "角色修改失败，异常：" + e.Message;
            }
            return message;
        }



        public Message DeleteById(string id)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "角色删除成功";

            try
            {
                _dRole.DeleteRole(id);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "角色删除失败：" + e.Message;
            }

            return message;
        }

        public Message BatchDeleteByIds(List<string> ids)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "角色删除成功";

            try
            {
                _dRole.BatchDeleteRole(ids);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "角色删除失败：" + e.Message;
            }

            return message;
        }
    }
}
