using System.Runtime.Serialization;

namespace SkyNet.Common.Contracts.Server
{
    [DataContract]
    public enum ServerAgentState
    {
        Idle,
        Busy,
        Died
    }
}
