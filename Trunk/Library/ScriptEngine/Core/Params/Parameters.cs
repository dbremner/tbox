using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.Common.UI.ModelsContainers;
using Mnk.Library.ScriptEngine.Core.Params;

// for compability reason don't change this namespace
namespace ScriptEngine.Core.Params
{
	public interface IBoolParameter { }
	public interface IGuidParameter { }
	public interface IStringParameter { bool CanBeEmpty { get; set; } }
	public interface IFileParameter { bool ShouldExist { get; set; } }
	public interface IDirectoryParameter { bool ShouldExist { get; set; } }
	public interface IIntegerParameter { int Min { get; set; } int Max { get; set; } }
	public interface IDoubleParameter { double Min { get; set; } double Max { get; set; } }

	public abstract class Parameter : Data
	{
		public abstract object GetValue();
		public bool Validate()
		{
			return ParametersValidator.Validate(this);
		}
	}

	public abstract class Parameter<T> : Parameter
	{
		public T Value { get; set; }
		public override object GetValue() { return Value; }
	}

	public sealed class BoolParameter : Parameter<bool>, IBoolParameter
	{
		public override object Clone()
		{
			return new BoolParameter {Key = Key, Value = Value};
		}
	}

	public sealed class StringParameter : Parameter<string>, IStringParameter
	{
		public bool CanBeEmpty { get; set; }
		public override object Clone()
		{
			return new StringParameter { Key = Key, Value = Value, CanBeEmpty = CanBeEmpty};
		}
	}

    public sealed class PasswordParameter : Parameter<string>, IStringParameter
	{
		public bool CanBeEmpty { get; set; }
		public override object Clone()
		{
            return new PasswordParameter { Key = Key, Value = Value, CanBeEmpty = CanBeEmpty };
		}
	}

	public sealed class GuidParameter : Parameter<Guid>, IGuidParameter
	{
		public bool Autogenerate { get; set; }
		public override object Clone()
		{
			return new GuidParameter { Key = Key, Value = Value, Autogenerate = Autogenerate };
		}
	}

	public sealed class FileParameter : Parameter<string>, IFileParameter
	{
		public bool ShouldExist { get; set; }
		public override object Clone()
		{
			return new FileParameter { Key = Key, Value = Value, ShouldExist = ShouldExist };
		}
	}

	public sealed class DirectoryParameter : Parameter<string>, IDirectoryParameter
	{
		public bool ShouldExist { get; set; }
		public override object Clone()
		{
			return new DirectoryParameter { Key = Key, Value = Value, ShouldExist = ShouldExist };
		}
	}

	public sealed class IntegerParameter : Parameter<int>, IIntegerParameter
	{
		public int Min { get; set; }
		public int Max { get; set; }
		public override object Clone()
		{
			return new IntegerParameter { Key = Key, Value = Value, Min = Min, Max = Max};
		}
	}

	public sealed class DoubleParameter : Parameter<double>, IDoubleParameter
	{
		public double Min { get; set; }
		public double Max { get; set; }
		public override object Clone()
		{
			return new DoubleParameter { Key = Key, Value = Value, Min = Min, Max = Max };
		}
	}


	public abstract class ListParameter<T> : Parameter
	{
		public CheckableDataCollection<CheckableData<T>> Values { get; set; }
		protected ListParameter(){Values = new CheckableDataCollection<CheckableData<T>>();}
		public override object GetValue() { return Values.CheckedItems.Select(x => x.Value).ToArray(); }
		public IEnumerable<CheckableData<T>> Checked {get{return Values.CheckedItems;}}
	}

	public sealed class StringListParameter : ListParameter<string>, IStringParameter
	{
		public bool CanBeEmpty { get; set; }
		public override object Clone()
		{
			return new StringListParameter { Key = Key, Values = Values.Clone(), CanBeEmpty = CanBeEmpty};
		}
	}

