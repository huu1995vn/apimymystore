using Microsoft.AspNetCore.Mvc;
using APIMyMyStore.Services;
using APIMyMyStore.Helpers;
using RaoXeAPI.Controllers;
using Newtonsoft.Json.Linq;

namespace APIMyMyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : CommonController
    {
        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] Dictionary<string, string> data)
        {
            return Ok(() =>
            {
                String username = CommonMethods.ConvertToString(data["username"]);
                String password = CommonMethods.GetEncryptMD5(data["password"]);
                return new TokenService().CreateToken(username, password);
            });
        }

        [Route("refreshlogin")]
        [HttpPost]
        public IActionResult RefreshToken([FromBody] Dictionary<string, string> data)
        {

            return Ok(() =>
            {
                return new TokenService().RefreshToken(Token());
            });
        }

        [Route("logout")]
        [Authorize]
        public IActionResult Logout([FromBody] Dictionary<string, string> data)
        {
            return Ok(() =>
            {
                return new TokenService().RemoveToken(Token());
            });
        }

       

    }
}
