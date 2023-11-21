using UnityEngine;

namespace IslandMonkey
{
	public class FishingAnimator : MonoBehaviour
	{
		[SerializeField] bool _succeed;
		Animator _animator;
		static readonly int _hashNext = Animator.StringToHash("Next");
		static readonly int _hashSucceed = Animator.StringToHash("Succeed");

		void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
		}

		[ContextMenu("Play Next Animation")]
		public void PlayNextAnimation()
		{
			_animator.SetTrigger(_hashNext);
			_animator.SetBool(_hashSucceed, _succeed);
		}
	}
}
