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

		public GoodsType GoodsType => goodsType;
		public int Income => income;
		public float ProducingInterval => producingInterval;
		public float PopupInterval => popupInterval;
	}
}
