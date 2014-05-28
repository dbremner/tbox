using System.Net;
using System.ServiceModel.Web;

namespace Mnk.Library.Common.Communications
{
    public class HttpContextHelper : IHttpContextHelper
    {
        public void SetStatusCode(HttpStatusCode status)
        {
            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = status;
            }
        }
    }
}
