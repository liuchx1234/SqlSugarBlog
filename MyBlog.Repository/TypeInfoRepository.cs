using System.Linq.Expressions;
using MyBlog.IRepository;
using MyBlog.Model;
using SqlSugar;

namespace MyBlog.Repository;

public class TypeInfoRepository:BaseRepository<TypeInfo>,ITypeInfoRepository
{
    
}