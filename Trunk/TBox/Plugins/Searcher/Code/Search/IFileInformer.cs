namespace Searcher.Code.Search
{
	interface IFileInformer
	{
		string GetFilePath(int id);
		string GetFileExt(int id);
	}
}
