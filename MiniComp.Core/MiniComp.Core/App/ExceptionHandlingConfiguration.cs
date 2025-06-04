using System.Net;

namespace MiniComp.Core.App
{
    public class ExceptionHandlingConfiguration
    {
        public Func<Exception, (object, bool, HttpStatusCode)>? ExceptionHandl { get; set; } = null;
    }
}
