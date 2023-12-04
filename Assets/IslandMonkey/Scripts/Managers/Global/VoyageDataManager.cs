using System;
using System.Collections;
using UnityEngine;

namespace IslandMonkey
{
	[Serializable]
	public class VoyageSaveData
	{
		public BuildingData CurrentBuildingData = new BuildingData();
		public MonkeyType MonkeyType = MonkeyType.Basic;
	}
	public class VoyageDataManager : MonoBehaviour, DataManager.ISavable<VoyageSaveData>
	{
		/* Field */
		[SerializeField] bool shouldBuild;
		VoyageSaveData voyageSaveData;
		int timer;
		bool canEnterVoyageScene;
		Coroutine checkBuildingTimeCoroutine;

		/* Property */
		public bool CanEnterVoyageScene => CurrentBuildingData.IsValid;

		public BuildingData CurrentBuildingData
		{
			get => voyageSaveData.CurrentBuildingData;
			set
			{
				voyageSaveData.CurrentBuildingData = value;

				OnCurrentBuildingDataUpdated?.Invoke();
				CheckBuildingData();

				DataManager.SaveData(this);
			}
		}

		public MonkeyType MonkeyType
		{
			get => voyageSaveData.MonkeyType;
			set => voyageSaveData.MonkeyType = value;
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
		void Awake()
		{
			var saveData = DataManager.LoadData(this);
			voyageSaveData = saveData ?? new VoyageSaveData();

			CheckBuildingData();
		}

		/* Method */
		public void Clear()
		{
			CurrentBuildingData = new BuildingData();
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

		/* ISavable 인터페이스 구현 */
		public const string SaveFileName = "VoyageSaveData.json";
		public string FileName => SaveFileName;

		public VoyageSaveData Data => voyageSaveData;
	}
}
