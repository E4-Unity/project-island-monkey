using System;
using System.ComponentModel;
using UnityEngine;

namespace IslandMonkey.MVVM
{
	public abstract class ViewModel : MonoBehaviour
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
