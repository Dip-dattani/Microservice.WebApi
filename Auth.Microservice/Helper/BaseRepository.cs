using Auth.Microservice.Data.DbContext;
using Auth.Microservice.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Microservice.Helper
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : AuditableEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id, bool includeDeleted = false)
        {
            var query = _dbSet.AsQueryable();
            if (!includeDeleted)
                query = query.Where(e => !e.IsDeleted);
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool includeDeleted = false)
        {
            var query = _dbSet.AsQueryable();
            if (!includeDeleted)
                query = query.Where(e => !e.IsDeleted);
            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false)
        {
            var query = _dbSet.AsQueryable();
            if (!includeDeleted)
                query = query.Where(e => !e.IsDeleted);
            return await query.Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            entity.LastModificationDate = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id, bool softDelete = true)
        {
            var entity = await GetByIdAsync(id, true);
            if (entity == null)
                throw new KeyNotFoundException($"Entity with ID {id} not found");

            if (softDelete)
            {
                entity.IsDeleted = true;
                entity.DeletionTime = DateTime.UtcNow;
                await UpdateAsync(entity);
            }
            else
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id && !e.IsDeleted);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var query = _dbSet.Where(e => !e.IsDeleted);
            if (predicate != null)
                query = query.Where(predicate);
            return await query.CountAsync();
        }
    }
}
