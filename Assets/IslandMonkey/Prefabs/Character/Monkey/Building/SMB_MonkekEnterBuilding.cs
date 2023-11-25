using UnityEngine;
using UnityEngine.Animations;

namespace IslandMonkey.SMB
{
	public class SMB_MonkeyEnterBuilding : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
			AnimatorControllerPlayable controller)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex, controller);

			var monkey = animator.GetComponentInParent<BuildingMonkey>();
			if (monkey is null) return;

			monkey.OnBuildingEnter();
			monkey.ToggleBuildingAnimation();
		}
	}
}
