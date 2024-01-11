using UnityEngine;

namespace IslandMonkey
{
	// TODO GoodsFactory 리팩토링 후 팩토리 메서드 패턴으로 변경
	/// <summary>
	/// 건물 심플 팩토리
	/// </summary>
	public class BuildingFactory
	{
		/* 필드 */
		Transform m_Root;
		Building m_Prototype;

		/* 생성자 */
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="root">생성될 건물의 부모 Transform</param>
		/// <param name="buildingPrefab">Building 컴포넌트가 부착된 프리팹</param>
		public BuildingFactory(Transform root, Building buildingPrefab)
		{
			// 필드 초기화
			m_Root = root;

			// 프로토타입 생성
			CreatePrototype(buildingPrefab);
		}

		/* API */
		/// <summary>
		/// 건물 생성 메서드
		/// </summary>
		/// <param name="buildingData">건물 설정 데이터</param>
		/// <returns>설정이 적용된 건물</returns>
		public Building CreateBuilding(BuildingData buildingData)
		{
			// 프로토타입 건물 복제
			var buildingInstance = Object.Instantiate(m_Prototype, m_Root);
			buildingInstance.gameObject.SetActive(true);

			// 건물 모델 프리팹 생성
			var modelInstance = Object.Instantiate(buildingData.Definition.BuildingPrefab, buildingInstance.transform);

			// Building 컴포넌트 초기화
			buildingInstance.InitComponent(buildingData, modelInstance);

			// TODO 생성된 건물 데이터베이스 등록

			// 빌딩 인스턴스 활성화 및 반환
			buildingInstance.name = "Building (" + modelInstance.name +")";
			return buildingInstance;
		}

		/* 메서드 */
		/// <summary>
		/// 원본 프리팹을 기반으로 프로토타입 생성
		/// </summary>
		void CreatePrototype(Building buildingPrefab)
		{
			// 프로토타입 생성
			m_Prototype = Object.Instantiate(buildingPrefab, m_Root);

			// 프로토타입 설정
			m_Prototype.gameObject.name = "Building (Prototype)";
			m_Prototype.gameObject.SetActive(false);
		}
	}
}
