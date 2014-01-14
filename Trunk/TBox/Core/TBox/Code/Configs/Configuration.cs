using System.Collections.ObjectModel;
using Mnk.Library.Common.UI.Model;

namespace Mnk.TBox.Core.Application.Code.Configs
{
    public class Configuration
    {
        public bool PortableMode { get; set; }
        public ObservableCollection<Profile> Profiles { get; set; }
        public string SelectedProfile { get; set; }

        public Configuration()
        {
            PortableMode = false;
            Profiles = new ObservableCollection<Profile>();
            SelectedProfile = string.Empty;
        }
    }
}
