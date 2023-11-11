using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogHub.Identity.Configuration;
using BlogHub.Identity.Models;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BlogHub.Identity.Controllers;

[Route("[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthorizationController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("connect/token")]
    public IActionResult GenerateToken(TokenRequest tokenRequest)
    {
        Func<TokenRequest, string>? handler = null;

        if (tokenRequest.GrantType.Equals(GrantTypes.ClientCredentials.FirstOrDefault()))
            handler = HandleClientCredentialsFlow;
        
        if (handler is null) return BadRequest(new { Message = $"{tokenRequest.GrantType} flow not implemented"});

        try
        {
            var token = handler(tokenRequest);
            return Ok(new { access_token = token });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { exception.Message });
        }

    }

    private string HandleClientCredentialsFlow(TokenRequest tokenRequest)
    {
        var client = IdentityServerConfiguration.Clients
            .Where(client => client.ClientId.Equals(tokenRequest.ClientId))
            .FirstOrDefault();

        if (client is null) 
            throw new ArgumentException("No client with given id found");
        
        if (client.ClientSecrets.FirstOrDefault()!.Value.Equals(tokenRequest.ClientSecret) is false)
            throw new ArgumentException("Incorrect client secret");

        var claims = client.AllowedScopes.Select(scope => new Claim(JwtClaimTypes.Scope, scope));
  
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtOptions:Issuer"],
            audience: _configuration["JwtOptions:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:SecretKey"]!)), 
                SecurityAlgorithms.HmacSha256)
        );  

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }
}