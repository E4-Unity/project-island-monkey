using UnityEngine;
using UnityEngine.Serialization;

namespace IslandMonkey
{
	public enum BuildingType
	{
		None,
		Voyage,
		Functional,
		Special
	}

	[CreateAssetMenu(fileName = "Building Definition", menuName = "Data/Building/Definition")]
	public class BuildingDefinition : ScriptableObject, GoodsFactory.IGoodsFactoryConfig
	{
		// 테스트 버전으로 활성화
		[SerializeField] bool enableTest;

		[SerializeField] int buildingID = -1;
		[SerializeField] BuildingType buildingType = BuildingType.None; // 건물 종류
		[SerializeField] GameObject buildingPrefab; // 건물 프리팹
		[SerializeField] int buildingTime = -1; // 건물 건설 시간 (초)

		public int ID => buildingID;
		public BuildingType BuildingType => buildingType;
		public GameObject BuildingPrefab => buildingPrefab;
		public int BuildingTime => buildingTime;

		/* IGoodsFactoryConfig */
		[SerializeField] int goodsIncome = -1;
		[SerializeField] float goodsProducingInterval = -1;
		[SerializeField] float goodsPopupInterval = -1;

		public GoodsType GoodsType
		{
			get
			{
				GoodsType goodsType;

				switch (buildingType)
				{
					case BuildingType.Voyage:
						goodsType = GoodsType.Gold;
						break;
					case BuildingType.Functional:
						goodsType = GoodsType.Banana;
						break;
					default:
						goodsType = GoodsType.None;
						break;
				}

				return goodsType;
			}
		}
		public int Income => enableTest ? 100 : goodsIncome;
		public float ProducingInterval => enableTest ? 6 : goodsProducingInterval;

		public float PopupInterval => enableTest ? 2 : goodsPopupInterval;
	}
}
