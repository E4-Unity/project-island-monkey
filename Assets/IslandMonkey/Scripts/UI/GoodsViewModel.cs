using System.Numerics;

namespace IslandMonkey.MVVM
{
	public class GoodsViewModel : ViewModel
	{
		public BigInteger Gold { get; set; }
		public BigInteger Banana { get; set; }
		public BigInteger Clam { get; set; }

		protected override void Start()
		{
			base.Start();

			// Model 등록
			PropertyNotifier = GlobalGameManager.Instance.GetGoodsManager();
		}

		protected override void RegisterProperties()
		{
			RegisterProperty(nameof(Gold));
			RegisterProperty(nameof(Banana));
			RegisterProperty(nameof(Clam));
		}
	}
}
