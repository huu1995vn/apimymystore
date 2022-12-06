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
        public TokenController(ITokenService pTokenService) : base(pTokenService)
        {
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] Dictionary<string, string> data)
        {
            return Ok(() =>
            {
                String username = CommonMethods.ConvertToString(data["username"]);
                String password = CommonMethods.ConvertToString(data["password"]);
                return CreateToken(username, password);
            });
        }

        [Route("refreshlogin")]
        public IActionResult RefreshToken([FromBody] Dictionary<string, string> data)
        {

            return Ok(() =>
            {
                return RefreshToken();
            });
        }

        [Route("logout")]
        [Authorize]
        public IActionResult Logout([FromBody] Dictionary<string, string> data)
        {
            return Ok(() =>
            {
                return RemoveToken();
            });
        }

       

    }
}
