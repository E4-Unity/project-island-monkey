using TMPro;
using UnityEngine;

namespace IslandMonkey.MVVM
{
	public class MonkeyCountUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI monkeyCountText;

		BuildingManager buildingManager;

		int monkeyCount;

		void Awake()
		{
			buildingManager = IslandGameManager.Instance.GetBuildingManager();
		}

		void Start()
		{
			Refresh();
			buildingManager.OnBuildingDataRegistered += Refresh;
		}

		void OnDestroy()
		{
			buildingManager.OnBuildingDataRegistered -= Refresh;
		}

		void Refresh()
		{
			monkeyCount = buildingManager.BuildingCountByType.TryGetValue(BuildingType.Voyage, out var value)
				? value
				: 0;

			if (monkeyCountText)
			{
				monkeyCountText.text = monkeyCount.ToString();
			}
		}
	}
}
