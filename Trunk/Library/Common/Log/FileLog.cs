using System;
using System.IO;

namespace Mnk.Library.Common.Log
{
	public class FileLog : AbstractLog
	{
		private readonly object locker = new object();
		private readonly string path;
		public FileLog(string filePath)
		{
			path = filePath;
			if(File.Exists(path))
			{
				File.Delete(path);
			}
		}

		public override void Write(string value)
		{
			lock (locker)
			{
				using (var s = new StreamWriter(path, true))
				{
					s.WriteLine("{0}: {1}", DateTime.Now, value);
				}
			}
		}
	}
}
