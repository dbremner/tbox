using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace Mnk.Library.ScriptEngine.Core
{
	public class CompilerExceptions : Exception
	{
		public IList<CompilerError> Errors { get; private set; }
		public CompilerExceptions()
		{
			Errors = new List<CompilerError>();
		}

		public override string ToString()
		{
			var sb = new StringBuilder("Compilation error: ");
			foreach (var error in Errors)
			{
				sb.AppendLine().AppendFormat("[{0},{1}] {2}", error.Line, error.Column, error.ErrorText);
			}
			return sb.ToString();
		}
	}
}
