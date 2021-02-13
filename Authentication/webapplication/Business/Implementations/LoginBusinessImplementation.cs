using System;
using webapplication.Models;
using webapplication.Repository;

namespace webapplication.Business.Implementations
{
    public class LoginBusinessImplementation : ILoginBusiness
    {
        private IUserRepository _repository;
        public LoginBusinessImplementation(IUserRepository repository)
        {
            _repository = repository;
        }

        public User ValidateCredentials(User user)
        {
            return _repository.ValidateCredentials(user);
        }

        public User ValidateCredentials(string username)
        {
            return _repository.ValidateCredentials(username);
        }

        public User RefreshUserInfo(User user)
        {
            return _repository.RefreshUserInfo(user);
        }

        public bool RevokeToken(string username)
        {
            return _repository.RevokeToken(username);
        }

    }
}
