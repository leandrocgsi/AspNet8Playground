using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RestWithASPNETUdemy.Configuration;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Repository;
using RestWithASPNETUdemy.Services;

namespace RestWithASPNETUdemy.Business.Implementations
{
    public class LoginBusinessImplementation : ILoginBusiness
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        private TokenConfiguration _configurations;

        private IUserRepository _repository;
        private readonly ITokenService _tokenService;
        public LoginBusinessImplementation(IUserRepository repository, ITokenService tokenService, TokenConfiguration configurations)
        {
            _repository = repository;
            _tokenService = tokenService;
            _configurations = configurations;
        }

        public TokenVO ValidateCredentials(UserVO userCredentials)
        {

            var user = _repository.ValidateCredentials(userCredentials);

            if (user == null) return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configurations.DaysToExpiry);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configurations.Minutes);

            _repository.RefreshUserInfo(user);
            return new TokenVO(
                        true,
                        createDate.ToString(DATE_FORMAT),
                        expirationDate.ToString(DATE_FORMAT),
                        accessToken,
                        refreshToken);
        }

        public TokenVO ValidateCredentials(TokenVO token)
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
            DateTime expirationDate = createDate.AddMinutes(_configurations.Minutes);

            _repository.RefreshUserInfo(user);
            return new TokenVO(
                        true,
                        createDate.ToString(DATE_FORMAT),
                        expirationDate.ToString(DATE_FORMAT),
                        accessToken,
                        refreshToken);
        }
        public bool RevokeToken(string username)
        {
            return _repository.RevokeToken(username);
        }
    }
}
