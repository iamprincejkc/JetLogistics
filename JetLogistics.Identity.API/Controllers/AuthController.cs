using JetLogistics.Identity.API.DataAccess;
using JetLogistics.Identity.API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Microsoft.AspNetCore.Authorization;

namespace JetLogistics.Identity.API.Controllers
{
    [ApiController]
    [Route("connect")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IdentityDbContext _db;

        public AuthorizationController(IdentityDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        [HttpGet("/health")]
        public async Task<ActionResult> Health(int id)
        {
            return Ok("Identity API");
        }

        [HttpPost("token"), IgnoreAntiforgeryToken]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            if (request is null)
            {
                return BadRequest(new
                {
                    error = "invalid_request",
                    error_description = "Invalid OpenID Connect request."
                });
            }

            if (request.IsPasswordGrantType())
                return await HandlePasswordGrantAsync(request);

            if (request.IsRefreshTokenGrantType())
                return await HandleRefreshTokenGrantAsync(request);

            return BadRequest(new
            {
                error = "unsupported_grant_type",
                error_description = "Only password and refresh_token grant types are supported."
            });
        }

        private async Task<IActionResult> HandlePasswordGrantAsync(OpenIddictRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user is null || !PasswordHasherService.VerifyPassword(user.PasswordHash, request.Password))
            {
                return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            identity.AddClaim(Claims.Subject, user.Id.ToString());
            identity.AddClaim(Claims.Name, user.Username);
            identity.AddClaim(Claims.Email, user.Email ?? "");

            var clientId = request.ClientId;
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return BadRequest(new
                {
                    error = "invalid_request",
                    error_description = "Missing client_id in the token request."
                });
            }

            var audiences = clientId switch
            {
                "clientid" => new[] { "consignee_audience", "gateway" },
                "clientid2" => new[] { "booking_audience", "gateway" },
                "clientid0" => new[] { "consignee_audience", "booking_audience", "gateway" },
                _ => new[] { "gateway" }
            };

            var principal = new ClaimsPrincipal(identity);

            // ✅ Set proper audiences, scopes, and resources
            principal.SetAudiences(audiences);
            principal.SetResources(audiences);

            var scopes = clientId switch
            {
                "clientid" => new[] { Scopes.OfflineAccess,"consignee_api" },
                "clientid2" => new[] { Scopes.OfflineAccess, "booking_api" },
                "clientid0" => new[] { Scopes.OfflineAccess, "booking_api", "consignee_api" },
                _ => new[] { "gateway" }
            };

            principal.SetScopes(scopes);

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }




        private async Task<IActionResult> HandleRefreshTokenGrantAsync(OpenIddictRequest request)
        {

            var clientId = request.ClientId;
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return BadRequest(new
                {
                    error = "invalid_request",
                    error_description = "Missing client_id in the token request."
                });
            }

            var audiences = clientId switch
            {
                "clientid" => new[] { "consignee_audience", "gateway" },
                "clientid2" => new[] { "booking_audience", "gateway" },
                "clientid0" => new[] { "consignee_audience", "booking_audience", "gateway" },
                _ => new[] { "gateway" }
            };


            var principal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;
            if (principal == null)
                return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);


            var identity = new ClaimsIdentity(principal.Claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            var newPrincipal = new ClaimsPrincipal(identity);


            newPrincipal.SetAudiences(audiences);
            newPrincipal.SetResources(audiences);

            newPrincipal.SetScopes(principal.GetScopes());


            return SignIn(newPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }
}
