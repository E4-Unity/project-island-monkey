using System;
using UnityEngine;

namespace IslandMonkey
{
	[RequireComponent(typeof(MonkeySkinController))]
	public class Monkey : MonoBehaviour
	{
		/* 필드 */
		[SerializeField] MonkeyDefinition defaultDefinition;
		MonkeyDefinition currentDefinition;

		/* 컴포넌트 */
		MonkeySkinController skinController;

		protected virtual void Awake()
		{
			skinController = GetComponent<MonkeySkinController>();
		}

		protected virtual void OnEnable()
		{
			Init(defaultDefinition);
		}

		protected virtual void Start()
		{

		}

		public virtual void Init(MonkeyDefinition inDefinition)
		{
			if (!inDefinition) return;
			currentDefinition = inDefinition;

			ChangeSkin(inDefinition.Type);
		}

		public void ChangeSkin(MonkeyType monkeyType) => skinController.ChangeSkin(monkeyType);
	}
}
