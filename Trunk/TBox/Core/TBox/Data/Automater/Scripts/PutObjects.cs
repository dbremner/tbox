using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using ICSharpCode.SharpZipLib.Zip;
using PluginsShared.Automator;
using ScriptEngine;
using WPFControls.Dialogs;

namespace Solution.Scripts
{
	public class PutObjects : IScript
	{
		[DirectoryList]
		public string[] TargetPathes { get; set; }

		[Directory]
		public string PathToDirectoryWithPackage { get; set; }

		public bool RemoveAfterUnpack { get; set; }

		[String("Sample.*.zip")]
		public string PackageMask { get; set; }

		[StringList("Sample.*.dll", "Sample.*.pdb")]
		public string[] FilesMasks { get; set; }

		public void Run(IScriptContext context)
		{
			bool exist;
			var package = GetPackage(context, out exist);
			if (!exist) return;
			if (package == null)
			{
				MessageBox.Show("Can't find any package in folder: " + PathToDirectoryWithPackage, PackageMask);
				return;
			}
			UnpackPackage(package);
			if(RemoveAfterUnpack)package.Delete();
		}

		private FileInfo GetPackage(IScriptContext context, out bool exist)
		{
			exist = false;
			var packages = new DirectoryInfo(PathToDirectoryWithPackage)
				.EnumerateFiles(PackageMask)
				.OrderBy(x => x.CreationTime)
				.Reverse()
				.ToArray();
			FileInfo package = null;
			if (packages.Length > 0)
			{
			    var result = new KeyValuePair<bool, string>();
			    context.Sync(() =>
                    result = DialogsCache.ShowInputSelect("Select file to extract", "Script", packages.First().Name, x => true,
                                                          packages.Select(x => x.Name + "\t" + x.CreationTime).ToArray(), null, true)
                    );
				exist = result.Key;
                var value = result.Value.Split('\t').First();
				package = packages.FirstOrDefault(x => string.Equals(x.Name, value));
			}
			return package;
		}

		private void UnpackPackage(FileInfo package)
		{
			using (var f = package.Open(FileMode.Open))
			{
				using (var zf = new ZipFile(f))
				{
					foreach (var entry in zf.Cast<ZipEntry>().Where(entry => CanUnpack(entry.Name)))
					{
						using (var zs = zf.GetInputStream(entry))
						{
							using (var ms = new MemoryStream())
							{
								zs.CopyTo(ms);
								Save(ms, entry.Name);
							}
						}
					}
				}
			}
		}

		private void Save(Stream s, string name)
		{
			foreach (var path in TargetPathes)
			{
				var targetDir = Path.Combine(path, Path.GetFileNameWithoutExtension(name));
				if (!Directory.Exists(targetDir))continue;
				var targetPath = Path.Combine(targetDir, name);
				if (File.Exists(targetPath)) File.Delete(targetPath);
				using (var f = File.OpenWrite(targetPath))
				{
					s.Position = 0;
					s.CopyTo(f);
				}
			}
		}

		private bool CanUnpack(string fileName)
		{
			return FilesMasks.Any(x => FitsMask(fileName, x));
		}

		private static bool FitsMask(string fileName, string fileMask)
		{
			return new Regex(fileMask.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."))
				.IsMatch(fileName);
		}
	}
}
