//public class ExceptionHandlingMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

//    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
//    {
//        _next = next;
//        _logger = logger;
//    }

//    public async Task Invoke(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "An unexpected error occurred.");
//            context.Response.StatusCode = 500;
//            context.Response.ContentType = "application/json";
//            await context.Response.WriteAsync(new
//            {
//                StatusCode = 500,
//                Message = "An unexpected error occurred. Please try again later."
//            }.ToString());
//        }
//    }
//}
