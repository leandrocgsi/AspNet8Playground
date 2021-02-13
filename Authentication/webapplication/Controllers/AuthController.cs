using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using webapplication.Business;
using webapplication.Business.Implementations;
using webapplication.Models;
using webapplication.Services;

namespace webapplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ILoginBusiness _loginBusiness;
        readonly ITokenService tokenService;

        public AuthController(ILoginBusiness loginBusiness, ITokenService tokenService)
        {
            _loginBusiness = loginBusiness;
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody]User userCredentials)
        {
            if (userCredentials == null)
            {
                return BadRequest("Invalid client request");
            }

            var user = _loginBusiness.ValidateCredentials(userCredentials);

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Manager")
            };

            var accessToken = tokenService.GenerateAccessToken(claims);
            var refreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(60);

            _loginBusiness.RefreshUserInfo(user);

            return Ok(new
            {
                autenticated = true,
                created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = accessToken,
                refreshToken = refreshToken,
                message = "OK"
            });
        }
    }
}