using System;
using Common.Network;
using Common.UI.Model;

namespace Requestor.Code.Settings
{
	[Serializable]
	public sealed class Header : Data, IHeader
	{
		public string Value { get; set; }
		public override object Clone()
		{
			return new Header { Key = Key, Value = Value };
		}
	}
}
