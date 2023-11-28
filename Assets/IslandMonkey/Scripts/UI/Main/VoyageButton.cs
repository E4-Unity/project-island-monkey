using UnityEngine;
using UnityEngine.UI;

namespace IslandMonkey
{
	public class VoyageButton : MonoBehaviour
	{
		[SerializeField] Button voyageButton;
		[SerializeField] bool autoDetectButton;
		VoyageDataManager voyageDataManager;
		bool hasEventBound;

		void Awake()
		{
			if (autoDetectButton && !voyageButton)
			{
				voyageButton = GetComponent<Button>();
			}
		}

		void Start()
		{
			if (voyageButton)
			{
				voyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();
				Refresh();
				voyageDataManager.OnCurrentBuildingDataUpdated += Refresh;
				hasEventBound = true;
			}
		}

		void Refresh()
		{
			voyageButton.interactable = voyageDataManager.CanEnterVoyageScene;
		}

		void OnDestroy()
		{
			if (hasEventBound)
			{
				voyageDataManager.OnCurrentBuildingDataUpdated -= Refresh;
			}
		}
	}
}
