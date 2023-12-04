using System.Numerics;

namespace IslandMonkey.MVVM
{
	public class GoodsViewModel : ViewModel
	{
		void Awake()
		{
			PropertyNotifier = IslandGameManager.Instance.GetGoodsManager();
		}
	}
}
