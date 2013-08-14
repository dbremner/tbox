namespace TBox.Code.Objects
{
	class PluginName
	{
		public string Name { get; private set; }
		public string Description { get; private set; }

		public PluginName(string name, string description)
		{
			Name = name;
			Description = description;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
