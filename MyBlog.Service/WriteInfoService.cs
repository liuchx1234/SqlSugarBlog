using MyBlog.IRepository;
using MyBlog.IService;
using MyBlog.Model;

namespace MyBlog.Service;

public class WriteInfoService : BaseService<WriteInfo>,IWriteInfoService
{
    private readonly IWriteInfoRepository _repository;
    public WriteInfoService(IWriteInfoRepository writeInfo)
    {
        base.__repository = writeInfo;
        _repository = writeInfo;
    }

    public string Test()
    {
        return "test";
    }
}