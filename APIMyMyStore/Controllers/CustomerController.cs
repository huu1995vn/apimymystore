using APIMyMyStore.Services;
using Microsoft.AspNetCore.Mvc;
using RaoXeAPI.Controllers;

namespace APIMyMyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : CommonController
    {
        public CustomerController(ITokenService TokenService) : base(TokenService)
        {
        }

        protected override string TableName => "customers";

        protected override string ViewName => "customers";

        protected override string OrderByGrid => "id ASC";
        
        protected override string FieldSelect => Variables.FieldSelectCustomer;

        protected override List<string> FieldInsert => new List<string> { "name", "phone", "email", "address" };

        protected override List<string> FieldUpdate => new List<string> { "name", "phone", "email", "address" };

        // [Authorize]
    }
}
