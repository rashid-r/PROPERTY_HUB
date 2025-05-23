using System.Security.Claims;

namespace BOOLOG.API.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly ILogger<AuthenticationMiddleware> _logger;
        private readonly RequestDelegate _next;
        public AuthenticationMiddleware(ILogger<AuthenticationMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;

            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var idclaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

                if (idclaim != null)
                {
                    _logger.LogInformation($"NameIdentifier claim found: {idclaim.Value}");
                        context.Items["UserId"] = idclaim;
                }
                else
                {
                    _logger.LogWarning("NameIdentifier claim not found in the token.");
                }
            }
            else
            {
                _logger.LogInformation("User is not authenticated.");
            }
            await _next(context);
        }

    }
}