	public sealed class GuidListParameter : ListParameter<Guid>, IGuidParameter
	{
		public override object Clone()
		{
			return new GuidListParameter { Key = Key, Values = Values.Clone()};
		}
	}

	public sealed class FileListParameter : ListParameter<string>, IFileParameter
	{
		public bool ShouldExist { get; set; }
		public override object Clone()
		{
			return new FileListParameter { Key = Key, Values = Values.Clone(), ShouldExist = ShouldExist };
		}
	}

	public sealed class DirectoryListParameter : ListParameter<string>, IDirectoryParameter
	{
		public bool ShouldExist { get; set; }
		public override object Clone()
		{
			return new DirectoryListParameter { Key = Key, Values = Values.Clone(), ShouldExist = ShouldExist };
		}
	}

	public sealed class IntegerListParameter : ListParameter<int>, IIntegerParameter
	{
		public int Min { get; set; }
		public int Max { get; set; }
		public override object Clone()
		{
			return new IntegerListParameter { Key = Key, Values = Values.Clone(), Min = Min, Max = Max};
		}
	}

	public sealed class DoubleListParameter : ListParameter<double>, IDoubleParameter
	{
		public double Min { get; set; }
		public double Max { get; set; }
		public override object Clone()
		{
			return new DoubleListParameter { Key = Key, Values = Values.Clone(), Min = Min, Max = Max };
		}
	}

	public abstract class DictionaryParameter<T> : Parameter
	{
		public CheckableDataCollection<CheckableData<T>> Values { get; set; }
		protected DictionaryParameter() { Values = new CheckableDataCollection<CheckableData<T>>(); }
		public override object GetValue() { return Values.CheckedItems.ToDictionary(x=>x.Key, x => x.Value); }
		public IEnumerable<CheckableData<T>> Checked { get { return Values.CheckedItems; } }
	}

	public sealed class StringDictionaryParameter : DictionaryParameter<string>, IStringParameter
	{
		public bool CanBeEmpty { get; set; }
		public override object Clone()
		{
			return new StringDictionaryParameter { Key = Key, Values = Values.Clone(), CanBeEmpty = CanBeEmpty};
		}
	}

	public sealed class BoolDictionaryParameter : DictionaryParameter<bool>, IBoolParameter
	{
		public override object Clone()
		{
			return new BoolDictionaryParameter { Key = Key, Values = Values.Clone()};
		}
	}

	public sealed class GuidDictionaryParameter : DictionaryParameter<Guid>, IGuidParameter
	{
		public bool Autogenerate { get; set; }
		public override object Clone()
		{
			return new GuidDictionaryParameter { Key = Key, Values = Values.Clone(), Autogenerate = Autogenerate };
		}
	}

	public sealed class FileDictionaryParameter : DictionaryParameter<string>, IFileParameter
	{
		public bool ShouldExist { get; set; }
		public override object Clone()
		{
			return new FileDictionaryParameter { Key = Key, Values = Values.Clone(), ShouldExist = ShouldExist };
		}
	}

	public sealed class DirectoryDictionaryParameter : DictionaryParameter<string>, IDirectoryParameter
	{
		public bool ShouldExist { get; set; }
		public override object Clone()
		{
			return new DirectoryDictionaryParameter { Key = Key, Values = Values.Clone(), ShouldExist = ShouldExist };
		}
	}

	public sealed class IntegerDictionaryParameter : DictionaryParameter<int>, IIntegerParameter
	{
		public int Min { get; set; }
		public int Max { get; set; }
		public override object Clone()
		{
			return new IntegerDictionaryParameter { Key = Key, Values = Values.Clone(), Min = Min, Max = Max};
		}
	}

	public sealed class DoubleDictionaryParameter : DictionaryParameter<double>, IDoubleParameter
	{
		public double Min { get; set; }
		public double Max { get; set; }
		public override object Clone()
		{
			return new DoubleDictionaryParameter { Key = Key, Values = Values.Clone(), Min = Min, Max = Max };
		}
	}
}
