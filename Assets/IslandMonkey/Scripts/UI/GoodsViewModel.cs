using System.Numerics;

namespace IslandMonkey.MVVM
{
	public class GoodsViewModel : ViewModel
	{
		void Awake()
		{
			PropertyNotifier = GlobalGameManager.Instance.GetGoodsManager();
		}
	}
}
