using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApp.Infrastructure.Exceptions;

public static class ProblemFactory
{
    public static ProblemDetails CreateProblemDetails(HttpContext httpContext, string detail)
    {
        return CreateProblemDetails(httpContext, detail, null);
    }

    public static ProblemDetails CreateProblemDetails(HttpContext httpContext, object errors)
    {
        return CreateProblemDetails(httpContext, null, errors);
    }

    public static ProblemDetails CreateProblemDetails(HttpContext httpContext, string? detail, object? errors)
    {
        var problemDetails = new ProblemDetails
        {
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        var statusCode = httpContext.Response.StatusCode < 400
            ? StatusCodes.Status400BadRequest
            : httpContext.Response.StatusCode;

        problemDetails.Status = statusCode;
        problemDetails.Title = ReasonPhrases.GetReasonPhrase(statusCode);

        if (errors != null) problemDetails.Extensions["errors"] = errors;

        return problemDetails;
    }
}