using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace IslandMonkey.MVVM
{
	public abstract class ViewModel : MonoBehaviour
	{
		[SerializeField] Model model;

		protected virtual void Awake()
		{
			model.PropertyChanged += OnPropertyChanged_Event;
		}

		protected abstract void OnPropertyChanged_Event(object sender, PropertyChangedEventArgs e);
		protected bool GetProperty<T>(object sender, PropertyChangedEventArgs e, out T value) where T : new()
		{
			value = new T();

			// Property Info 추출
			var propertyInfo = sender.GetType().GetProperty(
				e.PropertyName,
				BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
				null,
				typeof(T),
				Type.EmptyTypes,
				null
			);

			if (propertyInfo is null) return false;

			value = (T)propertyInfo.GetValue(sender);
			return true;
		}
	}
}
