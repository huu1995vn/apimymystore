using Microsoft.AspNetCore.Mvc;
using APIMyMyStore.Services;
using APIMyMyStore.Models;
using APIMyMyStore.Helpers;
using RaoXeAPI.Controllers;

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

        [HttpPost]
        public IActionResult Token(TokenRequest model)
        {
            var response = _TokenService.CreateToken(model);

           
            return Ok(()=>
            {
                 if (response == null)
                 {
                    throw new Exception( CommonConstants.MESSAGE_USER_NOT_VALID);
                 }
                return response;
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var admins = _TokenService.GetAll();
            return Ok(()=>
            {
                return admins;
            });
        }

    }
}
