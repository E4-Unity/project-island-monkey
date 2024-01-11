using UnityEngine;

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

		[Header("Test")]
		[SerializeField] bool enableTest; // 테스트 버전으로 활성화

		public int ID => buildingID;
		public BuildingType BuildingType => buildingType;
		public BuildingModel BuildingPrefab => buildingPrefab;
		public int BuildingTime => buildingTime;
		public int ActiveTime => activeTime;
		public string BuildCost => buildCost;
		public GoodsType BuildCostGoodsType => buildCostGoodsType;
	}
}
