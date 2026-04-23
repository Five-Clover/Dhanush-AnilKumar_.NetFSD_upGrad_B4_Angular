using EMS.DAL.Data;
using EMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EMSDbContext _context;

        public UserRepository(EMSDbContext context)
        {
            _context = context;
        }

        public void Add(UserInfo user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public UserInfo? GetByEmail(string email)
        {
            return _context.Users
                .FirstOrDefault(u => u.EmailId != null &&
                                     u.EmailId.ToLower() == email.ToLower());
        }
    }
}
