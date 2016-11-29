using HardaGroup.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess
{
    public class D_Role
    {
        HardaDBContext _dbContext = new HardaDBContext();

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="role"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<Role> GetPageData(Role role, int skip, int limit)
        {
            var query = _dbContext.Roles.Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(role.Name))
                query = query.Where(a => a.Name.Contains(role.Name));

            #endregion

            var identityRoles = query.OrderBy(r => r.Name)
            .Skip(skip)
            .Take(limit)
            .ToList();

            var result = identityRoles
            .Select(r => ConverIdentityRoleToRole(r))
                .ToList();

            return result;
        }

        public int GetPageDataTotalCount(Role role)
        {
            var query = _dbContext.Roles.Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(role.Name))
                query = query.Where(a => a.Name.Contains(role.Name));

            #endregion

            var result = query.ToList().Count();

            return result;
        }

        /// <summary>
        /// 根据ID查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Role GetRoleById(string id)
        {
            var identityRole = _dbContext.Roles.FirstOrDefault(a => a.Id.ToString() == id);
            var result = ConverIdentityRoleToRole(identityRole);
            return result;
        }

        public List<Role> GetByMulitCondition(Role searchModel)
        {
            var query = _dbContext.Roles.Where(n => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(searchModel.Id))
                query = query.Where(a => a.Id == searchModel.Id);
            if (!string.IsNullOrEmpty(searchModel.Name))
                query = query.Where(a => a.Name == searchModel.Name);
            #endregion

            var identityRoles = query
            .ToList();

            var result = identityRoles
            .Select(r => ConverIdentityRoleToRole(r))
                .ToList();

            return result;
        }


        public List<Role> GetAll()
        {
            var identityRoles = _dbContext.Roles
                .ToList();
            var result = identityRoles
                .Select(r => ConverIdentityRoleToRole(r))
                .ToList();

            return result;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public string AddRole(Role role)
        {
            var identityRole = new IdentityRole()
            {
                Name = role.Name
            };
            _dbContext.Roles.Add(identityRole);
            _dbContext.SaveChanges();
            return identityRole.Id;
        }

        public string EditRole(string id, Role model)
        {
            var role = _dbContext.Roles.FirstOrDefault(a => a.Id.ToString() == id);

            role.Name = model.Name;

            _dbContext.SaveChanges();
            return role.Id.ToString();
        }



        public void DeleteRole(string id)
        {
            var role = _dbContext.Roles.Where(i => i.Id == id).FirstOrDefault();
            if (role != null)
            {
                _dbContext.Roles.Remove(role);
                _dbContext.SaveChanges();
            }
        }

        public void BatchDeleteRole(List<string> ids)
        {
            foreach (var id in ids)
            {
                var role = _dbContext.Roles.Where(i => i.Id == id).FirstOrDefault();
                if (role != null)
                {
                    _dbContext.Roles.Remove(role);
                }

            }
            _dbContext.SaveChanges();

        }

        public IdentityRole ConverRoleToIdentityRole(Role role)
        {
            IdentityRole result = new IdentityRole();
            if (role != null)
            {
                result.Id = role.Id;
                result.Name = role.Name;
            }
            return result;
        }

        public Role ConverIdentityRoleToRole(IdentityRole identityRole)
        {
            Role result = new Role();
            if (identityRole != null)
            {
                result.Id = identityRole.Id;
                result.Name = identityRole.Name;
            }
            return result;
        }
    }
}
