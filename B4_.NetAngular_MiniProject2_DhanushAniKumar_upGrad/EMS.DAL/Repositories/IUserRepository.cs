using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Repositories
{
    using EMS.DAL.Models;

    public interface IUserRepository
    {
        void Add(UserInfo user);
        UserInfo? GetByEmail(string email);
    }
}
