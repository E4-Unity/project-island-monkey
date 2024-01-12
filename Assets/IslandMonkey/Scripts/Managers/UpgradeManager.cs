using IslandMonkey;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
	GoodsManager goodsManager;

	void Start()
	{
		goodsManager = GlobalGameManager.Instance.GetGoodsManager();
	}

	public void SpendUpgradeGold()
	{
		// 여기서 골드가 있는 지 확인
		if (goodsManager.Gold >= 1000)
		{
			// 임시 데이터에 따라 모든 건물 소모 골드 1000골드
			goodsManager.SpendGoods(GoodsType.Gold, 1000);
		}
	}

}
