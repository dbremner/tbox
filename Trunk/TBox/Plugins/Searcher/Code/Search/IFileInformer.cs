namespace Mnk.TBox.Plugins.Searcher.Code.Search
{
	interface IFileInformer
	{
		string GetFilePath(int id);
		string GetFileExt(int id);
	}
}
