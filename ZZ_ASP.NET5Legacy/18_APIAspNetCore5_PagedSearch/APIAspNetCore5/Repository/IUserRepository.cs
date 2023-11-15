using APIAspNetCore5.Model;

namespace APIAspNetCore5.Repository
{
    public interface IUserRepository
    {
        User FindByLogin(string login);
    }
}