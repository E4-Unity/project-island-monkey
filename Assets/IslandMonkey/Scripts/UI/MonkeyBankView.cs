using System.Numerics;
using IslandMonkey.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandMonkey.MVVM
{
	public class MonkeyBankView : View
	{
		[SerializeField] private TextMeshProUGUI[] goldTextList;
		[SerializeField] private GameObject GoldBankMonkey;
		[SerializeField] private Button GetBankGoldButton; // 버튼을 위한 SerializeField 추가

		private GoodsManager goodsManager; // 굿즈 매니저에 대한 참조를 저장합니다.

		protected override void Start()
		{
			base.Start();
			goodsManager = GlobalGameManager.Instance.GetGoodsManager();
		}
		public BigInteger Gold
		{
			get
			{
				// 가정: goldTextList의 첫 번째 요소가 '은행의 골드'를 나타냅니다.
				if (goldTextList.Length > 0 && goldTextList[0] != null)
				{
					// 골드 텍스트의 쉼표를 제거하고 숫자로 파싱합니다.
					string goldTextWithoutCommas = goldTextList[0].text.Replace(",", "");
					if (int.TryParse(goldTextWithoutCommas, out int goldValue))
					{
						return goldValue;
					}
				}
				return 0;
			}
			set
			{
				foreach (var goldText in goldTextList)
				{
					if (goldText)
						goldText.SetText(value.FormatLargeNumber());
				}

				GoldBankMonkey.SetActive(value > 0);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			goodsManager = GlobalGameManager.Instance.GetGoodsManager();
			if (GetBankGoldButton != null) // 버튼이 할당되었는지 확인
			{
				GetBankGoldButton.onClick.AddListener(CollectAllGold); // 버튼의 onClick 이벤트에 메서드 추가
			}
			else
			{
				Debug.LogWarning("GetBankGoldButton is not assigned in the inspector!");
			}
		}

		protected override void RegisterProperties()
		{
			RegisterProperty(nameof(Gold));
		}

		// 모든 골드를 회수하는 함수입니다.
		public void CollectAllGold()
		{
			// 가정: GoodsManager에 EarnGoods 함수가 있고, 골드를 플레이어의 총계에 추가할 수 있습니다.
			goodsManager.EarnGoods(GoodsType.Gold, Gold);
			Debug.Log("EarnGold");


			// 은행의 골드를 0으로 설정합니다.
			Gold = 0;
			GoldBankMonkey.SetActive(false);
		}
	}
}
