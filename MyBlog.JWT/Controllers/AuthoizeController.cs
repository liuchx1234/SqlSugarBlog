using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyBlog.IService;
using MyBlog.JWT.Helper;
using MyBlog.JWT.MD5;

namespace MyBlog.JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoizeController : ControllerBase
    {
        private readonly IWriteInfoService _info;
        private readonly IConfiguration _config;

        public AuthoizeController(IWriteInfoService info,IConfiguration config)
        {
            this._info = info;
            this._config = config;
        }
        [HttpPost("Login")]
        public async Task<ApiResult> Login(string username, string userpwd)
        
        {
            string pwd = MD5Helper.MD5Encrypt32(userpwd);
            //数据校验
              var write=  await _info.FindAsync(c => c.UserName == username&&c.UserPwd==pwd);
              if (write != null)
              {
                //登录成功
                var jwtConfig = _config.GetSection("Jwt");
                //秘钥，就是标头，这里用Hmacsha256算法，需要256bit的密钥
                var securityKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.GetValue<string>("Secret"))), SecurityAlgorithms.HmacSha256);
                //Claim，JwtRegisteredClaimNames中预定义了好多种默认的参数名，也可以像下面的Guid一样自己定义键名.
                //ClaimTypes也预定义了好多类型如role、email、name。Role用于赋予权限，不同的角色可以访问不同的接口
                //相当于有效载荷
                var claims = new Claim[] {
                    new Claim(JwtRegisteredClaimNames.Iss,jwtConfig.GetValue<string>("Iss")),
                    new Claim(JwtRegisteredClaimNames.Aud,jwtConfig.GetValue<string>("Aud")),
                    new Claim("Guid",Guid.NewGuid().ToString("D")),
                    new Claim(ClaimTypes.Name,write.Name),
                    new Claim("Id",write.BaseId.ToString()),
                    new Claim("username",write.UserName)
                };
                SecurityToken securityToken = new JwtSecurityToken(
                    signingCredentials: securityKey,//经过加密后的秘钥
                    expires: DateTime.Now.AddMinutes(2),//过期时间
                    claims: claims//payload
                );
                //生成jwt令牌
                var jwtToken= new JwtSecurityTokenHandler().WriteToken(securityToken);
                return ApiResultHelper.Success(jwtToken);
              }
              else
              {
                  return ApiResultHelper.Error("账号或密码错误");
              }
        }
    }
}
