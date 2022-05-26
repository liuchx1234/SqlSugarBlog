using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace 开始学习SqlSugar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("NotAuthoize")]
        public string  NotAuthoize()
        {
            return "this no authoize";
        }
        [Authorize]
        [HttpGet("Authoize")]
        public string Authoize()
        {
            return "this must authoize";
        }
    }
}
