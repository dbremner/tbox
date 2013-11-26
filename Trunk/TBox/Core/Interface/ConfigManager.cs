namespace Interface
{
    public interface IConfigManager<out TConfig>
    {
        TConfig Config { get; }
    }

    public class ConfigManager<TConfig> : IConfigManager<TConfig>
    {
        public TConfig Config { get; set; }
    }
}
