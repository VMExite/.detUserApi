using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace UserApi.Models;

[DisplayName("users")]
[Index(nameof(Username), IsUnique = true)]
public class User
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public required string Username { get; set; }

    [Required, MaxLength(100)]
    public required string Password { get; set; }
    public int Age { get; set; }
    
    [Required]
    public DateTime CreateDateTime { get; set; }
    public DateTime LastUpdateDateTime { get; set; }
}