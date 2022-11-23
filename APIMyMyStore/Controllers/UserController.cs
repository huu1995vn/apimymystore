using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RaoXeAPI.Controllers;

namespace APIMyMyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CommonController
    {
        protected override string TableName => "users";

        protected override string ViewName => "users";

        protected override string OrderByGrid => "id ASC";

        protected override string FieldSelect => Variables.FieldSelectUser;

        protected override List<string> FieldInsert => new List<string> { "name", "phone", "email", "address" };

        protected override List<string> FieldUpdate => new List<string> { "name", "address" };

      
    }
}
