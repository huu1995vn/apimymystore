using Microsoft.AspNetCore.Mvc;
using APIMyMyStore.Services;
using APIMyMyStore.Models;
using APIMyMyStore.Helpers;
using RaoXeAPI.Controllers;
using Newtonsoft.Json.Linq;

namespace APIMyMyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : CommonController
    {
        private ITokenService _TokenService;

        public TokenController(ITokenService TokenService)
        {
            _TokenService = TokenService;
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] Dictionary<string, string> data)
        {
            return Ok(() =>
            {
                String username = CommonMethods.ConvertToString(data["username"]);
                String password = CommonMethods.GetEncryptMD5(data["password"]);
                return _TokenService.CreateToken(username, password);
            });
        }

        [Route("refreshlogin")]
        // [Authorize]
        public IActionResult RefreshToken([FromBody] Dictionary<string, string> data)
        {

            return Ok(() =>
            {
                string token = Request.Headers[CommonConstants.TOKEN_HEADER_NAME].ToString();
                if(string.IsNullOrEmpty(token))
                {
                    throw new Exception(CommonConstants.MESSAGE_TOKEN_INVALID);
                }
                token = token.Replace("Bearer ", "").Replace("bearer ", "");
                return _TokenService.RefreshToken(token);
            });
        }

        [Route("logout")]
        [Authorize]
        public IActionResult Logout([FromBody] Dictionary<string, string> data)
        {
            return Ok(() =>
            {
                string token = Request.Headers[CommonConstants.TOKEN_HEADER_NAME].ToString();
                if(string.IsNullOrEmpty(token))
                {
                    throw new Exception(CommonConstants.MESSAGE_TOKEN_INVALID);
                }
                token = token.Replace("Bearer ", "").Replace("bearer ", "");
                return _TokenService.RemoveToken(token);
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(() =>
            {
                return _TokenService.GetAll();
            });
        }

    }
}
