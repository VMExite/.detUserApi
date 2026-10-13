using System.ComponentModel.DataAnnotations;

namespace UserApi.Dtos;

public sealed record UserLoginForm(
    [Required, StringLength(50, MinimumLength = 3)]
    string Username,
    [Required, StringLength(50, MinimumLength = 8)]
    string Password
);