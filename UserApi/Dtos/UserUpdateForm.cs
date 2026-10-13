using System.ComponentModel.DataAnnotations;

namespace UserApi.Dtos;

public sealed record UserUpdateForm(
    [Required, StringLength(50, MinimumLength = 3)]
    string Username,
    [StringLength(50, MinimumLength = 8)]
    string? NewPassword,
    [Range(1, 120)]
    int? Age,
    [Required, StringLength(50, MinimumLength = 8)]
    string Credential
);