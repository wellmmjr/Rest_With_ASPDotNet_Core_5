using System.Collections.Generic;
using System.Security.Claims;

namespace RestWithASPNet.Services
{
    public interface ITokenService
    {
        string GenarateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
