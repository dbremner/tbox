using System;
using System.Resources;
using System.Windows.Markup;
using Mnk.Library.Localization.WPFControls;

namespace Mnk.Library.WpfControls.Localization
{
	[MarkupExtensionReturnType(typeof(string))]
	public abstract class TranslateExtension : MarkupExtension
	{
		private readonly string key;
		//private DependencyObject targetObject;
		//private DependencyProperty targetProperty;

		protected TranslateExtension(string key, ResourceManager resm)
		{
			this.key = key;
		    Manager = resm;
		    //Translator.CultureChanged += Translator_CultureChanged;
		}

		protected ResourceManager Manager { get; private set; }
		/*
		void Translator_CultureChanged(object sender, EventArgs e)
		{
			if (targetObject != null && targetProperty != null)
			{
				targetObject.SetValue(targetProperty,
					  Manager.GetObject(key));
			}
		}
		*/
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			//var targetHelper = (IProvideValueTarget)serviceProvider.
			//	   GetService(typeof(IProvideValueTarget));
			//targetObject = targetHelper.TargetObject as DependencyObject;
			//targetProperty = targetHelper.TargetProperty as DependencyProperty;
			return Manager.GetObject(key)??("{" + key + "}");
		}
	}

	public class TrExtension : TranslateExtension
	{
        public TrExtension(string key) : base(key, WPFControlsLang.ResourceManager) { }
	}

}
