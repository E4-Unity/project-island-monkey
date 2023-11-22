using UnityEngine;
using IslandMonkey;

public class TestGold : MonoBehaviour
{
	public GoodsManager goodsManager;

	// Start is called before the first frame update
	void Start()
	{
		if (goodsManager == null)
		{
			Debug.LogError("GoodsManager reference not set on TestGold script.");
		}
	}

	// 이 함수를 호출하면 GoodsManager를 통해 Gold를 1000 증가시킵니다.
	public void EarnThousandGold()
	{
		if (goodsManager != null)
		{
			goodsManager.EarnGoods(GoodsType.Gold, 10000);
		}
	}
}
