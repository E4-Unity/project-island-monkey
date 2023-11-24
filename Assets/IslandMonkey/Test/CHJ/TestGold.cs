using UnityEngine;
using IslandMonkey;

public class TestGold : MonoBehaviour
{
	[SerializeField] int cheatGold = 10000;
	GoodsManager goodsManager;

	// Start is called before the first frame update
	void Start()
	{
		goodsManager = GlobalGameManager.Instance.GetGoodsManager();
		if (goodsManager == null)
		{
			Debug.LogError("GoodsManager reference not set on TestGold script.");
		}
	}

	// 이 함수를 호출하면 GoodsManager를 통해 Gold를 1000 증가시킵니다.
	public void EarnThousandGold()
	{
		goodsManager.EarnGoods(GoodsType.Gold, cheatGold);
	}
}
