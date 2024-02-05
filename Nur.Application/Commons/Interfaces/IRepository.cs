using System.Linq.Expressions;

namespace Nur.Application.Commons.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task InsertAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void Delete(Expression<Func<TEntity, bool>> expression);
    Task<TEntity> SelectAsync(Expression<Func<TEntity, bool>> expression, string[] includes = null!);
    IQueryable<TEntity> SelectAll(Expression<Func<TEntity, bool>> expression = null!, string[] includes = null!);
    Task<int> SaveAsync();
}
