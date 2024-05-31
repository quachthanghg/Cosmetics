using Ocelot.Configuration.Repository;
using Ocelot.Middleware;
using System.Security.Claims;
using Ocelot.Configuration;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using Web.Bff.Cosmetics.Dtos;

namespace Web.Bff.Cosmetics.Middlewares
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IInternalConfigurationRepository _internalConfigurationRepository;
        private readonly IConfiguration _configuration;

        public PermissionMiddleware(RequestDelegate next, IInternalConfigurationRepository internalConfigurationRepository, IConfiguration configuration)
        {
            _next = next;
            _internalConfigurationRepository = internalConfigurationRepository;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
            //var accessToken = GetCurrentAsync(context);
            //if (string.IsNullOrEmpty(accessToken))
            //{
            //    if (ApisIgnoreToken().Contains(context.Request.Path))
            //    {
            //        await _next(context);
            //    }
            //    else
            //    {
            //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //        await context.Response.WriteAsync("Forbidden: You don't have permission to access this resource.");
            //        return;
            //    }
            //}
            //else
            //{
            //    // Get userContext from token
            //    var userContext = GetUserContext(accessToken);
            //    if (userContext != null)
            //    {
            //        // Extract the module and action from the request context
            //        // Access the Ocelot configuration
            //        var config = _internalConfigurationRepository.Get();
            //        string module = string.Empty;
            //        string permission = string.Empty;

            //        _ = config.Data.Routes.FirstOrDefault(r =>
            //        {
            //            // Check if the current route matches the requested path
            //            var matchedRoute = r.DownstreamRoute.FirstOrDefault(x => x.UpstreamPathTemplate.OriginalValue == context.Request.Path);
            //            if (matchedRoute != null)
            //            {
            //                // Check if the route has claims requirements
            //                var claimsRequirement = matchedRoute.RouteClaimsRequirement;
            //                if (claimsRequirement != null)
            //                {
            //                    // Extract the module claim requirement
            //                    module = GetConfigByKey(claimsRequirement, "Module");
            //                    permission = GetConfigByKey(claimsRequirement, "Permission");
            //                }
            //                return true; // Found a matching route
            //            }
            //            return false; // No matching route found
            //        });

            //        // Check whether the user has the required permission
            //        if (!HasPermission(userContext, module, permission))
            //        {
            //            // If permission is denied, return a forbidden response
            //            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //            await context.Response.WriteAsync("Forbidden: You don't have permission to access this resource.");
            //            return;
            //        }

            //        // If permission is granted, continue processing the request
            //        await _next(context);
            //    }
            //    else
            //    {
            //        // If permission is denied, return a forbidden response
            //        context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //        await context.Response.WriteAsync("Forbidden: You don't have permission to access this resource.");
            //        return;
            //    }
            //}
        }

        private string GetConfigByKey(Dictionary<string, string> claimsRequirement, string key)
        {
            return claimsRequirement.GetValueOrDefault(key);
        }

        private bool HasPermission(UserContext user, string module, string pemission)
        {
            return true;
        }

        private string GetCurrentAsync(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }

        private UserContext GetUserContext(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            var userLogin = tokenS.Claims.FirstOrDefault(x => x.Type == "userName")?.Value;

            return new UserContext()
            {
                UserName = userLogin
            };
        }

        private List<string> ApisIgnoreToken()
        {
            return new List<string>()
            {
                "/api/report/GeneralReport/export"
            };
        }
    }
}
