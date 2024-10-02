using BookReservationSystem.Domain.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookReservationSystem.Infrastructure.Repository.Generic
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly BookDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(BookDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        // Retrieve entity by its primary key
        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Retrieve all entities of this type
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Retrieve entities based on a condition (predicate)
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        // Add a new entity to the database
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // Update an existing entity
        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        // Remove an entity from the database
        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        // Commit changes to the database
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
