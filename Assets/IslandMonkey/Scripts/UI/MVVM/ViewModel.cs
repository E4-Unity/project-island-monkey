using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace IslandMonkey.MVVM
{
	public abstract class ViewModel : MonoBehaviour
	{
		Model model;
		bool isInitialized;

		protected virtual void OnDestroy()
		{
			if (!model) return;

			model.PropertyChanged -= OnPropertyChanged_Event;
		}

		public void Init(Model newModel)
		{
			if (isInitialized)
			{
				Debug.LogWarning("이미 초기화된 상태입니다");
				return;
			}

			if (!newModel) return;
			isInitialized = true;

			// TODO 초기값 읽어오기
			// 이벤트 바인딩
			model = newModel;
			if (model) model.PropertyChanged += OnPropertyChanged_Event;
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
