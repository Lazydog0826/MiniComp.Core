using System.Net;
using Grpc.Core;

namespace MiniComp.Core.App.CoreException.Provide;

/// <summary>
/// 数据验证失败抛出此异常
/// </summary>
public class DataValidationException : CustomException
{
    private readonly IDictionary<string, object> _errors;

    public DataValidationException(
        IDictionary<string, object> errors,
        string message = "数据验证未通过"
    )
        : base(new Status(StatusCode.InvalidArgument, message), message)
    {
        _errors = errors;
        base.Data.Add(nameof(_errors), _errors);
    }

    public override WebApiResponse GetWebApiResponse()
    {
        return WebApiResponse.Error(Message, HttpStatusCode.BadRequest, _errors);
    }
}
