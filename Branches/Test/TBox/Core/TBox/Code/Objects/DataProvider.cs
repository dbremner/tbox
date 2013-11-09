using System.IO;
using Interface;

namespace TBox.Code.Objects
{
	class DataProvider : IDataProvider
	{
		private readonly string writebleDataPath;
		public DataProvider(string toolsPath, string readOnlyDataPath, string writebleDataPath)
		{
			ReadOnlyDataPath = readOnlyDataPath;
			this.writebleDataPath = writebleDataPath;
			ToolsPath = toolsPath;
		}

		public string ReadOnlyDataPath { get; private set; }
		public string WritebleDataPath
		{
			get
			{
				if (!Directory.Exists(writebleDataPath)) Directory.CreateDirectory(writebleDataPath);
				return writebleDataPath;
			}
		}

		public string ToolsPath { get; private set; }
	}
}
