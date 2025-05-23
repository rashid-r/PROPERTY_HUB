namespace BOOLOG.Application.Repository.RepositoryInterfaces
{
    public interface IRepository<T>
    {
        Task AddAsync(T Entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
