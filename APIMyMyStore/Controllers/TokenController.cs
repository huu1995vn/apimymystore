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
            return Ok(()=>
            {
                return _TokenService.CreateToken(model);
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(()=>
            {
                return _TokenService.GetAll();
            });
        }

    }
}
