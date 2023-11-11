using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace Auth.API.Repository
{
    /// <summary>
    /// Provides a contract for CRUD operations on entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Asynchronously retrieves all records from the database.
        /// </summary>
        /// <returns>A list of records.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Asynchronously retrieves a record by its primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        /// <returns>The found record or null.</returns>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// Asynchronously retrieves records based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each record for a condition.</param>
        /// <returns>A list of records satisfying the condition.</returns>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Asynchronously retrieves a single record based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test the record for a condition.</param>
        /// <returns>The first record satisfying the condition, or null if no records match.</returns>
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Asynchronously inserts a record into the database.
        /// </summary>
        /// <param name="entity">The record to insert.</param>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Asynchronously inserts multiple records into the database.
        /// </summary>
        /// <param name="entities">The records to insert.</param>
        Task InsertRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Asynchronously updates a record in the database.
        /// </summary>
        /// <param name="entity">The record to update.</param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Asynchronously deletes a record by its primary key.
        /// </summary>
        /// <param name="id">The primary key value of the record to delete.</param>
        Task DeleteAsync(object id);

        /// <summary>
        /// Asynchronously deletes multiple records from the database.
        /// </summary>
        /// <param name="entities">The records to delete.</param>
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    }

}

