using System;
using System.Collections;
using E4.Utilities;
using UnityEngine;

namespace IslandMonkey
{
	[Serializable]
	public class VoyageSaveData : ISavable
	{
		public BuildingData CurrentBuildingData = new BuildingData(null);
		public MonkeyType MonkeyType = MonkeyType.Basic;
	}
	public class VoyageDataManager : DataManagerClient<VoyageSaveData>
	{
		/* Field */
		[SerializeField] bool shouldBuild;
		int timer;
		bool canEnterVoyageScene;
		Coroutine checkBuildingTimeCoroutine;

		/* Property */
		public bool CanEnterVoyageScene => CurrentBuildingData.IsValid;

		public BuildingData CurrentBuildingData
		{
			get => Data.CurrentBuildingData;
			set
			{
				Data.CurrentBuildingData = value;

				OnCurrentBuildingDataUpdated?.Invoke();
				CheckBuildingData();

				// 데이터 저장
				SaveData();
			}
		}

		public MonkeyType MonkeyType
		{
			get => Data.MonkeyType;
			set => Data.MonkeyType = value;
		}

		public int Timer => timer;
		public float TimeLeftRatio => timer / (float)BuildingTime;
		int BuildingTime => CurrentBuildingData.Definition.BuildingTime;
		int BuildStartedTime => CurrentBuildingData.BuildStartedTime;

		public bool ShouldBuild
		{
			get => shouldBuild;
			set => shouldBuild = value;
		}

		/* Event */
		public event Action OnCurrentBuildingDataUpdated;
		public event Action OnBuildingFinished;

		/* MonoBehaviour */
		protected override void Awake()
		{
			base.Awake();

			CheckBuildingData();
		}

		/* Method */
		public void Clear()
		{
			CurrentBuildingData = new BuildingData(null);
			ShouldBuild = false;
		}
		void CheckBuildingData()
		{
			if (!CurrentBuildingData.IsValid)
			{
				if (checkBuildingTimeCoroutine is null) return;

				StopCoroutine(checkBuildingTimeCoroutine);
				checkBuildingTimeCoroutine = null;

				return;
			}

			if (GetCurrentTime() - BuildStartedTime > BuildingTime)
			{
				FinishBuilding();
			}
			else
			{
				checkBuildingTimeCoroutine = StartCoroutine(CheckBuildingTime());
			}
		}

		void FinishBuilding()
		{
			OnBuildingFinished?.Invoke();
			CurrentBuildingData.IsBuildCompleted = true;
		}

		IEnumerator CheckBuildingTime()
		{
			timer = BuildStartedTime + BuildingTime - GetCurrentTime();
			while (timer > 0)
			{
				var deltaTime = GetCurrentTime() - BuildStartedTime;
				timer = BuildingTime - deltaTime;

				yield return null;
			}

			FinishBuilding();
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
