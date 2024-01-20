using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace IslandMonkey
{
	public enum BuildingType
	{
		None,
		Voyage,
		Functional,
		Special
	}

	/// <summary>
	/// 건물 공용 설정 값을 담고 있는 스크립터블 오브젝트
	/// </summary>
	[CreateAssetMenu(fileName = "Building Definition", menuName = "Data/Building/Definition")]
	public class BuildingDefinition : ScriptableObject
	{
		/* Static 필드 */
		static Dictionary<int, BuildingDefinition> m_Database;

		/* Static API */
		public static BuildingDefinition GetDefinition(int id) => m_Database is not null && m_Database.ContainsKey(id) ? m_Database[id] : null;

		/// <summary>
		/// 모든 Building Definition 색인
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void RuntimeInitialize()
		{
			/* 모든 Building Definition 색인 */
			// TODO 제네릭 클래스
			// Building Definition 로드
			var loadingTasks = Addressables.LoadAssetsAsync<BuildingDefinition>("Definition", null);
			var definitions = loadingTasks.WaitForCompletion(); // TODO 비동기

			m_Database = new Dictionary<int, BuildingDefinition>(definitions.Count);
			foreach (var definition in definitions)
			{
				m_Database.Add(definition.ID, definition);
			}
		}

		/* 필드 */
		[Header("Config")]
		[SerializeField] GoodsFactoryConfig goodsFactoryConfig;

		public GoodsFactoryConfig GetGoodsFactoryConfig() => goodsFactoryConfig ? goodsFactoryConfig : null;

		[Header("Building")]
		[SerializeField] int buildingID = -1;
		[SerializeField] BuildingType buildingType = BuildingType.None; // 건물 종류
		[SerializeField] BuildingModel buildingPrefab; // 건물 프리팹
		[SerializeField] int buildingTime = -1; // 건물 건설 시간 (초)
		[SerializeField] int activeTime = -1;

		// TODO 구조체로 변경
		[Header("Cost")]
		[SerializeField] GoodsType buildCostGoodsType;
		[SerializeField] string buildCost;

		/* 프로퍼티 */
		public int ID => buildingID;
		public BuildingType BuildingType => buildingType;
		public BuildingModel BuildingPrefab => buildingPrefab;
		public int BuildingTime => buildingTime;
		public int ActiveTime => activeTime;
		public string BuildCost => buildCost;
		public GoodsType BuildCostGoodsType => buildCostGoodsType;
	}
}
