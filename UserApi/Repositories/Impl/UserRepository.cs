using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Models;

namespace UserApi.Repositories.Impl;

public class UserRepository(DatabaseContext _context) : IUserRepository
{
    public async Task<List<User>> GetAllUsersBetweenAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        return await _context.Set<User>()
            .Where(u => u.CreateDateTime >= startDate && u.CreateDateTime <= endDate)
            .OrderBy(u => u.CreateDateTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        _context.Set<User>().Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _context.Set<User>().Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User> RemoveAsync(User user, CancellationToken cancellationToken)
    {
        _context.Set<User>().Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }
}