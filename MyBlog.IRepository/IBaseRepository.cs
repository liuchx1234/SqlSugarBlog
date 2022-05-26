using System.Linq.Expressions;
using SqlSugar;

namespace MyBlog.IRepository;

public interface IBaseRepository<TEntity> where TEntity:class,new()
{
    Task<bool> CreateAsync(TEntity entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> EditAsync(TEntity entity);
    Task<TEntity> FindAsync(int id);
    Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> func);
    /// <summary>
    /// 查询全部数据
    /// </summary>
    /// <returns></returns>
    Task<List<TEntity>> QueryAsync();
    /// <summary>
    /// 自定义查询条件
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    Task<List<TEntity>> QueryAsync(Expression<Func<TEntity,bool>> func);
    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="total"></param>
    /// <returns></returns>
    Task<List<TEntity>> QueryAsync(int page,int size,RefAsync<int> total);
    /// <summary>
    /// 自定义分页查询
    /// </summary>
    /// <param name="func"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="total"></param>
    /// <returns></returns>
    Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func,int page, int size, RefAsync<int> total);
}