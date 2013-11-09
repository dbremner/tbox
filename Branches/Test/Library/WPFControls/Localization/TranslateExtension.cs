using System;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using LibsLocalization.WPFControls;

namespace WPFControls.Localization
{
	[MarkupExtensionReturnType(typeof(string))]
	public abstract class TranslateExtension : MarkupExtension
	{
		private readonly string key;
		//private DependencyObject targetObject;
		//private DependencyProperty targetProperty;

		protected TranslateExtension(string key)
		{
			this.key = key;
			//Translator.CultureChanged += Translator_CultureChanged;
		}

		protected abstract ResourceManager Manager { get; }
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
		public TrExtension(string key): base(key){}

		protected override ResourceManager Manager
		{
			get { return WPFControlsLang.ResourceManager; }
		}
	}

}
