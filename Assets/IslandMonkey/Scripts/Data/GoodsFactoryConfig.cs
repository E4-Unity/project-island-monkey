using System.Collections.Generic;
using UnityEngine;

namespace IslandMonkey
{
	[CreateAssetMenu(fileName = "Goods Factory Config", menuName = "Data/Building/Config/GoodsFactory")]
	public class GoodsFactoryConfig : ScriptableObject, GoodsFactory.IGoodsFactoryConfig
	{
		/* IGoodsFactoryConfig */
		[SerializeField] GoodsType goodsType;
		[SerializeField] int income = -1;
		[SerializeField] float producingInterval = -1;
		[SerializeField] float popupInterval = -1;
		[SerializeField] Vector3 popupOffset;

		[SerializeField] private int currentLevel = 1;  // 현재 레벨
		[SerializeField] private List<int> incomeLevels = new List<int>();  // 레벨별 수익

		public GoodsType GoodsType => goodsType;
		public int Income => income;
		public float ProducingInterval => producingInterval;
		public float PopupInterval => popupInterval;
		public Vector3 PopupOffset => popupOffset;

		// 현재 레벨에 맞는 수입 값을 가져오는 메서드
		private void UpdateIncome()
		{
			if (currentLevel >= 0 && currentLevel < incomeLevels.Count)
			{
				income = incomeLevels[currentLevel];
			}
			else
			{
				Debug.LogError("Invalid level for income update.");
			}
		}

		// 외부에서 호출하여 레벨을 업그레이드하고 수입을 업데이트
		public void LevelUp()
		{
			currentLevel++;
			UpdateIncome();
		}

		// 현재 레벨을 설정하고, 해당 레벨에 맞는 수입 값을 업데이트
		public void SetLevel(int level)
		{
			currentLevel = level;
			UpdateIncome();
		}
	}
}
