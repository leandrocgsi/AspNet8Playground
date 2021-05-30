using RestWithASPNETUdemy.Data.VO;
using DomainModel.Model;

namespace DataAccessMySqlProvider.Repository
{
    public interface IUserRepository
    {
        User ValidateCredentials(UserVO user);

        User ValidateCredentials(string username);

        bool RevokeToken(string username);

        User RefreshUserInfo(User user);
    }
}
