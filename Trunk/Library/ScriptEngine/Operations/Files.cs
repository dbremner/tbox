using System;
using System.IO;
using ScriptEngine.Core;
using ScriptEngine.Core.Attributes;
using ScriptEngine.Core.Interfaces;

namespace ScriptEngine.Operations
{
	[OperationsFixture]
	public static class Files
	{
		private static IScriptContext Sc
		{
			get { return ScriptCompiler.Sc; }
		}

		public static bool IsExist(string path)
		{
			return File.Exists(Sc.Resolve(path));
		}

		public static void Create(string path, bool overwrite = false)
		{
			path = Sc.Resolve(path);
			if (File.Exists(path))
			{
				if (!overwrite) throw new ArgumentException("File already exist: " + path);
				Move(path, Sc.GenerateNextUndoPath(), true);
			}
			File.Create(path);
			Sc.AddUndo("Files.Delete", path);
		}

		public static void Copy(string source, string destination, bool overwrite = false)
		{
			source = Sc.Resolve(source);
			destination = Sc.Resolve(destination);
			if (!File.Exists(source))
			{
				throw new ArgumentException("Source file not exist: " + source);
			}
			if (File.Exists(destination))
			{
				if (!overwrite) throw new ArgumentException("File already exist: " + destination);
				Move(destination, Sc.GenerateNextUndoPath());
			}
			File.Copy(source, destination);
			Sc.AddUndo("Files.Delete", destination);
		}

		public static void Move(string source, string destination, bool overwrite = false)
		{
			source = Sc.Resolve(source);
			destination = Sc.Resolve(destination);
			if (!File.Exists(source))
			{
				throw new ArgumentException("Source file not exist: " + source);
			}
			if (File.Exists(destination))
			{
				if (!overwrite) throw new ArgumentException("File already exist: " + destination);
				Move(destination, Sc.GenerateNextUndoPath());
			}
			File.Move(source, destination);
			Sc.AddUndo("Files.Move", destination, source, overwrite);
		}

		public static void Delete(string path)
		{
			path = Sc.Resolve(path);
			if (!File.Exists(path))
			{
				throw new ArgumentException("File not exist: " + path);
			}
			Move(path, Sc.GenerateNextUndoPath());
		}

	}
}
