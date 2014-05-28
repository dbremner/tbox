using Mnk.TBox.Tools.SkyNet.Server.Code.Settings;

namespace Mnk.TBox.Tools.SkyNet.Server.Code.Interfaces
{
    public interface IStorage
    {
        void Save();
        Config Config { get; }
    }
}