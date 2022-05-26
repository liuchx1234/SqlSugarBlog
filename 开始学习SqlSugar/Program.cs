using System.Configuration;
using System.Configuration.Internal;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyBlog.IRepository;
using MyBlog.IService;
using MyBlog.Repository;
using MyBlog.Service;
using SqlSugar.IOC;
using 开始学习SqlSugar.Utility.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
      {
           c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
           c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
           {
               Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
               Name = "Authorization",
               In = ParameterLocation.Header,
               Type = SecuritySchemeType.ApiKey,
               BearerFormat = "JWT",
               Scheme = "Bearer"
           });
           c.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
                   {
                       new OpenApiSecurityScheme{
                           Reference = new OpenApiReference {
                               Type = ReferenceType.SecurityScheme,
                               Id = "Bearer"}
                       },new string[] { }
                   }
               });
     });


#region 注入SqlSugar.IOC

builder.Services.AddSqlSugar(new IocConfig()
{
    //ConfigId="db01"  多租户用到
    ConnectionString = builder.Configuration.GetConnectionString("MySQLConnection"),
    DbType = IocDbType.MySql,
    IsAutoCloseConnection = true//自动释放
});

#endregion

#region IOC依赖注入

builder.Services.AddCustomIOC();
#endregion


#region AutoMapper

builder.Services.AddAutoMapper(typeof(CustomAutoMapperProfile));

#endregion
#region 添加JWT

var jwtConfig = builder.Configuration.GetSection("Jwt");
//生成密钥
var symmetricKeyAsBase64 = jwtConfig.GetValue<string>("Secret");
var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
var signingKey = new SymmetricSecurityKey(keyByteArray);
//认证参数
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,//是否验证签名,不验证的画可以篡改数据，不安全
            IssuerSigningKey = signingKey,//解密的密钥
            ValidateIssuer = true,//是否验证发行人，就是验证载荷中的Iss是否对应ValidIssuer参数
            ValidIssuer = jwtConfig.GetValue<string>("Iss"),//发行人
            ValidateAudience = true,//是否验证订阅人，就是验证载荷中的Aud是否对应ValidAudience参数
            ValidAudience = jwtConfig.GetValue<string>("Aud"),//订阅人
            ValidateLifetime = true,//是否验证过期时间，过期了就拒绝访问
            ClockSkew = TimeSpan.FromHours(1),//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
            RequireExpirationTime = true,
        };
    });

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
     {
         c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API"); //注意中间段v1要和上面SwaggerDoc定义的名字保持一致
        });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

 public static class IOCExtend
{
    public  static IServiceCollection AddCustomIOC(this IServiceCollection service)
    {
        service.AddScoped<IBlogNewsRepository, BlogNewsRepository>();
        service.AddScoped<IBlogNewsService, BlogNewsService>();
        service.AddScoped<ITypeInfoRepository, TypeInfoRepository>();
        service.AddScoped<ITypeInfoService, TypeInfoService>();
        service.AddScoped<IWriteInfoRepository, WriteInfoRepository>();
        service.AddScoped<IWriteInfoService, WriteInfoService>();
        return service;
    }

  
}