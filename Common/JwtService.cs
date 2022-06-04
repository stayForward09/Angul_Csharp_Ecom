using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StackApi.Models;

namespace StackApi.Common;

public class JwtService : IJwtService
{
    private readonly IConfiguration configuration;
    public JwtService(IConfiguration _config)
    {
        configuration = _config;
    }
    public object GenerateJwt(User data)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfig:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, data.UsID.ToString()),
                        new Claim("UserMail", data.EmailID),
                        new Claim("UserID", data.UsID.ToString()),
                        new Claim("Admin",data.IsAdmin.ToString().ToLower()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
        var token = new JwtSecurityToken(configuration["JwtConfig:Issuer"],
                configuration["JwtConfig:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(60),
                signingCredentials: credentials);
        return new { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpireAt = token.ValidTo };
    }

    public string HashPassword(string password)
    {
        StringBuilder strHashPass = new StringBuilder();
        using SHA256 sh = SHA256.Create();
        byte[] bdata = sh.ComputeHash(Encoding.UTF8.GetBytes(password));
        foreach (byte b in bdata)
        {
            strHashPass.Append(b.ToString("X2"));
        }
        return strHashPass.ToString();
    }
}