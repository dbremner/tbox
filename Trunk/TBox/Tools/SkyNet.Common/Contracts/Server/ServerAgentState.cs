using System.Runtime.Serialization;

namespace Mnk.TBox.Tools.SkyNet.Common.Contracts.Server
{
    [DataContract]
    public enum ServerAgentState
    {
        Idle,
        Busy,
        Died
    }
}
