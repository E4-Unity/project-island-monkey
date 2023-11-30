using UnityEngine;
using UnityEngine.Animations;

namespace IslandMonkey
{
	public class SMB_DestroyOnEnd : StateMachineBehaviour
	{
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
		{
			base.OnStateExit(animator, stateInfo, layerIndex, controller);

			Destroy(animator.GetComponentInParent<EffectGameObject>().gameObject);
		}
	}
}
