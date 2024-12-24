using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Text;

namespace WebApplication1
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public UnitOfWork(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            _context = new ApplicationDbContext(optionsBuilder.Options);
        }

        public virtual IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            var repository = new SqlRepository<DbContext, TEntity>(_context);
            return (IRepository<TEntity>)repository;
        }

        public virtual void AutoDetectChangesOn()
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        public virtual void AutoDetectChangesOff()
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public virtual void BeginTransaction()
        {
            if (_context.Database.CurrentTransaction == null)
                _context.Database.BeginTransaction();
        }

        public virtual void CommitTransaction()
        {
            _context.Database.CurrentTransaction?.Commit();
        }

        public virtual void RollbackTransaction()
        {
            _context.Database.CurrentTransaction?.Rollback();
        }

        protected virtual string ParameterizeStoredProcedureQuery(string name, SqlParameter[] sqlParameters)
        {
            var builder = new StringBuilder();
            builder.Append(name);
            foreach (var param in sqlParameters)
            {
                var paramName = $"param_{param.ParameterName}";
                builder.Append($" @{param.ParameterName} = @{paramName},");
                param.ParameterName = paramName;
            }

            return builder.ToString().TrimEnd(',');
        }

        public virtual void SetCommandTimeout(int seconds)
        {
            _context.Database.SetCommandTimeout(seconds);
        }
    }
}
