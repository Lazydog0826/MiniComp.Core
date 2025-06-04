using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MiniComp.Core.App.CoreException.Provide;
using MiniComp.Core.AppAttribute;
using MiniComp.Core.Extension;

namespace MiniComp.Core.App.Filter;

/// <summary>
/// 模型验证与响应处理过滤器，顺序：100（从小到大执行）
/// </summary>
public class CoreActionFilterAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        if (!context.ModelState.IsValid)
        {
            var error = new Dictionary<string, object>();
            context
                .ModelState.ToList()
                .ForEach(x =>
                {
                    error.Add(x.Key, x.Value?.Errors.Select(x2 => x2.ErrorMessage).ToList() ?? []);
                });
            throw new DataValidationException(error);
        }
        await base.OnActionExecutionAsync(context, next);
    }

    public override async Task OnResultExecutionAsync(
        ResultExecutingContext context,
        ResultExecutionDelegate next
    )
    {
        if (WebApp.HttpContext?.GetMetaData<NoHandleResponse>() == null)
        {
            var res =
                context.Result.GetType() == typeof(EmptyResult)
                    ? WebApiResponse.Operate()
                    : WebApiResponse.Query((context.Result as ObjectResult)?.Value ?? new { });
            var stagingRequest = new OkObjectResult(res);
            context.Result = stagingRequest;
        }
        await base.OnResultExecutionAsync(context, next);
    }
}
