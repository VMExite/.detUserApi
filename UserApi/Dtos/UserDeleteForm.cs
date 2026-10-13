using System.ComponentModel.DataAnnotations;

namespace UserApi.Dtos;

public sealed record UserDeleteForm(
    [Required, StringLength(50, MinimumLength = 3)]
    string Username,
    [Required, StringLength(50, MinimumLength = 8)]
    string Credential
);