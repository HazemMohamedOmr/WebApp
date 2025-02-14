using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Infrastructure.Exceptions;

public static class ProblemFactory
{
    public static ProblemDetails CreateProblemDetails(HttpContext httpContext, string title)
    {
        return CreateProblemDetails(httpContext, title, null, null);
    }

    public static ProblemDetails CreateProblemDetails(HttpContext httpContext, object errors)
    {
        return CreateProblemDetails(httpContext, null, null, errors);
    }

    public static ProblemDetails CreateProblemDetails(HttpContext httpContext, string title, string detail)
    {
        return CreateProblemDetails(httpContext, title, detail, null);
    }

    public static ProblemDetails CreateProblemDetails(HttpContext httpContext, string title, object errors)
    {
        return CreateProblemDetails(httpContext, title, null, errors);
    }

    public static ProblemDetails CreateProblemDetails(HttpContext httpContext, string? title, string? detail,
        object? errors)
    {
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        var statusCode = httpContext.Response.StatusCode < 400
            ? StatusCodes.Status400BadRequest
            : httpContext.Response.StatusCode;

        problemDetails.Status = statusCode;

        if (errors != null) problemDetails.Extensions["errors"] = errors;

        return problemDetails;
    }
}