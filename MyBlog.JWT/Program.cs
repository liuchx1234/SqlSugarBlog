using System.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyBlog.IRepository;
using MyBlog.IService;
using MyBlog.Repository;
using MyBlog.Service;
using SqlSugar.IOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlSugar(new IocConfig()
{
    //ConfigId="db01"  ���⻧�õ�
    ConnectionString = builder.Configuration.GetConnectionString("MySQLConnection"),
    DbType = IocDbType.MySql,
    IsAutoCloseConnection = true//�Զ��ͷ�
});
builder.Services.AddScoped<IWriteInfoRepository, WriteInfoRepository>();
builder.Services.AddScoped<IWriteInfoService, WriteInfoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
