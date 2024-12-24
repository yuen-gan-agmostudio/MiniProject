namespace WebApplication1
{
    /// <summary>
    /// Interface for base entity. All entities in the system must implement this interface
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key</typeparam>
    public interface IEntity<TPrimaryKey> : IEntity
    {
        /// <summary>
        /// Gets or sets unique identifier
        /// </summary>
        TPrimaryKey Id { get; set; }
    }

    public interface IEntity
    {
    }
}
