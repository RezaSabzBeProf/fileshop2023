using fileshop2023.DataLayer.entities.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileshop2023.Core.Service.Interface
{
    public interface IUserService
    {
        User GetUserByPhone(string phone);

        int AddUser(User user);

        void ChangeActiveCode(string phone);
    }
}
