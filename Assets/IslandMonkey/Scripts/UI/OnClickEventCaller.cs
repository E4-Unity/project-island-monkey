using System;
using UnityEngine;
using UnityEngine.Events;

namespace MyNamespace
{
	public class OnClickEventCaller : MonoBehaviour
	{
		[SerializeField] UnityEvent OnClickEvent;

		void OnMouseDown()
		{
			OnClickEvent?.Invoke();
		}
	}
}
