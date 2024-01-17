using IslandMonkeyPack;
using UnityEngine;

namespace IslandMonkey
{
	/// <summary>
	/// 빌딩 모델 프리팹에 부착되는 여러 컴포넌트들을 모아둔 컨테이너 클래스
	/// </summary>
	public class BuildingModel : MonoBehaviour
	{
		/* 컴포넌트 */
		BuildingAnimator m_BuildingAnimator;
		BuildingPayload m_BuildingPayload;

		/* 프로퍼티 */
		public BuildingAnimator GetBuildingAnimator() => m_BuildingAnimator;
		public BuildingPayload GetBuildingPayload() => m_BuildingPayload;

		/* MonoBehaviour */
		void Awake()
		{
			// 컴포넌트 할당
			m_BuildingAnimator = GetComponentInChildren<BuildingAnimator>();
			m_BuildingPayload = GetComponent<BuildingPayload>();
		}
	}
}
