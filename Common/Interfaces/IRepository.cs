using System.Linq.Expressions;
using Common.Entities;

namespace Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task <IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> RemoveAsync(int id);
}