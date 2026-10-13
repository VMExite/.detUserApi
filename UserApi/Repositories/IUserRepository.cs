using UserApi.Models;

namespace UserApi.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersBetweenAsync(DateTime startDate , DateTime endDate, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<User> AddAsync(User user, CancellationToken cancellationToken);
        Task<User> UpdateAsync(User user, CancellationToken cancellationToken);
        Task<User> RemoveAsync(User user, CancellationToken cancellationToken);
    }
}
