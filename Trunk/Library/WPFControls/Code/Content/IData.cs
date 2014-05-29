using System;

namespace Mnk.Library.WpfControls.Code.Content
{
	public interface IData
	{
		void Init(Action<bool> onChange);
		void Load();
		void Save();
		bool Changed { get; }
	}
}
