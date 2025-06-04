using Grpc.Core;

namespace MiniComp.Core.App.CoreException;

public abstract class CustomException : RpcException
{
    protected CustomException(Status status)
        : base(status) { }

    protected CustomException(Status status, Metadata trailers)
        : base(status, trailers) { }

    protected CustomException(Status status, Metadata trailers, string message)
        : base(status, trailers, message) { }

    protected CustomException(Status status, string message)
        : base(status, message) { }

    public abstract WebApiResponse GetWebApiCallBack();
}
