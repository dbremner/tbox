using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ScriptEngine;

namespace Solution.Scripts
{
	public sealed class TestParams : IScript 
	{
		[Bool(false)]
		public bool BoolParam{get;set;}

		[String(false, "test")]
		public string StringParam{get;set;}

		[Guid(false, "fe1d4f8e-1764-4d49-9d48-759bffceeedd")]
		public Guid GuidParam{get;set;}

		[File(true, "d:\text.txt")]
		public string FileParam{get;set;}

		[Directory(true, "d:\test")]
		public string DirectoryParam{get;set;}

		[Integer(0,10,3)]
		public int IntParam{get;set;}

		[Double(-10,10,0.5)]
		public double RealParam{get;set;}

		[StringList(false, "1", "2", "3")]
		public string[] StringListParam{get;set;}

		[GuidList("C97F361A-FE72-4532-813B-F54BDF922770")]
		public IList<Guid> GuidListParam{get;set;}

		[FileList(true, "f1", "f2", "f3")]
		public IList<string> FileListParam{get;set;}

		[DirectoryList(false, "d1", "d2", "d3")]
		public IList<string> FolderListParam{get;set;}

		[IntegerList(0,10,1,2,3,4,5,6,7)]
		public IList<int> IntListParam{get;set;}

		[DoubleList(-100, 100, 0, -1, 10)]
		public IList<double> RealListParam{get;set;}

		[StringDictionary(false, "key", "value")]
		public IDictionary<string,string> StringDictionaryParam { get; set; }

		[GuidDictionary(false, "Test", "C97F361A-FE72-4532-813B-F54BDF922770")]
		public IDictionary<string, Guid> GuidDictionaryParam { get; set; }

		[FileDictionary(true, "file", "D:/test.txt", "file2", "D:/test2.txt")]
		public IDictionary<string, string> FileDictionaryParam { get; set; }

		[DirectoryDictionary(false)]
		public IDictionary<string, string> FolderDictionaryParam { get; set; }

		[IntegerDictionary(0, 10, "int", 3)]
		public IDictionary<string, int> IntDictionaryParam { get; set; }

		[DoubleDictionary(-100, 100, "double", 10.0)]
		public IDictionary<string, double> RealDictionaryParam { get; set; }

		public void Run()
		{
			MessageBox.Show(StringParam, "Hello world", MessageBoxButtons.OK, MessageBoxIcon.Information);
			MessageBox.Show(FileParam, "Hello world", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}
