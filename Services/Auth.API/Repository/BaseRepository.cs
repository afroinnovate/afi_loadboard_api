using Auth.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.API.Repository
{
    /// <summary>
    /// Generic repository class that provides CRUD operations on entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Database context instance used for data operations.
        /// </summary>
        protected readonly ApplicationDbContext _dbset;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbset = dbContext;
        }

        /// <summary>
        /// Asynchronously retrieves all records from the database.
        /// </summary>
        /// <returns>A list of records.</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbset.Set<TEntity>().ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a record by its primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        /// <returns>The found record or null.</returns>
        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbset.Set<TEntity>().FindAsync(id);
        }

        /// <summary>
        /// Asynchronously retrieves records based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each record for a condition.</param>
        /// <returns>A list of records satisfying the condition.</returns>
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.Set<TEntity>().Where(predicate).ToListAsync();
        }

         /// <summary>
        /// Asynchronously retrieves a single record based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test the record for a condition.</param>
        /// <returns>The first record satisfying the condition, or null if no records match.</returns>
        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Asynchronously inserts a record into the database.
        /// </summary>
        /// <param name="entity">The record to insert.</param>
        public async Task InsertAsync(TEntity entity)
        {
            await _dbset.Set<TEntity>().AddAsync(entity);
            await _dbset.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously inserts multiple records into the database.
        /// </summary>
        /// <param name="entities">The records to insert.</param>
        public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbset.Set<TEntity>().AddRangeAsync(entities);
            await _dbset.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates a record in the database.
        /// </summary>
        /// <param name="entity">The record to update.</param>
        public async Task UpdateAsync(TEntity entity)
        {
            _dbset.Set<TEntity>().Update(entity);
            await _dbset.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a record by its primary key.
        /// </summary>
        /// <param name="id">The primary key value of the record to delete.</param>
        public async Task DeleteAsync(object id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbset.Set<TEntity>().Remove(entity);
                await _dbset.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Asynchronously deletes multiple records from the database.
        /// </summary>
        /// <param name="entities">The records to delete.</param>
        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbset.Set<TEntity>().RemoveRange(entities);
            await _dbset.SaveChangesAsync();
        }
    }
}