using RestWithASPNETUdemy.Models;

namespace RestWithASPNETUdemy.Business
{
    public interface ILoginBusiness
    {
        TokenResponse ValidateCredentials(User user);

        TokenResponse ValidateCredentials(TokenResponse token);

        bool RevokeToken(string username);
    }
}
