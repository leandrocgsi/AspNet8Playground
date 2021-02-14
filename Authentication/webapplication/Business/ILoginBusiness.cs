using webapplication.Models;

namespace webapplication.Business
{
    public interface ILoginBusiness
    {
        TokenResponse ValidateCredentials(User user);

        TokenResponse ValidateCredentials(TokenResponse token);

        bool RevokeToken(string username);
    }
}
