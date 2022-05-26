using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using 开始学习SqlSugar.Helper;

namespace 开始学习SqlSugar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class BlogNewsController : ControllerBase
    {
        public readonly IBlogNewsService _service;
        public BlogNewsController(IBlogNewsService service)
        {
            _service = service;
        }

        [HttpGet("GetId")]
        public async Task<ApiResult> GetBlogNews()
        {
            var data = await _service.QueryAsync();
            if (data == null) return ApiResultHelper.Error("没有查询到数据");
            return ApiResultHelper.Success(data);
        }
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ApiResult> Create(string title,string content,int typeid)
        {
            BlogNews blog=new BlogNews()
            {
                BrowseCount = 0,
                Content = content,
                LikeCount = 0,
                Time = DateTime.Now,
                Title = title,
                TypeId = typeid,
                WriteId = 1
            };
                bool b =await _service.CreateAsync(blog);
            if (!b) return ApiResultHelper.Error("添加失败，服务器发生错误");
            return ApiResultHelper.Success(blog);
            
            
        }

        [HttpPost("Update")]
        public async Task<ApiResult> Update(int id,string title,string content,int typeid)
        {
            BlogNews blog = await _service.FindAsync(id);
            if (blog == null) return ApiResultHelper.Error("查询不到该文章");
            blog.Content = content;
            blog.Title = title;
            blog.TypeId = typeid;
            bool b = await _service.EditAsync(blog);
            if (!b) return ApiResultHelper.Error("修改失败，服务器发生错误");
            return ApiResultHelper.Success(b);
        }

        [HttpDelete("Delete")]
        public async Task<ApiResult> Delete(int id)
        {
            bool b= await _service.DeleteAsync(id);
            if (!b) return ApiResultHelper.Error("删除失败");
            return ApiResultHelper.Success(b);
        }
    }
}
