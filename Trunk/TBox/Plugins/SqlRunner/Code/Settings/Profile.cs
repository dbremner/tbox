using System;
using System.Collections.Generic;
using Common.Tools;
using Common.UI.Model;
using Common.UI.ModelsContainers;
using PluginsShared.Ddos.Settings;

namespace SqlRunner.Code.Settings
{
	[Serializable]
	public sealed class Profile : Data, IProfile
	{
		public CheckableDataCollection<Op> Ops { get; set; }

		public Profile()
		{
			Ops = new CheckableDataCollection<Op>();
		}

		public override object Clone()
		{
			return new Profile
			{
				Ops = Ops.Clone(),
			};
		}

		public IEnumerable<IOperation> GetOperations()
		{
			return Ops.CheckedItems;
		}
	}
}
