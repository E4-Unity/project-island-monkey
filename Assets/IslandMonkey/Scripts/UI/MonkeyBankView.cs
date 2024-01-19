using System;
using System.Numerics;
using IslandMonkey.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandMonkey.MVVM
{
	public class MonkeyBankView : View
	{
		[Header("Gold")]
		[SerializeField] private TextMeshProUGUI[] goldTextList;
		[SerializeField] private GameObject GoldBankMonkey;

		[Header("Time Record")]
		[SerializeField] Slider timeSlider;
		[SerializeField] TextMeshProUGUI timeText;

		/* Field */
		SerializedBigInteger gold = new SerializedBigInteger();
		SerializedBigInteger goldLimit = new SerializedBigInteger();
		int lastGetRewardsTime;
		int maxTimeRecord;

		public BigInteger Gold
		{
			set
			{
				gold.Value = value;

				foreach (var goldText in goldTextList)
				{
					if (goldText)
						goldText.SetText(value.FormatLargeNumber());
				}

				GoldBankMonkey.SetActive(gold.Value == goldLimit.Value);
			}
		}

		public BigInteger GoldLimit
		{
			set
			{
				goldLimit.Value = value;

				GoldBankMonkey.SetActive(gold.Value == goldLimit.Value);
			}
		}

		public int LastGetRewardsTime
		{
			set
			{
				lastGetRewardsTime = value;
				UpdateTimeText();
			}
		}

		public int MaxTimeRecord
		{
			set
			{
				maxTimeRecord = value;
				UpdateTimeText();
			}
		}

		protected override void RegisterProperties()
		{
			RegisterProperty(nameof(Gold));
			RegisterProperty(nameof(GoldLimit));
			RegisterProperty(nameof(LastGetRewardsTime));
			RegisterProperty(nameof(MaxTimeRecord));
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			UpdateTimeText();
		}

		void UpdateTimeText()
		{
			int timeRecord = GetCurrentTime() - lastGetRewardsTime;
			int timeRecordMin = timeRecord / 60;
			int maxTimeRecordMin = maxTimeRecord / 60;

			// timeRecordMin이 maxTimeRecordMin을 넘지 않도록 함
			timeRecordMin = Mathf.Min(timeRecordMin, maxTimeRecordMin);

			timeText.text = string.Format("{0} / {1} 분", timeRecordMin, maxTimeRecordMin);
			timeSlider.value = (float)timeRecordMin / maxTimeRecordMin;
		}

		// TODO 라이브러리
		int GetCurrentTime()
		{
			var now = DateTime.Now.ToLocalTime();
			var span = now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
			var currentTime = (int)span.TotalSeconds;

			return currentTime;
		}
	}
}
