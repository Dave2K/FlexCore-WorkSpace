using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using FlexCore.Core.Exceptions;

namespace FlexCore.Web.Middleware;

/// <summary>
/// Middleware per la gestione centralizzata delle eccezioni nell'intera applicazione.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="GlobalExceptionMiddleware"/>.
    /// </summary>
    /// <param name="next">Delegate per la richiesta successiva nella pipeline.</param>
    /// <param name="logger">Logger per la registrazione degli errori.</param>
    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Gestisce la richiesta HTTP e intercetta tutte le eccezioni non gestite.
    /// </summary>
    /// <param name="context">Contesto HTTP della richiesta corrente.</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException appEx)
        {
            await HandleAppExceptionAsync(context, appEx);
        }
        catch (Exception ex)
        {
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private async Task HandleAppExceptionAsync(HttpContext context, AppException ex)
    {
        _logger.LogError("Errore applicativo {ErrorCode}: {Message}", ex.ErrorCode, ex.Message);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            Code = ex.ErrorCode,
            Category = ex.Category,
            ex.Message
        }));
    }

    private async Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogCritical(ex, "Errore non gestito: {Message}", ex.Message);
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync("Si è verificato un errore interno.");
    }
}