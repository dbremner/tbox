using System;
using System.Windows;

namespace Mnk.Library.WpfControls.Components
{
	public static class DpHelper
	{
		public static DependencyProperty Create<TOwner, TValue>(string name, 
			Action<TOwner, TValue> setter)
            where TOwner : FrameworkElement
		{
			return DependencyProperty.Register(
					name,
					typeof(TValue),
					typeof(TOwner),
					new FrameworkPropertyMetadata(
						default(TValue),
						FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
						(s, e) => setter((TOwner)s, (TValue)e.NewValue)
				));
		}

		public static DependencyProperty Create<TOwner, TValue>(string name, TValue defaultValue,
			Action<TOwner, TValue> setter)
            where TOwner : FrameworkElement
		{
			return DependencyProperty.Register(
					name,
					typeof(TValue),
					typeof(TOwner),
					new FrameworkPropertyMetadata(
						defaultValue,
						FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
						(s, e) => setter((TOwner)s, (TValue)e.NewValue)
				));
		}

		public static DependencyProperty Create<TOwner, TValue>(string name)
		{
			return DependencyProperty.Register(
					name,
					typeof(TValue),
					typeof(TOwner),
					new FrameworkPropertyMetadata(default(TValue))
					);
		}

	}
}
