using System;
using System.Collections.Generic;
using System.Security.Claims;
using webapplication.Models;
using webapplication.Repository;
using webapplication.Services;

namespace webapplication.Business.Implementations
{
    public class LoginBusinessImplementation : ILoginBusiness
    {
        private IUserRepository _repository;
        readonly ITokenService _tokenService;
        public LoginBusinessImplementation(IUserRepository repository, ITokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }

        public TokenResponse ValidateCredentials(User userCredentials)
        {

            var user = _repository.ValidateCredentials(userCredentials);

            if (user == null) return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Manager")
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(60);

            _repository.RefreshUserInfo(user);
            return new TokenResponse(
                        true,
                        createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        accessToken,
                        refreshToken);
        }

        public TokenResponse ValidateCredentials(TokenResponse token)
        {
            string accessToken = token.AccessToken;
            string refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;

            var user = _repository.ValidateCredentials(username);

            if (user == null ||
                user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now) return null;

            accessToken = _tokenService.GenerateAccessToken(principal.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(60);

            _repository.RefreshUserInfo(user);
            return new TokenResponse(
                        true,
                        createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        accessToken,
                        refreshToken);
        }
        public bool RevokeToken(string username)
        {
            return _repository.RevokeToken(username);
        }
    }
}
