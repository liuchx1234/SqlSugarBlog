using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using 开始学习SqlSugar.Helper;

namespace 开始学习SqlSugar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeController : ControllerBase
    {
        private  readonly ITypeInfoService _typeInfoService;

        public TypeController(ITypeInfoService itypeInfoService)
        {
            this._typeInfoService=itypeInfoService;
        }
        [HttpGet("Types")]
        public async Task<ApiResult> Types()
        {
            var types = await _typeInfoService.QueryAsync();
            if (types.Count==0) return ApiResultHelper.Error("查询不到类型");
            return ApiResultHelper.Success(types);
        }

        [HttpGet("Create")]
        public async Task<ApiResult> Create(string name)
        {
            if (string.IsNullOrEmpty(name)) return ApiResultHelper.Error("类型名不能为空");
            TypeInfo typeInfo = new TypeInfo() {Name = name};
            bool b = await _typeInfoService.CreateAsync(typeInfo);
            if (!b) return ApiResultHelper.Error("添加失败");
            return ApiResultHelper.Success(b);
        }

        [HttpPut("Edit")]
        public async Task<ApiResult> Edit(int id, string name)
        {
            var type = await _typeInfoService.FindAsync(id);
            if (type == null) return ApiResultHelper.Error("没有找到该文件类型");
            type.Name = name;
            bool b=await _typeInfoService.EditAsync(type);
            if (!b) return ApiResultHelper.Error("修改失败");
            return ApiResultHelper.Success(b);
        }

        [HttpDelete("Delete")]
        public async Task<ApiResult> Delete(int id)
        {
            bool b=await _typeInfoService.DeleteAsync(id);
            if (!b) return ApiResultHelper.Error("删除失败");
            return ApiResultHelper.Success(b);
        }
    }
}
