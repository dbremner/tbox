using System.Collections.Generic;

namespace TeamManager.Code.ProjectManagers
{
	interface IProjectManager
	{
        LoggedTime[] GetTimeReport(string dateFrom, string dateTo, IEnumerable<string> emails);
	}
}
