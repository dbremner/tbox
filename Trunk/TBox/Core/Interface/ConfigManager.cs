namespace Interface
{
    public interface IConfigManager<TConfig>
    {
        TConfig Config { get; set; }
    }

    public class ConfigManager<TConfig> : IConfigManager<TConfig>
    {
        public TConfig Config { get; set; }
    }
}
