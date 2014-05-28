using System.Net;

namespace Mnk.Library.Common.Communications
{
    public interface IHttpContextHelper
    {
        void SetStatusCode(HttpStatusCode status);
    }
}