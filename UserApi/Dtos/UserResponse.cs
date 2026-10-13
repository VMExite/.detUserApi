using UserApi.Models;

namespace UserApi.Dtos;

public sealed record UserResponse(
    string Username,
    DateTime CreateDateTime,
    DateTime LastUpdateDateTime,
    int Age
)
{
    public static UserResponse ToResponse(User user) 
    {
        return new UserResponse(user.Username, user.CreateDateTime, user.LastUpdateDateTime, user.Age);
    }
}