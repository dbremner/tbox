using System;
using System.IO;

namespace TBox.Code
{
	static class Folders
	{
		public static readonly string UserFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TBox");
	}
}
