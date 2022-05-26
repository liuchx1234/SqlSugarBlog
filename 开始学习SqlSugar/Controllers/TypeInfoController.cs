using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using 开始学习SqlSugar.Helper;

namespace 开始学习SqlSugar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeInfoController : ControllerBase
    {
        private readonly ITypeInfoService _service;
        public TypeInfoController(ITypeInfoService service)
        {
            this._service = service;
        }
        [HttpGet("GetTypeInfo")]
        public async Task<ApiResult> GetTypeInfo()
        {
            var data = await _service.QueryAsync();
            if (data == null) return ApiResultHelper.Error("没有数据");
            return ApiResultHelper.Success(data);
        }
    }
}
