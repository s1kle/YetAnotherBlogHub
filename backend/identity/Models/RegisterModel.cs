namespace BlogHub.Identity.Models;

public record RegisterModel
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string ErrorUri { get; init; }
    public required string RedirectUri { get; init; }
}