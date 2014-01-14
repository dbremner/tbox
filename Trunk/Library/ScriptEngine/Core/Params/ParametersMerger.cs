using System;
using System.Collections.Generic;
using System.Linq;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.Common.UI.ModelsContainers;
using ScriptEngine.Core.Params;

namespace Mnk.Library.ScriptEngine.Core.Params
{
	public sealed class ParametersMerger
	{
		public IList<Parameter> Merge(IList<Parameter> a, IList<Parameter> b)
		{
			var ret = new List<Parameter>();
			foreach (var p in a)
			{
				var p2 = b.FirstOrDefault(x => string.Equals(p.Key, x.Key));
				ret.Add(p2 != null ? Merge(p, p2) : p);
			}
			ret.AddRange(b.Where(x => !a.Any(y => string.Equals(x.Key, y.Key))));
			return ret.OrderBy(x=>x.Key).ToList();
		}

		public Parameter Merge(Parameter a, Parameter b)
		{
			if(!string.Equals(a.Key, b.Key))
				throw new ArgumentException("Parameters should have one key to merge");
			var p = DoMerge((dynamic)a, (dynamic)b);
			p.Key = a.Key;
			return p;
		}

		private static BoolParameter DoMerge(BoolParameter a, BoolParameter b)
		{
			var p = new BoolParameter{ Value = a.Value && b.Value};
			DoClarify(p, b);
			return p;
		}

		private static StringParameter DoMerge(StringParameter a, StringParameter b)
		{
			var p = new StringParameter { Value = string.IsNullOrEmpty(a.Value)?b.Value:a.Value, CanBeEmpty = a.CanBeEmpty};
			DoClarify(p, b);
			return p;
		}

		private static GuidParameter DoMerge(GuidParameter a, GuidParameter b)
		{
			var p = new GuidParameter { Value = (a.Autogenerate)? b.Value : a.Value, Autogenerate = a.Autogenerate };
			DoClarify(p, b);
			return p;
		}

		private static FileParameter DoMerge(FileParameter a, FileParameter b)
		{
			var p = new FileParameter { Value = (a.ShouldExist) ? a.Value : b.Value, ShouldExist = a.ShouldExist};
			DoClarify(p, b);
			return p;
		}

		private static DirectoryParameter DoMerge(DirectoryParameter a, DirectoryParameter b)
		{
			var p = new DirectoryParameter { Value = (a.ShouldExist) ? a.Value : b.Value, ShouldExist = a.ShouldExist};
			DoClarify(p, b);
			return p;
		}

		private static IntegerParameter DoMerge(IntegerParameter a, IntegerParameter b)
		{
			var p = new IntegerParameter { Min = a.Min, Max = a.Max};
			DoClarify(p, b);
			if (a.Value > p.Max || a.Value < p.Min)
			{
				p.Value = Math.Max(p.Min, Math.Min(p.Max, b.Value));
			}
			else
			{
				p.Value = Math.Max(p.Min, Math.Min(p.Max, a.Value));
			}
			return p;
		}

		private static DoubleParameter DoMerge(DoubleParameter a, DoubleParameter b)
		{
			var p = new DoubleParameter{ Min = a.Min, Max = a.Max };
			DoClarify(p, b);
			if (a.Value > p.Max || a.Value < p.Min)
			{
				p.Value = Math.Max(p.Min, Math.Min(p.Max, b.Value));
			}
			else
			{
				p.Value = Math.Max(p.Min, Math.Min(p.Max, a.Value));
			} 
			return p;
		}

		private static StringListParameter DoMerge(StringListParameter a, StringListParameter b)
		{
			var p = new StringListParameter { CanBeEmpty = a.CanBeEmpty};
			DoClarify(p, b);
			p.Values = new CheckableDataCollection<CheckableData<string>>(
				a.Values.Concat(b.Values).Where(x=>ParametersValidator.ValidateValue(p,x.Value))
				.DistinctByStringValue());
			return p;
		}

		private static GuidListParameter DoMerge(GuidListParameter a, GuidListParameter b)
		{
			var p = new GuidListParameter();
			DoClarify(p, b);
			p.Values = new CheckableDataCollection<CheckableData<Guid>>(
				a.Values.Concat(b.Values).Where(x => ParametersValidator.ValidateValue(p, x.Value))
				.DistinctByGuidValue());
			return p;
		}

		private static FileListParameter DoMerge(FileListParameter a, FileListParameter b)
		{
			var p = new FileListParameter {ShouldExist = a.ShouldExist};
			DoClarify(p, b);
			p.Values = new CheckableDataCollection<CheckableData<string>>(
				a.Values.Concat(b.Values).Where(x => ParametersValidator.ValidateValue(p, x.Value))
				.DistinctByStringValue());
			return p;
		}

		private static DirectoryListParameter DoMerge(DirectoryListParameter a, DirectoryListParameter b)
		{
			var p = new DirectoryListParameter { ShouldExist = a.ShouldExist};
			DoClarify(p, b);
			p.Values = new CheckableDataCollection<CheckableData<string>>(
				a.Values.Concat(b.Values).Where(x => ParametersValidator.ValidateValue(p, x.Value))
				.DistinctByStringValue());
			return p;
		}

		private static IntegerListParameter DoMerge(IntegerListParameter a, IntegerListParameter b)
		{
			var p = new IntegerListParameter { Min = a.Min, Max = a.Max};
			DoClarify(p, b);
			p.Values = new CheckableDataCollection<CheckableData<int>>(
				a.Values.Concat(b.Values).Where(x => ParametersValidator.ValidateValue(p, x.Value))
				.DistinctByIntegerValue());
			return p;
		}

