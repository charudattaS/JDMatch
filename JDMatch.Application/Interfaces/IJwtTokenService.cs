using JDMatch.Domain.Entities;

namespace JDMatch.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
