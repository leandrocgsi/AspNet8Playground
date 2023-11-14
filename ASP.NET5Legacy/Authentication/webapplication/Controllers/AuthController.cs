using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Models;

namespace RestWithASPNETUdemy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ILoginBusiness _loginBusiness;

        public AuthController(ILoginBusiness loginBusiness)
        {
            _loginBusiness = loginBusiness;
        }

        [HttpPost]
        [Route("signin")]
        public IActionResult Login([FromBody]User userCredentials)
        {
            if (userCredentials == null) return BadRequest("Invalid client request");

            var tokenResponse = _loginBusiness.ValidateCredentials(userCredentials);

            if (tokenResponse == null) return Unauthorized();

            return Ok(tokenResponse);
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] TokenResponse token)
        {
            if (token is null) return BadRequest("Invalid client request");

            var user = _loginBusiness.ValidateCredentials(token);

            if (user == null) BadRequest("Invalid client request");

            return Ok(user);
        }

        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;

            if (!_loginBusiness.RevokeToken(username)) return BadRequest("Invalid client request");
            return NoContent();
        }
    }
}