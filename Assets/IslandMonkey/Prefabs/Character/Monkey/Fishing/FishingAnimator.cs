using System.Collections;
using UnityEngine;

namespace IslandMonkey
{
	public class FishingAnimator : MonoBehaviour
	{
		[SerializeField] bool _succeed;
		public bool succeed
		{
			get
			{
				return _succeed;
			}
			set
			{
				_succeed = value;
			}
		}

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

		public void PlayAndShowPopup()
		{
			_animator.SetTrigger(_hashNext);
			_animator.SetBool(_hashSucceed, _succeed);

			StartCoroutine(WaitforAnimation());
		}

		IEnumerator WaitforAnimation()
		{
			while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("AS_Monkey_Fishing_Stand"))
			{
				yield return null;
			}
			VoyageUIManager.Show<ClamPopup>();
		}
	}
}
