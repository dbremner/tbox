namespace Mnk.Library.WpfControls.Code.Content
{
	public interface IDataManager
	{
		object Create(string key);
		object Clone(object sample, string key);
		object ChangeKey(object target, string newKey);
		string GetKey(object target);
	}
}
