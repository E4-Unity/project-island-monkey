using UnityEngine;
using UnityEngine.Animations;

namespace IslandMonkey.SMB
{
	public class AutoResetSucceed : StateMachineBehaviour
	{
		static readonly int _hashSucceed = Animator.StringToHash("Succeed");

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
			AnimatorControllerPlayable controller)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex, controller);

			animator.SetBool(_hashSucceed, false);
		}
	}
}
