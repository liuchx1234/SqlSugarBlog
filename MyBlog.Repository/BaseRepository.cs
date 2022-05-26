using System.Linq.Expressions;
using MyBlog.IRepository;
using MyBlog.Model;
using SqlSugar;
using SqlSugar.IOC;

namespace MyBlog.Repository;

public class BaseRepository<TEntity>: SimpleClient<TEntity>,IBaseRepository<TEntity> where TEntity:class,new()
{
    public BaseRepository(ISqlSugarClient context = null) : base(context)
    {
        // base.Context=context;
        base.Context = DbScoped.SugarScope;
        base.Context.DbMaintenance.CreateDatabase();
        base.Context.CodeFirst.InitTables(
            typeof(BlogNews),
            typeof(TypeInfo),
            typeof(WriteInfo));
    }
    public async Task<bool> CreateAsync(TEntity entity)
    {
        return await base.InsertAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await base.DeleteByIdAsync(id);
    }

    public async Task<bool> EditAsync(TEntity entity)
    {
        return await base.UpdateAsync(entity);
    }

    public virtual async Task<TEntity> FindAsync(int id)
    {
        return await base.GetByIdAsync(id);
    }

    public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> func)
    {
        return await base.GetSingleAsync(func);
    }

    public virtual async Task<List<TEntity>> QueryAsync()
    {
        return await base.GetListAsync();
    }

    public virtual async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func)
    {
        return await base.GetListAsync(func);
    }

    public virtual async Task<List<TEntity>> QueryAsync(int page, int size, RefAsync<int> total)
    {
        return  await base.Context.Queryable<TEntity>().ToPageListAsync(page, size, total);
    }

    public virtual async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func, int page, int size, RefAsync<int> total)
    {
        return await base.Context.Queryable<TEntity>().Where(func).ToPageListAsync(page, size, total);
    }
}