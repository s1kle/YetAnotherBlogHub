using System.ComponentModel.DataAnnotations;

namespace BlogHub.Identity.Models;

public record RegisterViewModel
{
    public required string Username { get; set; }
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    [DataType(DataType.Password)][Compare(nameof(Password))]
    public required string PasswordConfirmation { get; set; }
    public required string ReturnUrl { get; set; }
}