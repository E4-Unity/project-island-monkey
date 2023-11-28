using UnityEngine;

namespace IslandMonkey.AssetCollections
{
	public class BuildingAnimator : MonoBehaviour
	{
		enum AnimationState
		{
			Deactivated,
			Activated
		}

		static readonly int HashToggle = Animator.StringToHash("Toggle");

		[SerializeField] bool autoActivate;

		Animator animator;
		AnimationState state;

		void Awake()
		{
			animator = GetComponentInChildren<Animator>();
		}

		void Start()
		{
			if(autoActivate)
				Activate();
		}

		public void Activate()
		{
			if (state == AnimationState.Activated) return;

			state = AnimationState.Activated;
			ToggleAnimation();
		}

		public void Deactivate()
		{
			if (state == AnimationState.Deactivated) return;

			state = AnimationState.Deactivated;
			ToggleAnimation();
		}

		[ContextMenu("Toggle Animation")]
		public void ToggleAnimation()
		{
			animator.SetTrigger(HashToggle);
		}
	}
}
