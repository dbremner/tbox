using System;
using System.IO;
using Common.Tools;
using ScriptEngine.Core;
using ScriptEngine.Core.Attributes;
using ScriptEngine.Core.Interfaces;

namespace ScriptEngine.Operations
{
	[OperationsFixture]
	public static class Folders
	{
		private static IScriptContext Sc
		{
			get { return ScriptCompiler.Sc; }
		}

		public static bool IsExist(string path)
		{
			return Directory.Exists(Sc.Resolve(path));
		}

		public static void Create(string path, bool throwErrorIfExist = false)
		{
			path = Sc.Resolve(path);
			if (Directory.Exists(path))
			{
				if (throwErrorIfExist) throw new ArgumentException("Directory already exist: " + path);
			}
			Directory.CreateDirectory(path);
			Sc.AddUndo("Folders.Delete", path);
		}

		public static void Copy(string source, string destination, bool overwrite = false)
		{
			source = Sc.Resolve(source);
			destination = Sc.Resolve(destination);
			if (!Directory.Exists(source))
			{
				throw new ArgumentException("Source directory not exist: " + source);
			}
			if (Directory.Exists(destination))
			{
				if (!overwrite) throw new ArgumentException("Directory already exist: " + destination);
				Move(destination, Sc.GenerateNextUndoPath());
			}
			new DirectoryInfo(source).CopyFilesTo(destination);
			Sc.AddUndo("Folders.Delete", destination);
		}

		public static void Move(string source, string destination, string mask="*.*", bool overwrite = false)
		{
			source = Sc.Resolve(source);
			destination = Sc.Resolve(destination);
			if (!Directory.Exists(source))
			{
				throw new ArgumentException("Source file not exist: " + source);
			}
			if (Directory.Exists(destination))
			{
				if (!overwrite) throw new ArgumentException("Directory already exist: " + destination);
				Move(destination, Sc.GenerateNextUndoPath(), mask);
			}
			new DirectoryInfo(source).MoveFilesTo(destination, mask);
			Sc.AddUndo("Folders.Move", destination, source, mask, overwrite);
		}

		public static void Delete(string path)
		{
			path = Sc.Resolve(path);
			if (!Directory.Exists(path))
			{
				throw new ArgumentException("Directory not exist: " + path);
			}
			Move(path, Sc.GenerateNextUndoPath());
		}

	}
}
