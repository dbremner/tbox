using System;
using System.Collections;
using WPFControls.Tools;

namespace WPFControls.Components.Units.Properties
{
	public sealed class UnitConfigurator
	{
		private bool isConfigured = false;
		private string title;
		private ICollection collection;
		private UnitTypes unitType;
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

		public void SetUnitType(UnitTypes value)
		{
			unitType = value; 
			Configure();
		}

		private void Configure()
		{
			if (string.IsNullOrEmpty(title) || collection == null || unitType == default(UnitTypes))
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
				case UnitTypes.Text:
					SelectorExtensions.ConfigureInputText(owner, title, c);
					break;
				case UnitTypes.ComboBox:
					SelectorExtensions.ConfigureInputTextList(owner, title, c);
					break;
				case UnitTypes.DropDownList:
					SelectorExtensions.ConfigureInputSelect(owner, title, c);
					break;
				case UnitTypes.FilePath:
					SelectorExtensions.ConfigureInputFilePath(owner, title, c);
					break;
				case UnitTypes.FolderPath:
					SelectorExtensions.ConfigureInputFolderPath(owner, title, c);
					break;
                case UnitTypes.Date:
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
