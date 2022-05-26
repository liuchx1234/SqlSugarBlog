using System.Diagnostics.CodeAnalysis;
using MyBlog.IService;
using SqlSugar;
using System.Linq.Expressions;
using MyBlog.IRepository;

namespace MyBlog.Service;

public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
{
    protected  IBaseRepository<TEntity> __repository;
    
    public async Task<bool> CreateAsync(TEntity entity)
    {
       return await __repository.CreateAsync(entity);
    }

    public async  Task<bool> DeleteAsync(int id)
    {
        return await __repository.DeleteAsync(id);
    }

    public virtual async Task<TEntity> FindAsync(int id)
    {
        return await __repository.FindAsync(id);
    }

    public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> func)
    {
        return await __repository.FindAsync(func);
    }

    public virtual async Task<List<TEntity>> QueryAsync()
    {
        return await __repository.QueryAsync();
    }

    public virtual async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func)
    {
        return await __repository.QueryAsync(func);
    }

    public virtual async Task<List<TEntity>> QueryAsync(int page, int size, RefAsync<int> total)
    {
        return await __repository.QueryAsync(page,size,total);
    }

    public virtual async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func, int page, int size, RefAsync<int> total)
    {
        return await __repository.QueryAsync(func,page,size,total);
    }

    public  async Task<bool> EditAsync(TEntity entity)
    {
        return await __repository.EditAsync(entity);
    }
}