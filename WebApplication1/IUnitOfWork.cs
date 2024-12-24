using Microsoft.Data.SqlClient;

namespace WebApplication1
{
    /// <summary>
    /// Unit of work interface
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Dynamically Get Entity Repository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepository<T> GetRepository<T>() where T : class, IEntity;

        /// <summary>
        /// Auto detect changes set to ON
        /// </summary>
        void AutoDetectChangesOn();

        /// <summary>
        /// Auto detect changes set to OFF
        /// </summary>
        void AutoDetectChangesOff();

        /// <summary>
        /// Begin transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commit transaction
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rollback transaction
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Execute sql command
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Number of rows affected</returns>
        //int ExecuteSqlCommand(string command, params string[] parameters);

        /// <summary>
        /// Execute sql command asynchronous
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Number of rows affected</returns>
        //Task<int> ExecuteSqlCommandAsync(string command, params string[] parameters);

        /// <summary>
        /// Execute stored procedure
        /// </summary>
        /// <typeparam name="T">A class</typeparam>
        /// <param name="name">Name</param>
        /// <param name="sqlParameters">Paramters</param>
        /// <returns>Result</returns>
        //List<T> ExecuteStoredProcedure<T>(string name, params SqlParameter[] sqlParameters) where T : class;

        /// <summary>
        /// Execute stored procedure asynchronous
        /// </summary>
        /// <typeparam name="T">A class</typeparam>
        /// <param name="name">Name</param>
        /// <param name="sqlParameters">Paramters</param>
        /// <returns>Result</returns>
        //Task<List<T>> ExecuteStoredProcedureAsync<T>(string name, params SqlParameter[] sqlParameters) where T : class;

        /// <summary>
        /// SetCommandTimeout
        /// </summary>
        /// <param name="seconds">seconds</param>
        void SetCommandTimeout(int seconds);
    }
}
