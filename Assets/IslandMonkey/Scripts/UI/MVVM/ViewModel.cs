using System;
using System.ComponentModel;

namespace IslandMonkey.MVVM
{
	public abstract class ViewModel : PublicPropertyBinder
	{
		INotifyPropertyChanged propertyNotifier;

		public INotifyPropertyChanged PropertyNotifier
		{
			get => propertyNotifier;
			protected set
			{
				propertyNotifier = value;
				OnInitialized?.Invoke();
			}
		}
		public event Action OnInitialized;
	}
}
