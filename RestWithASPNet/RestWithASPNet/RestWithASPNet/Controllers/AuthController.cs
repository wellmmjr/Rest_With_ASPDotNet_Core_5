using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNet.Business;
using RestWithASPNet.Data.VO;

namespace RestWithASPNet.Controllers
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/api/[controller]")]
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
        public IActionResult SigningIn([FromBody] UserVO userVO)
        {
            if (userVO == null) return BadRequest("Invalid Request");

            var token = _loginBusiness.ValidateCredentials(userVO);

            if (token == null) return Unauthorized();

            return Ok(token);
        }
        
        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] TokenVO tokenVO)
        {
            if (tokenVO == null) return BadRequest("Invalid Request");

            var token = _loginBusiness.ValidateCredentials(tokenVO);

            if (token == null) return BadRequest("Invalid Request");

            return Ok(token);
        }
        
        [HttpGet]
        [Route("revoke")]
        [Authorize("Bearer")]
        public IActionResult RevokeToken()
        {
            var username = User.Identity.Name;

            var result = _loginBusiness.RevokeToken(username);

            if (!result) return BadRequest("Invalid Client Request");

            return NoContent();
        }
    }
}
