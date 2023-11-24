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
#if UNITY_EDITOR
				Debug.LogWarning("이미 초기화된 상태입니다");
#endif
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
		static bool TryGetProperty<T>(object target, string propertyName, out PropertyInfo propertyInfo)
		{
			// Property Info 추출
			propertyInfo = target.GetType().GetProperty(
				propertyName,
				BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
				null,
				typeof(T),
				Type.EmptyTypes,
				null
			);

			return propertyInfo is not null;
		}

		protected void Fetch<T>(object sender, string propertyName) where T : new()
		{
			if (!TryGetProperty<T>(sender, propertyName, out var modelPropertyInfo))
			{
#if UNITY_EDITOR
				Debug.LogWarning(sender + " : Property not found(" + propertyName + ")");
#endif
				return;
			}

			if (!TryGetProperty<T>(this, propertyName, out var viewModelPropertyInfo))
			{
#if UNITY_EDITOR
				Debug.LogWarning(name + " : Property not found(" + propertyName + ")");
#endif
				return;
			}

			viewModelPropertyInfo.SetValue(this, modelPropertyInfo.GetValue(sender));
		}
	}
}
