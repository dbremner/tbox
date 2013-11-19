using System;
using Common.Tools;

namespace ScriptEngine
{
	[Serializable]
	public sealed class SingleFileOperation : Operation
	{
        public string Path { get; set; }

		public override object Clone()
		{
            return new SingleFileOperation
			{
				Path = Path,
				Parameters = Parameters.Clone(),
				Key = Key,
                IsChecked = IsChecked
			};
		}
	}
}
