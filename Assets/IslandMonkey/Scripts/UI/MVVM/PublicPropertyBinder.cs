using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace IslandMonkey.MVVM
{
	public abstract class PublicPropertyBinder : MonoBehaviour
	{
		INotifyPropertyChanged propertyNotifier;
		static BindingFlags BindingAttr = BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance;
		Dictionary<string, PropertyInfo> validProperties = new Dictionary<string, PropertyInfo>();

		bool isEventBound;

		static bool TryGetPublicPropertyInfo(object target, string propertyName, Type propertyType, out PropertyInfo propertyInfo)
		{
			// Property Info 추출
			propertyInfo = target.GetType().GetProperty(
				propertyName,
				BindingAttr,
				null,
				propertyType,
				Type.EmptyTypes,
				null
			);

			return propertyInfo is not null;
		}

		protected virtual void Awake(){}

		protected virtual void Start(){}

		protected virtual void OnEnable()
		{
			BindPropertyEvent();
		}

		protected virtual void OnDisable()
		{
			UnbindPropertyEvent();
		}

		protected void Init(INotifyPropertyChanged propertyNotifier)
		{
			if (propertyNotifier is null) return;

			this.propertyNotifier = propertyNotifier;
			RegisterProperties();
			BindPropertyEvent();
		}

		// 바인딩할 프로퍼티 등록
		protected abstract void RegisterProperties();

		// RegisterProperties에서 사용되는 메서드
		protected void RegisterProperty(string propertyName)
		{
			// 인터페이스 확인
			if (propertyNotifier is null) return;

			// 프로퍼티 확인
			PropertyInfo property = GetType().GetProperty(propertyName, BindingAttr);
			if (property is null) return;

			// Source 프로퍼티 확인
			if (TryGetPublicPropertyInfo(propertyNotifier, property.Name, property.PropertyType,
				    out var sourceProperty))
			{
				validProperties.Add(property.Name, property);
			}
			else
			{
				Debug.LogWarning(propertyNotifier + " : Property not found(" + property.Name + ")");
			}
		}

		void BindPropertyEvent()
		{
			// 유효성 검사
			if (isEventBound || propertyNotifier is null) return;
			isEventBound = true;

			propertyNotifier.PropertyChanged += OnPropertyChanged_Event;

			// 바인딩된 프로퍼티 값 초기화
			FetchAll();
		}

		void UnbindPropertyEvent()
		{
			// 유효성 검사
			if (!isEventBound || propertyNotifier is null) return;
			isEventBound = false;

			propertyNotifier.PropertyChanged -= OnPropertyChanged_Event;
		}

		void OnPropertyChanged_Event(object sender, PropertyChangedEventArgs e)
		{
			if (!ReferenceEquals(sender, propertyNotifier))
			{
				Debug.LogError("sender 와 source 가 일치하지 않습니다");
				return;
			}

			Fetch(e.PropertyName);
		}

		void FetchAll()
		{
			foreach (var validProperty in validProperties)
			{
				Fetch(validProperty.Key);
			}
		}

		void Fetch(string propertyName)
		{
			// 프로퍼티 존재 여부 확인
			if (!validProperties.TryGetValue(propertyName, out var property)) return;

			// 바인딩된 프로퍼티 업데이트
			if (TryGetPublicPropertyInfo(propertyNotifier, property.Name, property.PropertyType,
				    out var sourceProperty))
			{
				property.SetValue(this, sourceProperty.GetValue(propertyNotifier));
			}
		}
	}
}
