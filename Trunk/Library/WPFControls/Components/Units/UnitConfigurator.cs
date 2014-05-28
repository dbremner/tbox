using System;
using System.Collections;
using Mnk.Library.WpfControls.Tools;

namespace Mnk.Library.WpfControls.Components.Units
{
	public sealed class UnitConfigurator
	{
		private bool isConfigured = false;
		private string title;
		private ICollection collection;
		private UnitType unitType;
		private readonly IUnit owner;
		private readonly Action configureAction;

		public UnitConfigurator(IUnit owner, Action configureAction)
		{
			this.owner = owner;
			this.configureAction = configureAction;
		}

		public void SetTitle(string value)
		{
			title = value; 
			Configure();
		}

		public void SetCollection(ICollection value)
		{
			collection = value; 
			Configure();
		}

		public void SetUnitType(UnitType value)
		{
			unitType = value; 
			Configure();
		}

		private void Configure()
		{
			if (string.IsNullOrEmpty(title) || collection == null || unitType == default(UnitType))
			{
				if (isConfigured)
				{
					owner.Unconfigure();
					isConfigured = false;
				}
				return;
			}
			dynamic c = collection;
			switch (unitType)
			{
				case UnitType.Text:
					SelectorExtensions.ConfigureInputText(owner, title, c);
					break;
				case UnitType.ComboBox:
					SelectorExtensions.ConfigureInputTextList(owner, title, c);
					break;
				case UnitType.DropDownList:
					SelectorExtensions.ConfigureInputSelect(owner, title, c);
					break;
				case UnitType.FilePath:
					SelectorExtensions.ConfigureInputFilePath(owner, title, c);
					break;
				case UnitType.FolderPath:
					SelectorExtensions.ConfigureInputFolderPath(owner, title, c);
					break;
                case UnitType.Date:
                    SelectorExtensions.ConfigureInputDate(owner, title, c);
                    break;
				default:
					throw new ArgumentException("Unknown type: " + unitType);
			}
			configureAction();
			isConfigured = true;
		}

	}
}
