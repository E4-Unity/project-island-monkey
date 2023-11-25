using UnityEngine;
using UnityEngine.Animations;

namespace IslandMonkey.SMB
{
	public class SMB_MonkeyExitBuilding : StateMachineBehaviour
	{
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
		{
			base.OnStateExit(animator, stateInfo, layerIndex, controller);

			var monkey = animator.GetComponentInParent<BuildingMonkey>();
			if (monkey is null) return;

			monkey.ResetModelTransform();
			monkey.OnBuildingOutStateExit();
			monkey.ToggleBuildingAnimation();
		}
	}
}
