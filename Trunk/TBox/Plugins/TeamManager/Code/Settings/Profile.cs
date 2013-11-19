using System;
using Common.Tools;
using Common.UI.Model;
using Common.UI.ModelsContainers;
using ScriptEngine;

namespace TeamManager.Code.Settings
{
	[Serializable]
	public sealed class Profile : Data
	{
        public Email Email { get; set; }
        public Report Report { get; set; }
        public CheckableDataCollection<Person> Persons { get; set; }
        public CheckableDataCollection<SingleFileOperation> Operations { get; set; }

		public Profile()
		{
            Email = new Email();
            Report = new Report();
            Persons =new CheckableDataCollection<Person>();
            Operations = new CheckableDataCollection<SingleFileOperation>();
		}

		public override object Clone()
		{
			return new Profile
			{
                Email = Email.Clone(),
                Report = Report.Clone(),
                Persons = Persons.Clone(),
				Operations = Operations.Clone(),
			};
		}
	}
}
