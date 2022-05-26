using AutoMapper;
using MyBlog.Model;
using MyBlog.Model.Dto;
using MySql.Data.MySqlClient;

namespace 开始学习SqlSugar.Utility.AutoMapper;

public class CustomAutoMapperProfile:Profile
{
    public CustomAutoMapperProfile()
    {
        base.CreateMap<WriteInfo, WriteDto>();
    }
}