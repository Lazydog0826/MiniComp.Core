using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MiniComp.Core.App.Middleware;

public class SetHttpContextMiddleware
{
    private readonly RequestDelegate _next;

    public SetHttpContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        WebApp.SetHttpContext(
            httpContext.RequestServices.GetRequiredService<IHttpContextAccessor>()
        );
        await _next(httpContext);
    }
}
