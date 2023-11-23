using UnityEngine;

public class BuildingMonkeyAnimator : MonoBehaviour
{
	public enum AnimationState
	{
		Locomotion,
		Building
	}

	static readonly int HashWalk = Animator.StringToHash("Walk");
	static readonly int HashToggle = Animator.StringToHash("Toggle");
	Animator animator;
	float speedBuffer;
	AnimationState state;

	public AnimationState State => state;

	public void DisableRootMotion(bool disable)
	{
		animator.applyRootMotion = !disable;
	}

	void Awake()
	{
		animator = GetComponentInChildren<Animator>();
	}

	public void PlayBuildingIn()
	{
		if (animator is null || state != AnimationState.Locomotion) return;

		DisableRootMotion(false);
		animator.SetTrigger(HashToggle);
		state = AnimationState.Building;
	}

	public void PlayBuildingOut()
	{
		if (animator is null || state != AnimationState.Building) return;

		animator.SetTrigger(HashToggle);
		state = AnimationState.Locomotion;
	}

	public void SetAnimatorController(AnimatorOverrideController newAnimatorController)
	{
		if (animator is null) return;

		animator.runtimeAnimatorController = newAnimatorController;
	}
	public void Walk() => animator.SetFloat(HashWalk, 1);
	public void Stop() => animator.SetFloat(HashWalk, 0);

	public void SyncAnimationWithSpeed(float speed, float speedFactor)
	{
		if (animator is null) return;

		speedBuffer = animator.speed;
		animator.speed = speedBuffer * speedFactor * speed;
	}

	public void ResetAnimationSpeed()
	{
		if (animator is null) return;

		animator.speed = speedBuffer;
	}
}
