using MyBlog.IRepository;
using MyBlog.IService;
using MyBlog.Model;

namespace MyBlog.Service;

public class TypeInfoService : BaseService<TypeInfo>,ITypeInfoService
{
    private readonly ITypeInfoRepository _repository;
    public TypeInfoService(ITypeInfoRepository infoRepository)
    {
        base.__repository = infoRepository;
        _repository = infoRepository;
    }
}