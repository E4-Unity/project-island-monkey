using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestUpgradeUI : MonoBehaviour
{
	[SerializeField] private GameObject upgradePool;
	[SerializeField] private GameObject upgradeBuilding;
	[SerializeField] private GameObject upgradeMonkey;
	[SerializeField] private GameObject upgradeBuildingPanel;

	[SerializeField] private GameObject MonkeyBankPopupPanel;
	[SerializeField] private GameObject DrawMachinePanel;

	[SerializeField] private List<GameObject> buildingIconImages; // 아이콘 이미지 리스트
	[SerializeField] private Image iconDisplay; // UI에서 아이콘을 표시할 Image 컴포넌트

	private void Update()
	{
		if (EventSystem.current.IsPointerOverGameObject()) // UI 위에 포인터가 있는지 확인
			return; // UI 위에 있으면 아래의 3D Raycast를 실행하지 않음

		if (Input.GetMouseButtonDown(0))
		{
			CheckRaycast(Camera.main.ScreenPointToRay(Input.mousePosition));
		}

		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
			{
				CheckRaycast(Camera.main.ScreenPointToRay(touch.position));
			}
		}
	}

	private void CheckRaycast(Ray ray)
	{
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			Debug.Log("Hit: " + hit.collider.gameObject.name); // 히트된 오브젝트의 이름을 로그로 출력

			GameObject hitObject = hit.collider.gameObject;

			if (hitObject == upgradeBuilding)
			{
				upgradeBuildingPanel.SetActive(true);
			}
			else if (hitObject.name == "Island BananaHouse")
			{
				upgradePool.SetActive(true);
				upgradeMonkey.SetActive(true);
				upgradeBuilding.SetActive(true);
			}
			else if (hitObject.name == "Island MonkeyBank(Clone)")
			{
				MonkeyBankPopupPanel.SetActive(true);
			}
			else if (hitObject.name == "Island LuckyDraw(Clone)")
			{
				DrawMachinePanel.SetActive(true);
			}
			else if (hitObject.name == "BuildingSlot(Clone)")
			{
				// 'Upgrade' 이름을 가진 자식들을 활성화합니다.
				SetActiveForChildWithName(hitObject, "Upgrade", true);
				ActivateBuildingImageWithTag(hitObject);
			}
			else if (hitObject.name == "icon_building")
			{
				ActivateBuildingImageWithTag(hitObject);
				upgradeBuildingPanel.SetActive(true);
				ActivateBuildingImageWithTag(hitObject);
			}

			else
			{
				// 아무것도 클릭되지 않았을 때 모든 패널을 비활성화합니다.
				upgradePool.SetActive(false);
				upgradeBuildingPanel.SetActive(false);
				MonkeyBankPopupPanel.SetActive(false);
				DrawMachinePanel.SetActive(false);
				// 'Upgrade' 이름을 가진 자식들을 활성화합니다.
				SetActiveForChildWithName(hitObject, "Upgrade", true);
				DeactivateAllPanels();
			}
		}
		else
		{
			Debug.Log("No hit"); // 레이캐스트가 아무것도 맞추지 않았을 때 로그로 출력
			
		}


	}

	private void SetActiveForChildWithName(GameObject parent, string childName, bool active)
	{
		Transform[] children = parent.GetComponentsInChildren<Transform>(true);
		foreach (Transform child in children)
		{
			if (child.name == childName)
			{
				child.gameObject.SetActive(active);
			}
		}
	}


	private void DeactivateAllPanels()
	{
		// 모든 패널을 비활성화하는 메소드
		upgradePool.SetActive(false);
		upgradeBuildingPanel.SetActive(false);
		MonkeyBankPopupPanel.SetActive(false);
		DrawMachinePanel.SetActive(false);

		GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
		foreach (GameObject obj in allObjects)
		{
			if (obj.name == "BuildingSlot(Clone)")
			{
				SetActiveForChildWithName(obj, "Upgrade", false);
			}
		}
	}
	private void ActivateBuildingImageWithTag(GameObject hitObject)
	{
		// 모든 이미지 비활성화
		foreach (var icon in buildingIconImages)
		{
			icon.SetActive(false);
		}

		// 히트된 오브젝트의 태그에 따라 적절한 이미지 활성화
		switch (hitObject.tag)
		{
			case "Island FireStation":
				iconDisplay.sprite = buildingIconImages[0].GetComponent<Image>().sprite; // FireStation 이미지
				break;
			case "Island IceRink":
				iconDisplay.sprite = buildingIconImages[1].GetComponent<Image>().sprite; // IceRink 이미지
				break;
			// 다른 태그에 대한 케이스 추가
		}
	}
}
