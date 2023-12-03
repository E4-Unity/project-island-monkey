using IslandMonkey.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandMonkey
{
	public class BuildingButtonSlot : MonoBehaviour
	{
		enum BuildingState
		{
			Init,
			Locked,
			Finished,
			WaitForBuilding
		}

		/* Component */
		// TODO 건설 시간 데이터 반영
		// 건설 버튼
		[Header("Building Button")]
		[SerializeField] Button buildButton;
		[SerializeField] TextMeshProUGUI costText;
		[SerializeField] Image buildTimeIcon;
		[SerializeField] TextMeshProUGUI buildTimeText;
		[SerializeField] GameObject descriptions;

		Color costTextColor;

		// 기타
		[Header("Finished")]
		[SerializeField] Image finishedStamp;

		[Header("Locked")]
		[SerializeField] Image lockedBanner;

		BuildingManager buildingManager;
		GoodsManager goodsManager;

		/* Field */
		[Header("Default Config")]
		[SerializeField] BuildingDefinition defaultDefinition;
		[SerializeField] bool useDefaultConfig;

		BuildingDefinition buildingDefinition;
		bool isInitialized;
		BuildingState state = BuildingState.Init;
		bool isEventBound;

		/* MonoBehaviour */
		void Awake()
		{
			// 컴포넌트 할당
			buildingManager = IslandGameManager.Instance.GetBuildingManager();
			goodsManager = GlobalGameManager.Instance.GetGoodsManager();

			// UI 초기화
			DeactivateAll();
			costTextColor = costText.color;

			// 자동 초기화
			if (useDefaultConfig && defaultDefinition)
			{
				InitComponent(defaultDefinition);
			}
		}

		void OnEnable()
		{
			ChangeState(CheckBuildingState());
		}

		void OnDestroy()
		{
			UnbindEvents();
		}

		void BindEvents()
		{
			if (isEventBound) return;
			isEventBound = true;

			OnGoodsUpdated_Event(GoodsType.Gold); // Fetch
			goodsManager.OnGoodsUpdated += OnGoodsUpdated_Event;
		}

		void UnbindEvents()
		{
			if (!isEventBound) return;
			isEventBound = false;

			goodsManager.OnGoodsUpdated -= OnGoodsUpdated_Event;
		}

		void OnGoodsUpdated_Event(GoodsType goodsType)
		{
			if (goodsType != buildingDefinition.BuildCostGoodsType) return;

			// 건설을 위한 금액이 부족한 경우
			if (goodsManager.CanSpend(buildingDefinition.BuildCostGoodsType, buildingDefinition.BuildCost.ToBigInteger()))
			{
				OnEnoughGoods();
			}
			else
			{
				OnNotEnoughGoods();
			}
		}

		void OnNotEnoughGoods()
		{
			buildButton.interactable = false;
			costText.color = new Color(Color.red.r, Color.red.g, Color.red.b, buildButton.colors.disabledColor.a);

			if (buildingDefinition.BuildingTime <= 0) return;

			buildTimeIcon.enabled = false;
			buildTimeText.enabled = false;
		}

		void OnEnoughGoods()
		{
			buildButton.interactable = true;
			costText.color = costTextColor;

			if (buildingDefinition.BuildingTime <= 0) return;

			buildTimeIcon.enabled = true;
			buildTimeText.enabled = true;
		}

		void ChangeState(BuildingState newState)
		{
			// 이미 해당 상태인 경우 무시
			if (state == newState) return;

			DeactivateAll();

			// 건물 상태에 따라 UI 업데이트
			state = newState;

			switch (state)
			{
				case BuildingState.Locked:
					ActivateLockedBanner();
					break;
				case BuildingState.Finished:
					ActivateFinishedStamp();
					break;
				case BuildingState.WaitForBuilding:
					costText.text = buildingDefinition.BuildCost;
					ActivateBuildButton();
					break;
				default:
					ActivateLockedBanner();
					break;
			}

			// 이벤트 바인딩
			if (state == BuildingState.WaitForBuilding)
			{
				BindEvents();
			}
			else
			{
				UnbindEvents();
			}
		}

		BuildingState CheckBuildingState()
		{
			// 초기화되지 않은 경우에는 Lock
			if (!isInitialized)
			{
				return BuildingState.Locked;
			}

			// 건물 건설이 완료된 경우
			if (buildingManager.IsBuildingExist(buildingDefinition))
			{
				return BuildingState.Finished;
			}

			// TODO 건물 건설 조건을 만족하지 못한 경우
			if (false)
			{
				return BuildingState.Locked;
			}

			return BuildingState.WaitForBuilding;
		}

		void DeactivateAll()
		{
			buildButton.gameObject.SetActive(false);
			finishedStamp.gameObject.SetActive(false);
			lockedBanner.gameObject.SetActive(false);
			descriptions.SetActive(false);
		}

		void ActivateFinishedStamp()
		{
			descriptions.SetActive(true);
			finishedStamp.gameObject.SetActive(true);
		}

		void ActivateLockedBanner()
		{
			lockedBanner.gameObject.SetActive(true);
		}

		void ActivateBuildButton()
		{
			descriptions.SetActive(true);
			buildButton.gameObject.SetActive(true);
		}

		/* API */
		public void InitComponent(BuildingDefinition definition)
		{
			// 필드 초기화
			if (isInitialized || definition is null) return;
			isInitialized = true;
			buildingDefinition = definition;

			// 초기화 시퀀스
			ChangeState(CheckBuildingState());
		}
	}
}
