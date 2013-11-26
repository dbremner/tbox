using System;
using System.Xml.Serialization;
using Common.UI.Model;

namespace PluginsShared.Watcher
{
	[Serializable]
	public class DirInfo : CheckableData
	{
		[XmlAttribute]
		public string Mask { get; set; }
		[XmlAttribute]
		public FileDirection Direction { get; set; }
		[XmlAttribute]
		public string Path { get; set; }

		public DirInfo()
		{
			Mask = "*.*";
			Direction = FileDirection.Down;
		}

		public override object Clone()
		{
			return new DirInfo
			{
				Key = Key,
				IsChecked = IsChecked,
				Direction = Direction,
				Mask = Mask,
				Path = Path,
			};
		}
	}
}
