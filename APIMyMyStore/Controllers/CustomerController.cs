﻿using Microsoft.AspNetCore.Mvc;
using RaoXeAPI.Controllers;

namespace APIMyMyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : CommonController
    {
        protected override string TableName => "customers";

        protected override string ViewName => "customers";

        protected override string OrderByGrid => "id ASC";

        protected override List<string> FieldInsert => new List<string> { "name", "phone", "email", "address" };

        protected override List<string> FieldUpdate => new List<string> { "name", "phone", "email", "address" };

        // [Authorize]
    }
}
