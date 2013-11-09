using System;

namespace Searcher.Code
{
	class AvailabilityChecker
	{
		private readonly Action onNeedRebuildMenu;
		public bool CanSearch { get; private set; }
		public bool CanUnload { get; private set; }
		public bool CanRebuild { get; private set; }
		public bool IndexesLoaded { get; private set; }

		public AvailabilityChecker(Action onNeedRebuildMenu) 
		{
			this.onNeedRebuildMenu = onNeedRebuildMenu;
			CanSearch = true;
			CanUnload = false;
			CanRebuild = true;
			IndexesLoaded = false;
		}

		public void BeginOperation()
		{
			DisableControls();
		}

		private void DisableControls()
		{
			CanSearch = false;
			CanRebuild = false;
			CanUnload = false;
			onNeedRebuildMenu();
		}

		public void EndMakeIndexes(bool success)
		{
			CanSearch = CanUnload = success;
			CanRebuild = true;
			IndexesLoaded = success;
			onNeedRebuildMenu();
		}

		public void EndUnload()
		{
			CanSearch = true;
			CanUnload = false;
			CanRebuild = true;
			IndexesLoaded = false;
			onNeedRebuildMenu();
		}

		public void EndInitSearch(bool success)
		{
			CanSearch = CanUnload = success;
			CanRebuild = true;
			IndexesLoaded = success;
			onNeedRebuildMenu();
		}
	}
}
