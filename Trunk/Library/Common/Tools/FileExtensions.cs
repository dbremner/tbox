using System.IO;
using System.Text;

namespace Common.Tools
{
	public static class FileExtensions
	{
		public static string ReadBegin(this FileInfo info, int size)
		{
			using (var s = info.OpenRead())
			{
				var data = new byte[size];
				s.Read(data, 0, data.Length);
				return Encoding.UTF8.GetString(data);
			}
		}
	}
}
