using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTabPanel : MonoBehaviour
{
	public Sprite activeImage;
	public Sprite inActiveImage;

	private static BuildingTabPanel currentActiveTab;

	private TabType activeTabType;
	[SerializeField] TabType thisTabType;

	[SerializeField] Button voyageButton;
	[SerializeField] Button FunctionalButton;
	[SerializeField] Button specialButton;

	// 각 탭에 해당하는 게임 오브젝트
	[SerializeField] GameObject voyagePanel;
	[SerializeField] GameObject functionalPanel;
	[SerializeField] GameObject specialPanel;

	public enum TabType
	{
		Voyage,
		Functional,
		Special,
		None
	}

	private void Awake()
	{
		voyageButton.onClick.AddListener(() => OnTabButtonClick(TabType.Voyage));
		FunctionalButton.onClick.AddListener(() => OnTabButtonClick(TabType.Functional));
		specialButton.onClick.AddListener(() => OnTabButtonClick(TabType.Special));

		activeTabType = TabType.Voyage; // 시작 시에 활성화 탭 voyage로 설정
	}

	private void Start()
	{
		
	}

	public void OnTabButtonClick(TabType tabType)
	{
		if (thisTabType != tabType)
		{
			if (currentActiveTab != null && currentActiveTab != this)
			{
				currentActiveTab.DeactivateTab();
			}

			thisTabType = tabType;
			ActivateTab();
		}
		else
		{
			Debug.Log("같은 탭 선택");
		}
	}


	private void DeactivateTab()
	{
		// 현재 활성화된 탭에 해당하는 게임 오브젝트를 비활성화
		GameObject panelToDeactivate = GetPanelByTabType(thisTabType);
		if (panelToDeactivate != null)
		{
			panelToDeactivate.SetActive(false);
		}

		UpdateTabButtonImage(thisTabType, inActiveImage);
		if (currentActiveTab == this)
		{
			currentActiveTab = null;
		}
	}

	// 모든 패널을 비활성화하는 메서드
	private void DeactivateAllPanels()
	{
		// 모든 패널을 비활성화하고, 모든 탭 버튼의 이미지를 inactiveImage로 설정
		if (voyagePanel)
		{
			voyagePanel.SetActive(false);
			voyageButton.GetComponent<Image>().sprite = inActiveImage;
		}
		if (functionalPanel)
		{
			functionalPanel.SetActive(false);
			FunctionalButton.GetComponent<Image>().sprite = inActiveImage;
		}
		if (specialPanel)
		{
			specialPanel.SetActive(false);
			specialButton.GetComponent<Image>().sprite = inActiveImage;
		}
	}

	private void ActivateTab()
	{
		// 모든 패널과 버튼을 비활성화
		DeactivateAllPanels();

		// 이 탭에 해당하는 패널을 활성화
		GameObject panelToActivate = GetPanelByTabType(thisTabType);
		if (panelToActivate != null)
		{
			panelToActivate.SetActive(true);
			// 이 탭에 해당하는 버튼의 이미지만 activeImage로 설정
			Button buttonToActivate = GetButtonByTabType(thisTabType);
			if (buttonToActivate != null)
			{
				buttonToActivate.GetComponent<Image>().sprite = activeImage;
			}
		}

		currentActiveTab = this;
	}

	// 탭 타입에 따라 해당하는 버튼을 반환하는 메서드
	private Button GetButtonByTabType(TabType tabType)
	{
		switch (tabType)
		{
			case TabType.Voyage:
				return voyageButton;
			case TabType.Functional:
				return FunctionalButton;
			case TabType.Special:
				return specialButton;
			default:
				return null;
		}
	}

	// 탭 타입에 따라 해당하는 패널을 반환하는 메서드
	private GameObject GetPanelByTabType(TabType tabType)
	{
		switch (tabType)
		{
			case TabType.Voyage:
				return voyagePanel;
			case TabType.Functional:
				return functionalPanel;
			case TabType.Special:
				return specialPanel;
			default:
				return null;
		}
	}


	private void UpdateTabButtonImage(TabType tabType, Sprite image)
	{
		switch (tabType)
		{
			case TabType.Voyage:
				voyageButton.GetComponent<Image>().sprite = image;
				break;
			case TabType.Functional:
				FunctionalButton.GetComponent<Image>().sprite = image;
				break;
			case TabType.Special:
				specialButton.GetComponent<Image>().sprite = image;
				break;
		}
	}
	public void Quit()
	{
		gameObject.SetActive(false);
		if (currentActiveTab == this)
		{
			currentActiveTab = null;
		}
	}

}
