using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapplication.Models;

namespace webapplication.Repository.Implementations
{
    public class UserRepositoryImplementation : IUserRepository
    {
        private readonly MySQLContext _context;

        public UserRepositoryImplementation(MySQLContext context)
        {
            _context = context;
        }
        public User ValidateCredentials(string username)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == username);
        }
        public User ValidateCredentials(User user)
        {
            return _context.Users
                .FirstOrDefault(u => (u.UserName == user.UserName) && (u.Password == user.Password));
        }

        public User RefreshUserInfo(User user)
        {
            if (!Exists(user.Id)) return null;

            var result = _context.Users.SingleOrDefault(u => u.UserName == user.UserName);
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        public bool RevokeToken(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == username);
            if (user == null) return false;

            user.RefreshToken = null;
            _context.SaveChanges();
            return true;
        }
        public bool Exists(long? id)
        {
            return _context.Users.Any(b => b.Id.Equals(id));
        }
    }
}
