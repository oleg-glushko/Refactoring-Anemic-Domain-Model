using System.ComponentModel.DataAnnotations;

namespace Logic.Dtos;

public class CreateCustomerDto
{
    [MaxLength(100, ErrorMessage = "Name is too long")]
    public required string Name { get; set; }

    [EmailAddress]
    public required string Email { get; set; }
}
