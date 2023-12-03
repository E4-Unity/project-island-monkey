using IslandMonkey.AssetCollections;
using UnityEngine;

namespace IslandMonkey
{
	/// <summary>
	/// 사령탑 역할
	/// </summary>
	public class Building : MonoBehaviour
	{
		/* Component */
		GoodsFactory goodsFactory;
		BuildingAnimator animator;
		HexagonalPlacementManager hexagonalPlacementManager; // TODO BuildingManager 로 이전

		/* Field */
		[Header("Default Config")]
		[SerializeField] BuildingData defaultData;
		[SerializeField] bool useDefaultConfig;

		BuildingData data;
		bool isInitialized;

		void Awake()
		{
			goodsFactory = GetComponent<GoodsFactory>();
			hexagonalPlacementManager = IslandGameManager.Instance.GetPlacementManager();
		}

		public void InitComponent(BuildingData newData)
		{
			// 필드 초기화
			if (isInitialized || newData is null) return;
			isInitialized = true;
			data = newData;

			/* 초기화 시퀀스 */
			// Building 스폰
			GameObject buildingInstance = Instantiate(data.Definition.BuildingPrefab, transform);
			animator = buildingInstance.GetComponent<BuildingAnimator>();

			// TODO BuildingDefinition 이나 별도의 Config 로 이동, 특별 시설에는 필요없음
			// Building 초기화
			BuildingMonkey.IBuilding building = buildingInstance.GetComponent<BuildingMonkey.IBuilding>();
			if(building is not null)
				building.Init(data);

			// 건물 종류에 따른 추가 설정
			switch (data.Definition.BuildingType)
			{
				case BuildingType.Special:

					// 애니메이션 즉시 활성화
					animator.Activate();

					// GoodsFactory 컴포넌트 제거
					Destroy(goodsFactory);
					goodsFactory = null;

					break;
				case BuildingType.Functional:

					// 기능 시설 등록
					hexagonalPlacementManager.FunctionalBuildings.Add(building);

					break;
				default:

					break;
			}

			// GoodsFactory 초기화
			if (goodsFactory is not null)
			{
				goodsFactory.Init(data.Definition.GetGoodsFactoryConfig());
			}
		}
	}
}
