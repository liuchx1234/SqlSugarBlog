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
using ��ʼѧϰSqlSugar.Utility.AutoMapper;

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
               Description = "���¿�����������ͷ����Ҫ���Jwt��ȨToken��Bearer Token",
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


#region ע��SqlSugar.IOC

builder.Services.AddSqlSugar(new IocConfig()
{
    //ConfigId="db01"  ���⻧�õ�
    ConnectionString = builder.Configuration.GetConnectionString("MySQLConnection"),
    DbType = IocDbType.MySql,
    IsAutoCloseConnection = true//�Զ��ͷ�
});

#endregion

#region IOC����ע��

builder.Services.AddCustomIOC();
#endregion


#region AutoMapper

builder.Services.AddAutoMapper(typeof(CustomAutoMapperProfile));

#endregion
#region ���JWT

var jwtConfig = builder.Configuration.GetSection("Jwt");
//������Կ
var symmetricKeyAsBase64 = jwtConfig.GetValue<string>("Secret");
var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
var signingKey = new SymmetricSecurityKey(keyByteArray);
//��֤����
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,//�Ƿ���֤ǩ��,����֤�Ļ����Դ۸����ݣ�����ȫ
            IssuerSigningKey = signingKey,//���ܵ���Կ
            ValidateIssuer = true,//�Ƿ���֤�����ˣ�������֤�غ��е�Iss�Ƿ��ӦValidIssuer����
            ValidIssuer = jwtConfig.GetValue<string>("Iss"),//������
            ValidateAudience = true,//�Ƿ���֤�����ˣ�������֤�غ��е�Aud�Ƿ��ӦValidAudience����
            ValidAudience = jwtConfig.GetValue<string>("Aud"),//������
            ValidateLifetime = true,//�Ƿ���֤����ʱ�䣬�����˾;ܾ�����
            ClockSkew = TimeSpan.FromHours(1),//����ǻ������ʱ�䣬Ҳ����˵����ʹ���������˹���ʱ�䣬����ҲҪ���ǽ�ȥ������ʱ��+���壬Ĭ�Ϻ�����7���ӣ������ֱ������Ϊ0
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
         c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API"); //ע���м��v1Ҫ������SwaggerDoc��������ֱ���һ��
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