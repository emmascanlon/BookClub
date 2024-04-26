using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookClub.Bff.Exceptions;

public static class CustomExceptionHandler
{
    private class ProblemDetailsWithErrors : ProblemDetails
    {
        public List<string> Errors { get; set; }
    }

    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                await Respond(env, loggerFactory, context, contextFeature);
            });
        });

        return app;
    }

    private static async Task Respond(IHostEnvironment env, ILoggerFactory loggerFactory, HttpContext context, IExceptionHandlerFeature contextFeature)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        if (contextFeature != null)
        {
            var logger = loggerFactory.CreateLogger<Startup>();
            var isDomainException = contextFeature.Error is DomainException;
            var subErrors = (contextFeature.Error as AggregateDomainException)?.Errors;

            if (isDomainException)
            {
                logger.LogWarning(contextFeature.Error?.Message, subErrors);
            }
            else
            {
                logger.LogError(contextFeature.Error?.Message);
            }

            var statusCode = contextFeature.Error switch
            {
                DomainException _ => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            var problemDetails = new ProblemDetailsWithErrors
            {
                Status = (int)statusCode,
                Title = isDomainException ? contextFeature.Error.Message
                    : "We're sorry, an error occurred.",
                Errors = subErrors,
                Detail = env.IsDevelopment()
                    ? contextFeature.Error?.StackTrace
                    : "Enable development mode to see more details about this error",
                Type = (contextFeature.Error as DomainException)?.Type
            };

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}