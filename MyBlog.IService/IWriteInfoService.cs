using MyBlog.Model;

namespace MyBlog.IService;

public interface IWriteInfoService : IBaseService<WriteInfo>
{
    string Test();
}