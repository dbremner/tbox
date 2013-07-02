using System;
using System.Data.Objects;
using System.Linq;
using Common.Base;
using Common.Base.Log;

namespace MarketService.Service
{
	class BugService
	{
		private static readonly ILog Log = LogManager.GetLogger<BugService>();
		private readonly MarketEntities marketEntities;
		private readonly ObjectSet<Bug> bugs;
		public BugService(MarketEntities marketEntities)
		{
			this.marketEntities = marketEntities;
			bugs = this.marketEntities.Bugs;
		}
		public MarketInterfaces.Bug[] GetList(long uid, int offset, int count)
		{
			var ret = new MarketInterfaces.Bug[0];
			Shared.Do("Bug GetList", () =>
			{
				ret = bugs.Where(bug => Filter(bug, uid)).Skip(offset).Take(count).
					Select(bug =>
						new MarketInterfaces.Bug
						{
							Description = bug.Description,
							UID = bug.UID,
							PluginUID = bug.PluginUID,
							Date = bug.Date,
						}
					).ToArray();
			}, Log);
			return ret;
		}

		private static bool Filter(Bug bug, long uid)
		{
			return bug.UID == uid;
		}

		public int GetListCount(long uid)
		{
			return bugs.Count(bug => Filter(bug, uid));
		}

		public void Send(MarketInterfaces.Bug bug)
		{
			Shared.Do("Bug send", () =>
			{
				bugs.AddObject(new Bug
				{
					Description = bug.Description,
					PluginUID = bug.PluginUID,
					Date = DateTime.Now,
				});
				marketEntities.SaveChanges();
			}, Log);
		}
	}
}
