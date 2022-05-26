using MyBlog.IRepository;
using MyBlog.IService;
using MyBlog.Model;

namespace MyBlog.Service;

public class BlogNewsService:BaseService<BlogNews>,IBlogNewsService
{
    private readonly IBlogNewsRepository _blogNewsRepository;
    public BlogNewsService(IBlogNewsRepository iBlogNewsRepository)
    {
        base.__repository=iBlogNewsRepository;
        _blogNewsRepository=iBlogNewsRepository;
    }
}