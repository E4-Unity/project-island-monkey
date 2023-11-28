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
			void Activate(BuildingMonkey monkey);
			void Deactivate();
			bool IsActivated { get; }
			bool IsBusy { get; set; }
			void Init(BuildingData buildingData);
		}

		public enum BuildingMonkeyState
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
		[SerializeField] GameObject defaultBuilding;

		IBuilding workBuilding;
		IBuilding currentBuilding;
		IBuilding targetBuilding;
		Coroutine checkRemainingDistanceCoroutine;
		BuildingMonkeyState state = BuildingMonkeyState.Init;

		// TODO 임시
		public BuildingMonkeyState State => state;

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

		protected override void Start()
		{
			base.Start();

			if (defaultBuilding)
			{
				SetBuilding(defaultBuilding.GetComponent<IBuilding>());
			}
		}

#if UNITY_EDITOR
		/* TODO 테스트용 나중에 삭제 시작 */
		[Header("Test")]
		[SerializeField] GameObject testWorkBuilding;

		[ContextMenu("Test Set Building")]
		void TestSetBuilding()
		{
			SetBuilding(testWorkBuilding.GetComponent<IBuilding>());
		}

		[SerializeField] GameObject testRestBuilding;
		IBuilding restBuildingInterface;
		bool isValid = true;

		[ContextMenu("Test Go To Rest")]
		void TestGoToRest()
		{
			if (!isValid) return;

			if (restBuildingInterface is null)
			{
				restBuildingInterface = testRestBuilding.GetComponent<IBuilding>();
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
			// 유효성 검사
			if (inBuilding is null) return;

			// 이미 초기화된 상태
			if (workBuilding is not null) return;

			workBuilding = inBuilding;
			targetBuilding = workBuilding;
			currentBuilding = workBuilding;

			// Entrance 위치로 순간이동
			var thisTransform = transform;
			thisTransform.position = workBuilding.Entrance.position;
			thisTransform.rotation = workBuilding.Entrance.rotation;

			// 건물 관련 동작 실행
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

			currentBuilding = targetBuilding;
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

			currentBuilding = targetBuilding;
			targetBuilding = workBuilding;
			state = BuildingMonkeyState.Working;

			if(buildingMonkeyAnimator.State == BuildingMonkeyAnimator.AnimationState.Building)
				buildingMonkeyAnimator.PlayBuildingOut();
			else
				EnterBuilding(workBuilding);
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

		public void DeactivateCurrentBuilding()
		{
			if (currentBuilding is null) return;

			currentBuilding.Deactivate();
		}

		public void ActivateTargetBuilding()
		{
			if (targetBuilding is null) return;

			targetBuilding.Activate(this);
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
