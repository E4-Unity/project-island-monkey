
namespace IslandMonkey
{
	public class FishingMonkey : Monkey
	{
		protected override void Start()
		{
			base.Start();

			ChangeSkin(GlobalGameManager.Instance.GetVoyageDataManager().MonkeyType);
		}
	}
}
