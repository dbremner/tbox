using System.IO;
using Interface;

namespace TBox.Code.Objects
{
	class DataProvider : IDataProvider
	{
		private readonly string dataPath;
		public DataProvider(string toolsPath, string dataPath)
		{
			this.dataPath = dataPath;
			ToolsPath = toolsPath;
		}

		public string DataPath
		{
			get
			{
				if (!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);
				return dataPath;
			}
		}

		public string ToolsPath { get; private set; }
	}
}
