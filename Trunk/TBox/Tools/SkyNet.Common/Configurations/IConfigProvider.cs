using System.ServiceModel;

namespace Mnk.TBox.Tools.SkyNet.Common.Configurations
{
    [ServiceContract]
    public interface IConfigProvider<T>
    {
        [OperationContract]
        T ReceiveConfig();

        [OperationContract]
        void UpdateConfig(T config);
    }
}
