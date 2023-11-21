using UnityEngine;

namespace IslandMonkey.AssetCollections
{
	public class BuildingAnimator : MonoBehaviour
	{
		Animator _animator;
		static readonly int _hashToggle = Animator.StringToHash("Toggle");

		void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
		}

		[ContextMenu("Toggle Animation")]
		public void ToggleAnimation()
		{
			_animator.SetTrigger(_hashToggle);
		}
	}
}
