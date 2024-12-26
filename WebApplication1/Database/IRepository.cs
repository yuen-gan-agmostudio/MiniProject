namespace WebApplication1
{
    /// <summary>
    /// Provides repository pattern for the entity
    /// </summary>
    /// <typeparam name="TEntity">Entity</typeparam>
    public interface IRepository<TEntity> : IRepository
    {
        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Insert entity (Asynchronous)
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Task</returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Insert entities (Asynchronous)
        /// </summary>
        /// <param name="entities">Entities</param>
        /// <returns>Task</returns>
        Task InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Insert entities with auto detect changes off
        /// </summary>
        /// <param name="entities">Entities</param>
        /// <returns>Task</returns>
        void InsertBatchWithAutoDetectChangessOff(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update no tracking entity
        /// </summary>
        /// <param name="entity">entity</param>
        void UpdateNoTracking(TEntity entity);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Update entity (Asynchronous)
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>Task</returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">entities</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update entities (Asynchronous)
        /// </summary>
        /// <param name="entities">entities</param>
        /// <returns>Task</returns>
        Task UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete entity (Asynchronous)
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Task</returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete entities (Asynchronous)
        /// </summary>
        /// <param name="entities">Entities</param>
        /// <returns>Task</returns>
        Task DeleteAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Get a table
        /// </summary>
        IQueryable<TEntity> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<TEntity> TableNoTracking { get; }
    }

    public interface IRepository { }
}
