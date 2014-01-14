using System;

namespace Mnk.Library.Common.Tools
{
	public sealed class StringTemplate
	{
		private readonly string begin;
		private readonly string end;
		public StringTemplate(string template, string itemTemplate)
		{
			var start = template.IndexOf(itemTemplate, StringComparison.Ordinal);
			if(start == -1)throw new ArgumentException("Can't find item in template");
			begin = template.Substring(0, start);
			var length = start + itemTemplate.Length;
			end = template.Substring(length, template.Length - length);
		}

		public bool TryParse(string text, out string value)
		{
			value = string.Empty;
			var start = text.IndexOf(begin, StringComparison.Ordinal);
			if (start == -1) return false;
			start += begin.Length;
			var last = string.IsNullOrEmpty(end) ? 
				text.Length :
				text.LastIndexOf(end, StringComparison.Ordinal);
			if (last == -1 || last <= start) return false;
			value = text.Substring(start, last - start);
			return true;
		}
	}
}
