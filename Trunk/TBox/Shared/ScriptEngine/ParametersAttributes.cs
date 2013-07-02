using System;
using System.Collections.Generic;
using System.Linq;
using Common.UI.Model;
using Common.UI.ModelsContainers;
using ScriptEngine.Core.Params;
using Common.Tools;

namespace ScriptEngine
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public abstract class ParameterAttribute : Attribute
	{
		public abstract Parameter CreateParameter();
		public abstract Type GetTypeForAttribute();
	}

	public sealed class BoolAttribute : ParameterAttribute
	{
		public bool Value { get; set; }
		public BoolAttribute(bool value=true)
		{
			Value = value;
		}
		public override Parameter CreateParameter(){return new BoolParameter { Value = Value };}
		public override Type GetTypeForAttribute() { return typeof(bool); }
	}

	public sealed class StringAttribute : ParameterAttribute
	{
		public string Value { get; set; }
		public bool CanBeEmpty { get; set; }

		public StringAttribute(bool canBeEmpty = false, string value = "" )
		{
			CanBeEmpty = canBeEmpty;
			Value = value;
		}
		public override Parameter CreateParameter() { return new StringParameter { CanBeEmpty = CanBeEmpty, Value = Value }; }
		public override Type GetTypeForAttribute() { return typeof(string); }
	}

	public sealed class GuidAttribute : ParameterAttribute
	{
		public Guid Value { get; set; }
		public bool Autogenerate { get; set; }

		public GuidAttribute(bool autogenerate = true, string value = "")
		{
			Autogenerate = autogenerate;
			Value = autogenerate ? Guid.NewGuid() : Guid.Parse(value);
		}
		public override Parameter CreateParameter() { return new GuidParameter { Autogenerate = Autogenerate, Value = Value }; }
		public override Type GetTypeForAttribute() { return typeof(Guid); }
	}

	public sealed class FileAttribute : ParameterAttribute
	{
		public string Value { get; set; }
		public bool ShouldExist { get; set; }

		public FileAttribute(bool shouldExist = true, string value = "")
		{
			Value = value;
			ShouldExist = shouldExist;
		}
		public override Parameter CreateParameter() { return new FileParameter { ShouldExist = ShouldExist, Value = Value }; }
		public override Type GetTypeForAttribute() { return typeof(string); }
	}

	public sealed class DirectoryAttribute : ParameterAttribute
	{
		public string Value { get; set; }
		public bool ShouldExist { get; set; }

		public DirectoryAttribute(bool shouldExist = true, string value = "")
		{
			Value = value;
			ShouldExist = shouldExist;
		}
		public override Parameter CreateParameter() { return new DirectoryParameter { ShouldExist = ShouldExist, Value = Value }; }
		public override Type GetTypeForAttribute() { return typeof(string); }
	}

	public sealed class IntegerAttribute : ParameterAttribute
	{
		public int Value { get; set; }
		public int Max { get; set; }
		public int Min { get; set; }

		public IntegerAttribute(int min = int.MinValue, int max = int.MaxValue, int value = 0 )
		{
			Value = value;
			Min = min;
			Max = max;
		}
		public override Parameter CreateParameter() { return new IntegerParameter { Min = Min, Max = Max, Value = Value }; }
		public override Type GetTypeForAttribute() { return typeof(int); }
	}

	public sealed class DoubleAttribute : ParameterAttribute
	{
		public double Value { get; set; }
		public double Max { get; set; }
		public double Min { get; set; }

		public DoubleAttribute(double min = double.MinValue, double max = double.MaxValue, double value = 0)
		{
			Value = value;
			Min = min;
			Max = max;
		}
		public override Parameter CreateParameter() { return new DoubleParameter { Min = Min, Max = Max, Value = Value }; }
		public override Type GetTypeForAttribute() { return typeof(double); }
	}

	public sealed class StringListAttribute : ParameterAttribute
	{
		public string []Values { get; set; }
		public bool CanBeEmpty { get; set; }

		public StringListAttribute(bool canBeEmpty = false, params string[] values)
		{
			CanBeEmpty = canBeEmpty;
			Values = values;
		}
		public override Parameter CreateParameter(){return new StringListParameter { CanBeEmpty = CanBeEmpty, Values = new CheckableDataCollection<CheckableData<string>>(Values.Select(x=>new CheckableData<string>{Value = x})) };}
		public override Type GetTypeForAttribute() { return typeof(IList<string>); }
	}

	public sealed class GuidListAttribute : ParameterAttribute
	{
		public Guid[] Values { get; set; }

		public GuidListAttribute(params string[] values)
		{
			Values = values.Select(Guid.Parse).ToArray();
		}
		public override Parameter CreateParameter() { return new GuidListParameter { Values = new CheckableDataCollection<CheckableData<Guid>>(Values.Select(x => new CheckableData<Guid> { Value = x })) }; }
		public override Type GetTypeForAttribute() { return typeof(IList<Guid>); }
	}

	public sealed class FileListAttribute : ParameterAttribute
	{
		public string[] Values { get; set; }
		public bool ShouldExist { get; set; }

		public FileListAttribute(bool shouldExist = true, params string[] values)
		{
			ShouldExist = shouldExist;
			Values = values;
		}
		public override Parameter CreateParameter() { return new FileListParameter { ShouldExist = ShouldExist, Values = new CheckableDataCollection<CheckableData<string>>(Values.Select(x => new CheckableData<string> { Value = x })) }; }
		public override Type GetTypeForAttribute() { return typeof(IList<string>); }
	}

	public sealed class DirectoryListAttribute : ParameterAttribute
	{
		public string[] Values { get; set; }
		public bool ShouldExist { get; set; }

		public DirectoryListAttribute(bool shouldExist = true, params string[] values)
		{
			ShouldExist = shouldExist;
			Values = values;
		}
		public override Parameter CreateParameter() { return new DirectoryListParameter { ShouldExist = ShouldExist, Values = new CheckableDataCollection<CheckableData<string>>(Values.Select(x => new CheckableData<string> { Value = x })) }; }
		public override Type GetTypeForAttribute() { return typeof(IList<string>); }
	}

	public sealed class IntegerListAttribute : ParameterAttribute
	{
		public int[] Values { get; set; }
		public int Max { get; set; }
		public int Min { get; set; }

		public IntegerListAttribute(int min = int.MinValue, int max = int.MaxValue, params int[] values)
		{
			Values = values;
			Min = min;
			Max = max;
		}
		public override Parameter CreateParameter() { return new IntegerListParameter { Min = Min, Max = Max, Values = new CheckableDataCollection<CheckableData<int>>(Values.Select(x => new CheckableData<int> { Value = x })) }; }
		public override Type GetTypeForAttribute() { return typeof(IList<int>); }
	}

	public sealed class DoubleListAttribute : ParameterAttribute
	{
		public double[] Values { get; set; }
		public double Max { get; set; }
		public double Min { get; set; }

		public DoubleListAttribute(double min=double.MinValue, double max=double.MaxValue, params double[] values)
		{
			Values = values;
			Min = min;
			Max = max;
		}
		public override Parameter CreateParameter() { return new DoubleListParameter { Min = Min, Max = Max, Values = new CheckableDataCollection<CheckableData<double>>(Values.Select(x => new CheckableData<double> { Value = x })) }; }
		public override Type GetTypeForAttribute() { return typeof(IList<double>); }
	}

	public sealed class StringDictionaryAttribute : ParameterAttribute
	{
		public IDictionary<string, string> Values { get; set; }
		public bool CanBeEmpty { get; set; }

		public StringDictionaryAttribute(bool canBeEmpty = false, params object[] values)
		{
			Values = values.ToDictionary<string>();
			CanBeEmpty = canBeEmpty;
		}
		public override Parameter CreateParameter() { return new StringDictionaryParameter { CanBeEmpty = CanBeEmpty, Values = Values.ToCollection() }; }
		public override Type GetTypeForAttribute() { return typeof(IDictionary<string,string>); }
	}

	public sealed class GuidDictionaryAttribute : ParameterAttribute
	{
		public IDictionary<string, Guid> Values { get; set; }
		public bool Autogenerate { get; set; }

		public GuidDictionaryAttribute(bool autogenerate = false, params object[] values)
		{
			Values = values.ToDictionary<string>().ToDictionary(x=>x.Key, x=>new Guid(x.Value));
			Autogenerate = autogenerate;
		}
		public override Parameter CreateParameter() { return new GuidDictionaryParameter { Autogenerate = Autogenerate, Values = Values.ToCollection() }; }
		public override Type GetTypeForAttribute() { return typeof(IDictionary<string, Guid>); }
	}

	public sealed class BoolDictionaryAttribute : ParameterAttribute
	{
		public IDictionary<string, bool> Values { get; set; }

		public BoolDictionaryAttribute(params object[] values)
		{
			Values = values.ToDictionary<bool>();
		}
		public override Parameter CreateParameter() { return new BoolDictionaryParameter { Values = Values.ToCollection() }; }
		public override Type GetTypeForAttribute() { return typeof(IDictionary<string, bool>); }
	}

	public sealed class FileDictionaryAttribute : ParameterAttribute
	{
		public IDictionary<string, string> Values { get; set; }
		public bool ShouldExist { get; set; }

		public FileDictionaryAttribute(bool shouldExist = true, params object[] values)
		{
			Values = values.ToDictionary<string>();
			ShouldExist = shouldExist;
		}
		public override Parameter CreateParameter() { return new FileDictionaryParameter { ShouldExist = ShouldExist, Values = Values.ToCollection() }; }
		public override Type GetTypeForAttribute() { return typeof(IDictionary<string, string>); }
	}

	public sealed class DirectoryDictionaryAttribute : ParameterAttribute
	{
		public IDictionary<string, string> Values { get; set; }
		public bool ShouldExist { get; set; }

		public DirectoryDictionaryAttribute(bool shouldExist = true, params object[] values)
		{
			Values = values.ToDictionary<string>();
			ShouldExist = shouldExist;
		}
		public override Parameter CreateParameter() { return new DirectoryDictionaryParameter { ShouldExist = ShouldExist, Values = Values.ToCollection() }; }
		public override Type GetTypeForAttribute() { return typeof(IDictionary<string, string>); }
	}

	public sealed class IntegerDictionaryAttribute : ParameterAttribute
	{
		public IDictionary<string, int> Values { get; set; }
		public int Min { get; set; }
		public int Max { get; set; }

		public IntegerDictionaryAttribute(int min=int.MinValue, int max=int.MaxValue, params object[] values)
		{
			Values = values.ToDictionary<int>();
			Min = min;
			Max = max;
		}
		public override Parameter CreateParameter() { return new IntegerDictionaryParameter { Min = Min, Max = Max, Values = Values.ToCollection() }; }
		public override Type GetTypeForAttribute() { return typeof(IDictionary<string, int>); }
	}

	public sealed class DoubleDictionaryAttribute : ParameterAttribute
	{
		public IDictionary<string, double> Values { get; set; }
		public double Min { get; set; }
		public double Max { get; set; }

		public DoubleDictionaryAttribute(double min = double.MinValue, double max = double.MaxValue, params object[] values)
		{
			Values = values.ToDictionary<double>();
			Min = min;
			Max = max;
		}
		public override Parameter CreateParameter() { return new DoubleDictionaryParameter { Min = Min, Max = Max, Values = Values.ToCollection() }; }
		public override Type GetTypeForAttribute() { return typeof(IDictionary<string, double>); }
	}
}
