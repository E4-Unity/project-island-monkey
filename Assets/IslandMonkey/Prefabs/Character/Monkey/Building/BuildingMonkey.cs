using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace IslandMonkey
{
	[RequireComponent(typeof(NavMeshAgent), typeof(BuildingMonkeyAnimator), typeof(EquipmentComponent))]
	public class BuildingMonkey : Monkey
	{
		public interface IBuilding
		{
			Transform Entrance { get; }
			AnimatorOverrideController AnimatorController { get; }

			EquipmentComponent.EquipmentSet Equipments { get; }
		}

		enum BuildingMonkeyState
		{
			Init,
			Working,
			Resting
		}

		/* 컴포넌트 */
		[SerializeField] Transform model;
		NavMeshAgent agent;
		BuildingMonkeyAnimator buildingMonkeyAnimator;
		EquipmentComponent equipmentComponent;

		/* 필드 */
		[SerializeField] float walkSpeedFactor = 2; // 애니메이션 속도와 실제 속도 싱크에 사용
		[SerializeField] bool stayOnly;

		IBuilding building;
		IBuilding targetBuilding;
		Coroutine checkRemainingDistanceCoroutine;
		BuildingMonkeyState state = BuildingMonkeyState.Init;

		/* 이벤트 */
		public event Action OnArrived;

		protected override void Awake()
		{
			base.Awake();

			agent = GetComponent<NavMeshAgent>();
			buildingMonkeyAnimator = GetComponent<BuildingMonkeyAnimator>();
			equipmentComponent = GetComponent<EquipmentComponent>();

			if (buildingMonkeyAnimator is not null)
			{
				OnArrived += () =>
				{
					buildingMonkeyAnimator.Stop();
					buildingMonkeyAnimator.ResetAnimationSpeed();
					buildingMonkeyAnimator.PlayBuildingIn();

					// TODO 다른 방법?
					var t = transform;
					t.position = targetBuilding.Entrance.position;
					t.rotation = targetBuilding.Entrance.rotation;
				};
			}
		}

#if UNITY_EDITOR
		/* TODO 테스트용 나중에 삭제 시작 */
		[Header("Test")]
		[SerializeField] GameObject workBuilding;

		[ContextMenu("Test Set Building")]
		void TestSetBuilding()
		{
			SetBuilding(workBuilding.GetComponent<IBuilding>());
		}

		[SerializeField] GameObject restBuilding;
		IBuilding restBuildingInterface;
		bool isValid = true;

		[ContextMenu("Test Go To Rest")]
		void TestGoToRest()
		{
			if (!isValid) return;

			if (restBuildingInterface is null)
			{
				restBuildingInterface = restBuilding.GetComponent<IBuilding>();
				if (restBuildingInterface is null) return;
			}

			GoToRest(restBuildingInterface);
		}
		/* TODO 테스트용 나중에 삭제 끝 */
#endif

		/* API */
		// 초기화 시 한 번만 호출 가능
		public void SetBuilding(IBuilding inBuilding)
		{
			if (building is not null) return;
			building = inBuilding;
			BackToWork();
		}

		public void GoToRest(IBuilding inBuilding)
		{
			if (stayOnly)
			{
#if UNITY_EDITOR
				Debug.LogWarning("휴식 시설을 이용할 수 없는 숭숭이입니다.");
#endif
				return;
			}

			if (state == BuildingMonkeyState.Resting)
			{
#if UNITY_EDITOR
				Debug.LogWarning("이미 쉬고 있는 중입니다.");
#endif
				return;
			}

			targetBuilding = inBuilding;
			state = BuildingMonkeyState.Resting;
			buildingMonkeyAnimator.PlayBuildingOut();
		}

		[ContextMenu("Back To Work")]
		public void BackToWork()
		{
			if (state == BuildingMonkeyState.Working)
			{
#if UNITY_EDITOR
				Debug.LogWarning("이미 일하고 있는 중입니다.");
#endif
				return;
			}

			targetBuilding = building;
			state = BuildingMonkeyState.Working;

			if(buildingMonkeyAnimator.State == BuildingMonkeyAnimator.AnimationState.Building)
				buildingMonkeyAnimator.PlayBuildingOut();
			else
				EnterBuilding(building);
		}

		/* StateMachineBehaviour 전용 */
		// 루트 모션으로 인한 Offset 초기화
		public void ResetModelTransform()
		{
			buildingMonkeyAnimator.DisableRootMotion(true);
			model.localPosition = Vector3.zero;
			model.localRotation = Quaternion.identity;
		}

		public void OnBuildingOutStateExit()
		{
			EnterBuilding(targetBuilding);
			equipmentComponent.UnEquip();
		}

		public void OnBuildingEnter()
		{
			equipmentComponent.Equip(targetBuilding.Equipments);
		}

		/* Method */
		void EnterBuilding(IBuilding inBuilding)
		{
			buildingMonkeyAnimator.SetAnimatorController(inBuilding.AnimatorController);
			MoveToGoal(inBuilding.Entrance);
		}

		void MoveToGoal(Transform goal)
		{
			if (stayOnly || agent is null) return;

			agent.SetDestination(goal.position);

			if (buildingMonkeyAnimator)
			{
				buildingMonkeyAnimator.Walk();
				buildingMonkeyAnimator.SyncAnimationWithSpeed(agent.speed, walkSpeedFactor);
			}

			checkRemainingDistanceCoroutine ??= StartCoroutine(CheckRemainingDistance());
		}

		IEnumerator CheckRemainingDistance()
		{
			// SetDestination 를 호출한 직후에는 agent.remainingDistance 값이 아직 변하지 않은 상태이다.
			yield return null;

			while (agent.remainingDistance > agent.stoppingDistance)
			{
				yield return null;
			}

			OnArrived?.Invoke();
			checkRemainingDistanceCoroutine = null;
		}
	}
}