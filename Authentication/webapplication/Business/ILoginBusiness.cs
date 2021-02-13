using webapplication.Models;

namespace webapplication.Business
{
    public interface ILoginBusiness
    {
        User ValidateCredentials(User user);

        User ValidateCredentials(string username);

        User RefreshUserInfo(User user);

        bool RevokeToken(string username);
    }
}
