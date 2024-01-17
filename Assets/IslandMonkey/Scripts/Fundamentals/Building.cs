using IslandMonkeyPack;
using UnityEngine;

namespace IslandMonkey
{
	/// <summary>
	/// 건물 클래스
	/// </summary>
	public class Building : MonoBehaviour
	{
		/* 설정 */
		[Header("기본 설정")]
		[SerializeField] bool m_UseDefaultConfig;
		[Space]
		[SerializeField] BuildingData m_DefaultBuildingData;
		[SerializeField] BuildingModel m_DefaultBuildingModelPrefab;

		/* 필드 */
		bool m_IsInitialized;

		// 컴포넌트
		GoodsFactory m_GoodsFactory;
		BuildingManager m_BuildingManager;

		// 의존성 주입
		BuildingModel m_BuildingModel;
		BuildingData m_BuildingData;
		public BuildingData GetBuildingData() => m_BuildingData;
		public BuildingModel GetBuildingModel() => m_BuildingModel;

		/* 프로퍼티 */
		BuildingAnimator GetBuildingAnimator() => m_BuildingModel.GetBuildingAnimator();
		BuildingMonkey.IBuilding GetBuildingPayload() => m_BuildingModel.GetBuildingPayload();

		/* MonoBehaviour */
		void Awake()
		{
			// 컴포넌트 할당
			m_GoodsFactory = GetComponent<GoodsFactory>();
			m_BuildingModel = GetComponentInChildren<BuildingModel>();
			m_BuildingManager = IslandGameManager.Instance.GetBuildingManager();
		}

		void Start()
		{
			// 기본 설정 사용
			if (m_UseDefaultConfig)
			{
				InitComponent(m_DefaultBuildingData, m_DefaultBuildingModelPrefab);
			}
		}

		/* API */
		/// <summary>
		/// 단 한 번만 호출 가능한 초기화 함수
		/// </summary>
		/// <param name="buildingData">건물 설정 데이터</param>
		/// <param name="buildingModelPrefab">BuildingModel 컴포넌트가 부착된 프리팹</param>
		public void InitComponent(BuildingData buildingData, BuildingModel buildingModelPrefab)
		{
			// 유효성 검사
			if (buildingData is null) return;

			// 중복 호출 방지
			if (m_IsInitialized) return;
			m_IsInitialized = true;

			// 의존성 주입
			m_BuildingData = buildingData;
			m_BuildingModel = buildingModelPrefab;

			// 건물 종류에 따른 추가 설정
			switch (m_BuildingData.Definition.BuildingType)
			{
				case BuildingType.Voyage:

					// Building Payload 초기화
					GetBuildingPayload().Init(m_BuildingData);

					// Goods Factory 초기화
					m_GoodsFactory.Init(m_BuildingData.Definition.GetGoodsFactoryConfig());

					break;
				case BuildingType.Functional:

					// Building Payload 초기화
					GetBuildingPayload().Init(m_BuildingData);

					// Goods Factory 초기화
					m_GoodsFactory.Init(m_BuildingData.Definition.GetGoodsFactoryConfig());

					break;
				case BuildingType.Special:

					// 애니메이션 즉시 활성화
					GetBuildingAnimator().Activate();

					break;
			}
		}
	}
}
