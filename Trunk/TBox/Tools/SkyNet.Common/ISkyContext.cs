using Mnk.Library.Common.MT;

namespace Mnk.TBox.Tools.SkyNet.Common
{
    public interface ISkyContext : IUpdater
    {
        string TargetFolder { get; set; }
    }
}
