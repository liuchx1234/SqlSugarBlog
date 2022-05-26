using MyBlog.IRepository;
using MyBlog.Model;
using SqlSugar;
using System.Linq.Expressions;

namespace MyBlog.Repository;

public class BlogNewsRepository:BaseRepository<BlogNews>,IBlogNewsRepository
{
  
}