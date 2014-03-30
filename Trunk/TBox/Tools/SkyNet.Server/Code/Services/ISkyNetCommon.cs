using System.ServiceModel;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Files;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Server;

namespace Mnk.TBox.Tools.SkyNet.Server.Code.Services
{
    [ServiceContract]
    public interface ISkyNetCommon : ISkyNetServerService, ISkyNetFileService
    {
    }
}
