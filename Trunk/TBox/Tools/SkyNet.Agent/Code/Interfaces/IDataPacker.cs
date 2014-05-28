using System.IO;

namespace Mnk.TBox.Tools.SkyNet.Agent.Code.Interfaces
{
    public interface IDataPacker
    {
        string Unpack(Stream stream);
    }
}
