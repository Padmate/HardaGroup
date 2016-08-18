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
    public class B_User
    {
        D_User _dUser = new D_User();

        public M_User GetUserByName(string userName)
        {
            M_User mUser = null;
            User user = _dUser.GetUserByName(userName);

            if (user != null)
            {
                mUser = new M_User();
                mUser.UserName = user.UserName;
                mUser.Email = user.Email;
                mUser.PhoneNumber = user.PhoneNumber;

            }

            return mUser;
        }
    }
}
