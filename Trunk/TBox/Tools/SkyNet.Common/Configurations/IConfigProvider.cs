using System.ServiceModel;

namespace SkyNet.Common.Configurations
{
    [ServiceContract]
    public interface IConfigProvider<T>
    {
        [OperationContract]
        T Get();

        [OperationContract]
        void Set(T config);
    }
}
