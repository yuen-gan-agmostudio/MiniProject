using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    public class SqlRepository<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class, IEntity
    {
        public TDbContext Context { get; }
        public DbSet<TEntity> DbSet { get; }
        public IQueryable<TEntity> Table => DbSet;
        public IQueryable<TEntity> TableNoTracking => DbSet.AsNoTracking();

        public SqlRepository(TDbContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
            Context.SaveChanges();
        }

        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
            Context.SaveChanges();
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
            await Context.SaveChangesAsync();
        }

        public virtual void InsertBatchWithAutoDetectChangessOff(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
            try
            {
                Context.ChangeTracker.AutoDetectChangesEnabled = false;
                Context.SaveChanges();
            }
            catch
            {
                Context.ChangeTracker.AutoDetectChangesEnabled = true;
                throw;
            }
            Context.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        public virtual void UpdateNoTracking(TEntity entity)
        {
            Context.Attach(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            Context.SaveChanges();
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            Context.SaveChanges();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await Context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            await Context.SaveChangesAsync();
        }

        public virtual void Delete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
            Context.SaveChanges();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
            await Context.SaveChangesAsync();
        }
    }
}
