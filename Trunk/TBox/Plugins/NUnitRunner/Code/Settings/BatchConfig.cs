namespace NUnitRunner.Code.Settings
{
    public class BatchConfig
    {
        public bool NeedSync { get; set; }
        public bool UseCategories { get; set; }
        public bool IncludeCategories { get; set; }
        public bool ShowOnlyFailed { get; set; }

        public BatchConfig()
        {
            ShowOnlyFailed = true;
            NeedSync = true;
            UseCategories = false;
            IncludeCategories = true;
        }
    }
}
