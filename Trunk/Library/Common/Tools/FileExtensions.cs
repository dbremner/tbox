using System.IO;
using System.Text;

namespace Mnk.Library.Common.Tools
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

		public static void MoveIfExist(this FileInfo source, string destination)
		{
			if (!source.Exists) return;
			if (File.Exists(destination)) File.Delete(destination);
			source.CopyTo(destination);
            source.Delete();
		}
	}
}
