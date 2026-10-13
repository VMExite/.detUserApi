using System.ComponentModel.DataAnnotations;

namespace UserApi.Dtos;

public  sealed record UsersBetweenForm(
    [Required]
    DateTime StartDateTime,
    [Required]
    DateTime EndDateTime
    ) {}