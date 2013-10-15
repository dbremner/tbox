using System;
using System.Collections.Generic;

namespace TeamManager.Code.ProjectManagers
{
	interface IProjectManager
	{
        LoggedTime[] GetTimeReport(DateTime dateFrom, DateTime dateTo, IEnumerable<string> emails);
	}
}
