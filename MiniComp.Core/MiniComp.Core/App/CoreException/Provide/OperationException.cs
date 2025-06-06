using Grpc.Core;

namespace MiniComp.Core.App.CoreException.Provide;

/// <summary>
/// 业务操作失败抛出此异常
/// </summary>
public class OperationException : CustomException
{
    public OperationException(string message)
        : base(new Status(StatusCode.Unknown, message), message) { }

    public override WebApiResponse GetWebApiResponse()
    {
        return WebApiResponse.Error(Message);
    }
}
