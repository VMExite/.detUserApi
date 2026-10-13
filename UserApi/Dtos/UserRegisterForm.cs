using System.ComponentModel.DataAnnotations;

namespace UserApi.Dtos;

public sealed record UserRegisterForm(
    [Required, StringLength(50, MinimumLength = 3)]
    string Username,
    [Required, StringLength(50, MinimumLength = 8)]
    string Password,
    [Required, Range(1, 120)]
    int Age
    );