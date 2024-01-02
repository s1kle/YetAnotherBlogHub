using System.ComponentModel.DataAnnotations;

namespace BlogHub.Identity.Models;

public record LoginViewModel
{
    public required string Username { get; set; }
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    public required string ReturnUrl { get; set; }
}