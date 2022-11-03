using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using APIMyMyStore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RaoXeAPI.Controllers
{
    [ApiController]
    public class CommonController : ControllerBase
    {
        
        [Route("OK")]
        public OkObjectResult Ok(Func<object> pMethod)
        {
            APIResult res = new APIResult();
            try
            {
                res.SetResult(pMethod());
            }
            catch (Exception ex)
            {
                res.SetException(ex);
            }
            return base.Ok(res);
        }
    }
}