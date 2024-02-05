using Nur.Domain.Commons;
using System.Linq.Expressions;
using Nur.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Helpers;
using Nur.Application.Commons.Interfaces;

namespace Nur.Infrastructure.Repositories;

public class Repository<T>(AppDbContext appDbContext) : IRepository<T> where T : Auditable
{
    public DbSet<T> Table
    {
        get => appDbContext.Set<T>();
    }

    public async Task InsertAsync(T entity)
    {
        entity.CreatedAt = TimeHelper.GetDateTime();
        await Table.AddAsync(entity);
    }

    public void Update(T entity)
    {
        entity.LastUpdatedAt = TimeHelper.GetDateTime();
        Table.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        Table.Remove(entity);
    }

    public void Delete(Expression<Func<T, bool>> expression)
    {
        Table.RemoveRange(Table.Where(expression));
    }

    public async Task<T> SelectAsync(Expression<Func<T, bool>> expression, string[] includes = null!)
    {
        if (includes is null)
            return (await Table.FirstOrDefaultAsync(expression))!;

        var query = Table.AsQueryable();
        foreach (string item in includes)
            query = query.Include(item);

        return (await query.FirstOrDefaultAsync(expression))!;
    }

    public IQueryable<T> SelectAll(Expression<Func<T, bool>> expression = null!, string[] includes = null!)
    {
        if (includes is null)
            return expression is null ? Table : Table.Where(expression);

        var query = Table.AsQueryable();
        foreach (string item in includes)
            query = query.Include(item);

        return expression is null ? Table : Table.Where(expression);
    }

    public Task<int> SaveAsync()
        => appDbContext.SaveChangesAsync();
}
