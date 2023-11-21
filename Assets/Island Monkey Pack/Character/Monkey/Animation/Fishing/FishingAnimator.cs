using UnityEngine;

namespace IslandMonkey.AssetCollections
{
	public class FishingAnimator : MonoBehaviour
	{
		[SerializeField] int _animationCount = 3;

		Animator _animator;
		static readonly int _hashIndex = Animator.StringToHash("Index");
		int _index;

		void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
		}

		[ContextMenu("Play Next Animation")]
		public void PlayNextAnimation()
		{
			_index++;
			_animator.SetInteger(_hashIndex, _index);
			_index %= _animationCount;
		}
	}
}
