using APIAspNetCore5.Model;
using APIAspNetCore5.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAspNetCore5.Repository.Implementations
{
    public class UserRepositoryImplementation : IUserRepository
    {
        private readonly MySQLContext _context;

        public UserRepositoryImplementation(MySQLContext context)
        {
            _context = context;
        }

        public User FindByLogin(string login)
        {
            return _context.Users.SingleOrDefault(u => u.Login.Equals(login));
        }
    }
}