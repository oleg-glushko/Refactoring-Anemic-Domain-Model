using System.ComponentModel.DataAnnotations;

namespace Logic.Dtos;

public class UpdateCustomerDto
{
    [MaxLength(100, ErrorMessage = "Name is too long")]
    public required string Name { get; set; }
}