using IslandMonkey;
using UnityEngine;

namespace IslandMonkey
{
	public class ShowcaseMonkey : Monkey
	{
		static readonly int HashToggle = Animator.StringToHash("Toggle");
		Animator animator;

		protected override void Awake()
		{
			base.Awake();
			animator = GetComponentInChildren<Animator>();
		}

		[ContextMenu("Toggle Animation")]
		public void ToggleAnimation()
		{
			if (animator is null)
			{
#if UNITY_EDITOR
				Debug.LogError("애니메이터 컴포넌트가 부착되어있는지 확인해주세요");
#endif
				return;
			}

			animator.SetTrigger(HashToggle);
		}
	}
}
