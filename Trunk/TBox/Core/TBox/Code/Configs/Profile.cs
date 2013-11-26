using Common.Tools;
using Common.UI.Model;
using Common.UI.ModelsContainers;

namespace TBox.Code.Configs
{
    public class Profile : Data
    {
        public CheckableDataCollection<CheckableData> Selected { get; set; }

        public Profile()
        {
            Selected = new CheckableDataCollection<CheckableData>();
        }

        public override object Clone()
        {
            return new Profile
                {
                    Key = Key,
                    Selected = Selected.Clone()
                };
        }
    }
}
