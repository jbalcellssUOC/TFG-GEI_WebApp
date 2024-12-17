using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

/// <summary>
/// Exception handling middleware to catch unhandled exceptions and log them.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// ExceptionHandlingMiddleware constructor.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invoke method to catch unhandled exceptions and log them.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// HandleExceptionAsync method to handle exceptions.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError; // 500 if unexpected
        var result = JsonConvert.SerializeObject(new { error = exception.Message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(result);
    }
}
