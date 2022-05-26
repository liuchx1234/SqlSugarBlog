using MyBlog.Model;

namespace MyBlog.IRepository;

public interface IWriteInfoRepository:IBaseRepository<WriteInfo>
{
    void Method();
}