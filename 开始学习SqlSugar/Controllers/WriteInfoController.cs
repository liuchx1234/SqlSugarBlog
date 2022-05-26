using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using MyBlog.Model.Dto;
using 开始学习SqlSugar.Helper;
using 开始学习SqlSugar.MD5;

namespace 开始学习SqlSugar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WriteInfoController : ControllerBase
    {
        private  readonly IWriteInfoService _service;
        private  readonly  ILogger<WriteInfo> _logger;

        public WriteInfoController(IWriteInfoService iwrInfoService,ILogger<WriteInfo> logger)
        {
            this._service=iwrInfoService;
            _logger = logger;
        }
        [HttpPost("Create")]
        public async Task<ApiResult> Create(string name,string username, string userpwd)
        {
            //数据校验
            WriteInfo writer=new WriteInfo(){Name = name,UserPwd =MD5Helper.MD5Encrypt32(userpwd),UserName = username};
            bool b = await _service.CreateAsync(writer);
        
            var oldWriter = await _service.FindAsync(n => n.UserName == username);
            if (oldWriter != null) return ApiResultHelper.Error("账号已经存在");
            bool c = await _service.CreateAsync(writer);
            if (!c) return ApiResultHelper.Error("添加失败");
            return ApiResultHelper.Success(writer);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("Edit")]
        public async Task<ApiResult> Edit(string name)
        {
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);
            var writer = await _service.FindAsync(id);
            writer.Name=name;
            bool b = await _service.EditAsync(writer);
            if (!b) return ApiResultHelper.Error("修改失败");
            return ApiResultHelper.Success("修改成功");
        }
        [HttpGet("Select")]
        public async Task<ApiResult> Select()
        {
            var date = await _service.QueryAsync();
            Console.Write(_service.Test());
            if (date.Count == 0) return ApiResultHelper.Error("查不到数据");
            return ApiResultHelper.Success(date);
        }

        [HttpGet("Find")]
        public async Task<ApiResult> Find([FromServices]IMapper iMapper,int id)
        {
            var writer = await _service.FindAsync(id);
           var mapper= iMapper.Map<WriteDto>(writer);
           _logger.LogDebug("np");
            return ApiResultHelper.Success(mapper);
        }
    }
}
