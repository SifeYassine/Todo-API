using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using TodoApi.Services;

namespace TodoApi.Middlewares;

public class TokenAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IAuthService authService) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
  private readonly IAuthService _authService = authService;

  protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
  {
    if (!Request.Headers.ContainsKey("Authorization")) {
      return AuthenticateResult.Fail("Missing Authorization header");
    }

    var header = Request.Headers.Authorization.ToString();
    if (!header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) {
      return AuthenticateResult.Fail("Invalid Authorization header");
    }

    var plainToken = header["Bearer ".Length..].Trim();
    var accessToken = await _authService.ValidateTokenAsync(plainToken);
    
    if (accessToken == null) {
      return AuthenticateResult.Fail("Invalid or expired token");
    }

    var user = accessToken.User;

    var claims = new[]
    {
      new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new Claim(ClaimTypes.Email, user.Email),
      new Claim(ClaimTypes.Name, user.Username)
    };

    var identity = new ClaimsIdentity(claims, Scheme.Name);
    var principal = new ClaimsPrincipal(identity);
    var ticket = new AuthenticationTicket(principal, Scheme.Name);

    return AuthenticateResult.Success(ticket);
  }
}