using Microsoft.AspNetCore.Identity;
using UserApi.Dtos;
using UserApi.Models;
using UserApi.Repositories;

namespace UserApi.Services;

public class UserService(
    IUserRepository _repository,
    IPasswordHasher<string> _passwordHasher,
    ILogger<UserService> _logger)
{
    public async Task<List<UserResponse>> GetAllUsersBetweenAsync(
        UsersBetweenForm form,
        CancellationToken cancellationToken)
    {
        var users = await _repository.GetAllUsersBetweenAsync(
            form.StartDateTime,
            form.EndDateTime,
            cancellationToken);
        _logger.LogDebug($"Users found: {users.Count}");
        
        return users.Select(UserResponse.ToResponse).ToList();
    }

    public async Task<UserResponse> AddUserAsync(
        UserRegisterForm form,
        CancellationToken cancellationToken)
    {
        var hashed = _passwordHasher.HashPassword(form.Username, form.Password);

        var user = new User
        {
            Username = form.Username,
            Password = hashed,
            Age = form.Age,
            CreateDateTime = DateTime.UtcNow,
            LastUpdateDateTime = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(user, cancellationToken);
        _logger.LogDebug($"User created: {created}");
        
        return UserResponse.ToResponse(created);
    }

    public async Task<UserResponse?> UpdateUserAsync(
        UserUpdateForm form,
        CancellationToken cancellationToken)
    {
        var user = await _repository.GetByUsernameAsync(form.Username, cancellationToken);
        if (user is null)
            return null;

        var result = _passwordHasher.VerifyHashedPassword(
            user.Username, user.Password, form.Credential);

        switch (result)
        {
            case PasswordVerificationResult.Failed:
                throw new UnauthorizedAccessException();

            case PasswordVerificationResult.Success:
            case PasswordVerificationResult.SuccessRehashNeeded:
                if (!string.IsNullOrEmpty(form.NewPassword))
                    user.Password = _passwordHasher.HashPassword(user.Username, form.NewPassword);
                else if (result == PasswordVerificationResult.SuccessRehashNeeded)
                    user.Password = _passwordHasher.HashPassword(user.Username, form.Credential);

                if (form.Age.HasValue)
                    user.Age = form.Age.Value;

                user.LastUpdateDateTime = DateTime.UtcNow;

                var updated = await _repository.UpdateAsync(user, cancellationToken);
                _logger.LogDebug($"User updated: {updated}");
                return UserResponse.ToResponse(updated);

            default:
                _logger.LogDebug($"User {form.Username} not found");
                return null;
        }
    }

    public async Task DeleteUserAsync(
        UserDeleteForm form,
        CancellationToken cancellationToken)
    {
        var user = await _repository.GetByUsernameAsync(form.Username, cancellationToken)
                   ?? throw new KeyNotFoundException("User not found");

        var result = _passwordHasher.VerifyHashedPassword(
            user.Username, user.Password, form.Credential);

        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException();

        await _repository.RemoveAsync(user, cancellationToken);
        _logger.LogDebug($"User deleted: {user}");
    }

    public async Task<UserResponse?> LoginAsync(
        UserLoginForm form,
        CancellationToken cancellationToken)
    {
        var user = await _repository.GetByUsernameAsync(form.Username, cancellationToken);
        if (user is null)
            return null;

        var result = _passwordHasher.VerifyHashedPassword(
            user.Username, user.Password, form.Password);
        _logger.LogDebug($"User {user.Username} password verify status: {result}");

        return result == PasswordVerificationResult.Failed
            ? null
            : UserResponse.ToResponse(user);
    }
}