using System;
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
		VoyageSaveData voyageSaveData;

		/* Property */
		public bool CanEnterVoyageScene => voyageSaveData.CurrentBuildingData.IsValid;

		public BuildingData CurrentBuildingData
		{
			get => voyageSaveData.CurrentBuildingData;
			set
			{
				voyageSaveData.CurrentBuildingData = value;

				OnCurrentBuildingDataUpdated?.Invoke();

				DataManager.SaveData(this);
			}
		}

		public MonkeyType MonkeyType
		{
			get => voyageSaveData.MonkeyType;
			set => voyageSaveData.MonkeyType = value;
		}

		/* Event */
		public event Action OnCurrentBuildingDataUpdated;

		/* MonoBehaviour */
		void Awake()
		{
			var saveData = DataManager.LoadData(this);
			voyageSaveData = saveData ?? new VoyageSaveData();
		}

		/* ISavable 인터페이스 구현 */
		public const string SaveFileName = "VoyageSaveData.json";
		public string FileName => SaveFileName;

		public VoyageSaveData Data => voyageSaveData;
	}
}