		private static DoubleListParameter DoMerge(DoubleListParameter a, DoubleListParameter b)
		{
			var p = new DoubleListParameter { Min = a.Min, Max = a.Max };
			DoClarify(p, b);
			p.Values = new CheckableDataCollection<CheckableData<double>>(
				a.Values.Concat(b.Values).Where(x => ParametersValidator.ValidateValue(p, x.Value))
				.DistinctByDoubleValue());
			return p;
		}

		private static StringDictionaryParameter DoMerge(StringDictionaryParameter a, StringDictionaryParameter b)
		{
			var p = new StringDictionaryParameter { CanBeEmpty = a.CanBeEmpty };
			DoClarify(p, b);
			p.Values = new CheckableDataCollection<CheckableData<string>>(
				b.Values.Concat(a.Values).Where(x => ParametersValidator.ValidateValue(p, x.Value)));
			return p;
		}

		private static BoolDictionaryParameter DoMerge(BoolDictionaryParameter a, BoolDictionaryParameter b)
		{
			var p = new BoolDictionaryParameter();
			DoClarify(p, b);
			p.Values = new CheckableDataCollection<CheckableData<bool>>(
				b.Values.Concat(a.Values).Where(x => ParametersValidator.ValidateValue(p, x.Value)));
			return p;
		}

		private static GuidDictionaryParameter DoMerge(GuidDictionaryParameter a, GuidDictionaryParameter b)
		{
			var p = new GuidDictionaryParameter{Autogenerate = a.Autogenerate};
			DoClarify(p, b);
			p.Values = new CheckableDataCollection<CheckableData<Guid>>(
				b.Values.Concat(a.Values).Where(x => ParametersValidator.ValidateValue(p, x.Value)));
			return p;
		}

		private static FileDictionaryParameter DoMerge(FileDictionaryParameter a, FileDictionaryParameter b)
		{
			var p = new FileDictionaryParameter { ShouldExist = a.ShouldExist };
			DoClarify(p, b);
			p.Values = new CheckableDataCollection<CheckableData<string>>(
				b.Values.Concat(a.Values).Where(x => ParametersValidator.ValidateValue(p, x.Value)));
			return p;
		}

		private static DirectoryDictionaryParameter DoMerge(DirectoryDictionaryParameter a, DirectoryDictionaryParameter b)
		{
			var p = new DirectoryDictionaryParameter { ShouldExist = a.ShouldExist };
			DoClarify(p, b);
			p.Values = new CheckableDataCollection<CheckableData<string>>(
				b.Values.Concat(a.Values).Where(x => ParametersValidator.ValidateValue(p, x.Value)));
			return p;
		}

		private static IntegerDictionaryParameter DoMerge(IntegerDictionaryParameter a, IntegerDictionaryParameter b)
		{
			var p = new IntegerDictionaryParameter { Min = a.Min, Max = a.Max };
			DoClarify(p, b);
			p.Values = new CheckableDataCollection<CheckableData<int>>(
				b.Values.Concat(a.Values).Where(x => ParametersValidator.ValidateValue(p, x.Value)));
			return p;
		}

		private static DoubleDictionaryParameter DoMerge(DoubleDictionaryParameter a, DoubleDictionaryParameter b)
		{
			var p = new DoubleDictionaryParameter { Min = a.Min, Max = a.Max };
			DoClarify(p, b);
			p.Values = new CheckableDataCollection<CheckableData<double>>(
				b.Values.Concat(a.Values).Where(x => ParametersValidator.ValidateValue(p, x.Value)));
			return p;
		}

		public IList<Parameter> Clarify(IList<Parameter> current, IList<Parameter> known)
		{
			foreach (var p in current.ToArray())
			{
				var p2 = known.FirstOrDefault(x => string.Equals(p.Key, x.Key));
				if (p2 == null || p.GetType()!=p2.GetType()) current.Remove(p);
				else Clarify(p, p2);
			}
			foreach (var p in known.Where(x => !current.Any(y => string.Equals(x.Key, y.Key))))
			{
				current.Add(p);
			}
			return current.OrderBy(x=>x.Key).ToList();
		}

		private static void Clarify(Parameter current, Parameter exist)
		{
			DoClarify((dynamic)current, (dynamic)exist);
		}

		private static void DoClarify(IBoolParameter current, IBoolParameter exist)
		{
		}

		private static void DoClarify(IStringParameter current, IStringParameter exist)
		{
			current.CanBeEmpty &= exist.CanBeEmpty;
		}

		private static void DoClarify(IFileParameter current, IFileParameter exist)
		{
			current.ShouldExist |= exist.ShouldExist;
		}

		private static void DoClarify(IDirectoryParameter current, IDirectoryParameter exist)
		{
			current.ShouldExist |= exist.ShouldExist;
		}

		private static void DoClarify(IIntegerParameter current, IIntegerParameter exist)
		{
			current.Max = Math.Min(current.Max, exist.Max);
			current.Min = Math.Max(current.Min, exist.Min);
		}

		private static void DoClarify(IDoubleParameter current, IDoubleParameter exist)
		{
			current.Max = Math.Min(current.Max, exist.Max);
			current.Min = Math.Max(current.Min, exist.Min);
		}

		private static void DoClarify(GuidParameter current, GuidParameter exist)
		{
			current.Autogenerate &= exist.Autogenerate;
			if (current.Autogenerate) current.Value = Guid.NewGuid();
		}

		private static void DoClarify(GuidListParameter current, GuidListParameter exist)
		{
		}

		private static void DoClarify(GuidDictionaryParameter current, GuidDictionaryParameter exist)
		{
			current.Autogenerate &= exist.Autogenerate;
			if (!current.Autogenerate) return;
			foreach (var data in current.Values)
			{
				data.Value = Guid.NewGuid();
			}
		}
	}
}
