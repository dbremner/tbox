using Common.UI.Model;

namespace TeamManager.Code.Settings
{
    public class SpecialDay : Data
    {
        public bool IsHolyday { get; set; }

        public SpecialDay()
        {
            IsHolyday = true;
        }

        public override object Clone()
        {
            return new SpecialDay
                {
                    Key = Key,
                    IsHolyday = IsHolyday
                };
        }
    }
}
