using Common.UI.Model;

namespace TeamManager.Code.Settings
{
	public class Person : CheckableData
	{
        public int ReportType { get; set; }

	    public Person()
	    {
	        ReportType = (int)TimeReportType.Personal;
	    }

		public override object Clone()
		{
			return new Person
			{
				Key = Key,
				IsChecked = IsChecked,
                ReportType = ReportType
			};
		}
	}
}
