using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;



namespace IslandMonkey
{
	public class GoodsFactory : MonoBehaviour
	{
		enum FactoryState
		{
			Init, // 초기화가 필요한 상태
			Activated,
			Deactivated
		}

		public interface IGoodsFactoryConfig
		{
			GoodsType GoodsType { get; }
			int Income { get; }
			float ProducingInterval { get; }
			float PopupInterval { get; }
			Vector3 PopupOffset { get; }
		}

		/* Config */
		[SerializeField] GoodsType goodsType = GoodsType.None;
		[SerializeField] int income = 100;
		[SerializeField] float interval = 6f;
		[SerializeField] float popupInterval = 2.0f;
		[SerializeField] GameObject popupImage;

		//[SerializeField] private int currentLevel = 1;
		//[SerializeField] private List<int> incomeLevels = new List<int>();

		// 자동 활성화 설정
		[SerializeField] bool autoConfig;
		[SerializeField] GoodsFactoryConfig defaultConfig;



		/* Component */
		GoodsManager goodsManager;
		MonkeyBank monkeyBank;

		/* Field */
		FactoryState state = FactoryState.Init;
		Coroutine producingCoroutine;
		float timer = 0;

		/* Event */
		public event Action<int> OnGoodsProduced; // 스크립트 전용 이벤트

		// clickedPosition 선언 및 초기화
		private Vector2 clickedPosition = Vector2.zero;

		[SerializeField] Transform target;
		[SerializeField] ItemAcquireFx prefabItem;

		void Start()
		{
			goodsManager = GlobalGameManager.Instance.GetGoodsManager();
			monkeyBank = GlobalGameManager.Instance.GetMonkeyBank();

			popupImage.SetActive(false);

			if (autoConfig)
			{
				Init(defaultConfig);
			}

			// 이벤트 구독
			if (defaultConfig != null)
			{
				defaultConfig.OnLevelUp += UpdateIncomeFromConfig;
			}
		}

		public void Init(IGoodsFactoryConfig config)
		{
			if (config is null || config.GoodsType == GoodsType.None) return;

			// GoodsFactoryConfig의 참조를 저장합니다.
			defaultConfig = config as GoodsFactoryConfig;

			goodsType = config.GoodsType;
			income = config.Income;
			interval = config.ProducingInterval;
			popupInterval = config.PopupInterval;

			state = FactoryState.Deactivated;

			// TODO 임시
			popupImage.transform.localPosition = config.PopupOffset;

			// income 값을 config에서 가져옵니다.
			UpdateIncomeFromConfig();
		}
		private void UpdateIncomeFromConfig()
		{
			// config가 null이 아닌지 확인하고 income 값을 업데이트합니다.
			if (defaultConfig != null)
			{
				income = defaultConfig.Income;
			}
		}


		public void Activate()
		{
			if (state == FactoryState.Activated || goodsType == GoodsType.None) return;
			state = FactoryState.Activated;

			// 재화 생산 시작
			producingCoroutine = StartCoroutine(Producing());
		}

		public void Deactivate()
		{
			if (state == FactoryState.Deactivated) return;
			state = FactoryState.Deactivated;

			// 재화 생산 중단
			if (producingCoroutine is null) return;

			StopCoroutine(producingCoroutine);
			producingCoroutine = null;
		}

		public void ResetTimer()
		{
			popupImage.SetActive(false);
			timer = 0;
		}

		void ProduceGoods()
		{
			switch (goodsType)
			{
				case GoodsType.Gold:
					monkeyBank.AddToBank(income);
					break;
				default:
					goodsManager.EarnGoods(goodsType, income);
					break;
			}

			OnGoodsProduced?.Invoke(income);
		}

		void EarnGoods()
		{
			goodsManager.EarnGoods(goodsType, income);
		}


		public void OnClicked()
		{
			if (!popupImage.activeSelf) return;
			ResetTimer();
			EarnGoods();
			SoundManager.Instance.PlaySoundEffect("Acquisition_Goods");

			// Example 스크립트 활성화
			Example exampleScript = FindObjectOfType<Example>();
			if (exampleScript != null)
			{
				exampleScript.ActivateItemFx();
			}
		}

		IEnumerator Producing()
		{
			while (true)
			{
				yield return null;
				timer += Time.deltaTime;

				// TODO 선택적 비활성화 방법 리팩토링
				if (popupInterval > 0 && popupImage && !popupImage.activeSelf && timer > popupInterval)
				{
					popupImage.SetActive(true);
				}

				if (interval > 0 && timer > interval)
				{
					ResetTimer();
					ProduceGoods();
				}
			}
		}
		void OnDestroy()
		{
			// 이벤트 구독 해제
			if (defaultConfig != null)
			{
				defaultConfig.OnLevelUp -= UpdateIncomeFromConfig;
			}
		}
	}
}
