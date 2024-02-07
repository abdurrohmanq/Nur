using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Helpers;
using Nur.Application.Commons.Interfaces;
using Nur.Domain.Commons;
using Nur.Infrastructure.Contexts;
using System.Linq.Expressions;

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

    public IQueryable<T> SelectAll(Expression<Func<T, bool>> expression = null!, bool isNoTracked = true, string[] includes = null!)
    {
        IQueryable<T> query = expression is null ? Table.AsQueryable() : Table.Where(expression).AsQueryable();
        query = isNoTracked ? query.AsNoTracking() : query;

        if (includes is not null)
            foreach (var item in includes)
                query = query.Include(item);

        return query;
    }

    public Task<int> SaveAsync()
        => appDbContext.SaveChangesAsync();
}
