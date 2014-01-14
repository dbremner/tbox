namespace Mnk.TBox.Core.PluginsShared.Ddos.Settings
{
	public interface IOperation
	{
		int Threads { get; set; }
		int Delay { get; set; }
		int Timeout { get; set; }
		bool IsChecked { get; set; }
		string Key { get; set; }
	}
}
