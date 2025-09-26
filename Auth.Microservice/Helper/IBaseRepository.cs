using Auth.Microservice.Models.Base;
using System.Linq.Expressions;

namespace Auth.Microservice.Helper
{
    public interface IBaseRepository<TEntity> where TEntity : AuditableEntity
    {
        Task<TEntity?> GetByIdAsync(Guid id, bool includeDeleted = false);
        Task<IEnumerable<TEntity>> GetAllAsync(bool includeDeleted = false);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id, bool softDelete = true);
        Task<bool> ExistsAsync(Guid id);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
    }
}
