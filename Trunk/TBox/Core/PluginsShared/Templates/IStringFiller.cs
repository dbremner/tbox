﻿namespace PluginsShared.Templates
{
	public interface IStringFiller
	{
		bool CanFill(string value);
		string Fill(string value);
	}
}
