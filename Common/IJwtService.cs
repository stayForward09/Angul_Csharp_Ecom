using StackApi.Models;

namespace StackApi.Common;

public interface IJwtService
{
    object GenerateJwt(User data);
    string HashPassword(string password);
}