using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldSpaceUI : MonoBehaviour
{
	[SerializeField] private GameObject upgradePool;
	[SerializeField] private GameObject upgradeBuilding;
	[SerializeField] private GameObject upgradeMonkey;
	[SerializeField] private GameObject upgradeBuildingPanel;

	[SerializeField] private GameObject MonkeyBankPopupPanel;
	[SerializeField] private GameObject DrawMachinePanel;

	[SerializeField] private List<GameObject> buildingIconImages;
	[SerializeField] private Image iconDisplay;
	[SerializeField] private TextMeshProUGUI textDisplay;
	[SerializeField] private TextMeshProUGUI textSubDisplay;

	private const string BuildingSlot = "BuildingSlot";
	private const string BuildingSlotClone = "BuildingSlot(Clone)";
	private const string IslandMonkeyBankClone = "Island MonkeyBank(Clone)";
	private const string IslandLuckyDrawClone = "Island LuckyDraw(Clone)";
	private const string IconBuilding = "icon_building";

	void Start()
	{
		// 아이콘 디스플레이 UI 초기화
		if (buildingIconImages.Count > 0 && buildingIconImages[0].activeInHierarchy)
		{
			Image initialImage = buildingIconImages[0].GetComponent<Image>();
			if (initialImage != null)
			{
				iconDisplay.sprite = initialImage.sprite;
			}
		}
	}

	private void Update()
	{
		if (IsPointerOverUI())
			return;

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			CheckRaycast(ray);
		}

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			if (!IsPointerOverUI(Input.GetTouch(0).fingerId))
				CheckRaycast(ray);
		}

	}


	private bool IsPointerOverUI(int fingerId = -1)
	{
		return EventSystem.current.IsPointerOverGameObject(fingerId);
	}

	private void CheckRaycast(Ray ray)
	{
		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			Debug.Log("Hit Object: " + hit.collider.gameObject.name); // 찍은 오브젝트 이름 로그로 출력
			HandleRaycastHit(hit.collider.gameObject);
		}
		else
		{
			DeactivateAllPanels();
			Debug.Log("No hit");
		}

	}

	private void HandleRaycastHit(GameObject hitObject)
	{


		switch (hitObject.name)
		{
			case var name when name == upgradeBuilding.name:
				upgradeBuildingPanel.SetActive(true);
				break;
			case IslandLuckyDrawClone:
				DrawMachinePanel.SetActive(true);
				break;
			case IslandMonkeyBankClone:
				MonkeyBankPopupPanel.SetActive(true);
				break;
			case BuildingSlot:	//바나나하우스
				ActivateUpgrade(hitObject);
				break;
			case BuildingSlotClone:
				if (name != IslandMonkeyBankClone && name != IslandLuckyDrawClone)
					ActivateUpgrade(hitObject);
				break;
			case IconBuilding:
				upgradeBuildingPanel.SetActive(true);
				ActivateBuildingImageWithTag(hitObject);
				break;
			default:
				DeactivateAllPanels();
				break;
		}
	}

	private void ActivateUpgrade(GameObject hitObject)
	{
		SetActiveForChildWithName(hitObject, "Upgrade", true);
		ActivateBuildingImageWithTag(hitObject);
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
		upgradePool.SetActive(false);
		upgradeBuildingPanel.SetActive(false);
		MonkeyBankPopupPanel.SetActive(false);
		DrawMachinePanel.SetActive(false);

		GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
		foreach (GameObject obj in allObjects)
		{
			if (obj.name == BuildingSlotClone)
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

		// 클릭된 오브젝트의 자식 중에서 특정 이름을 가진 개체를 찾아서 처리
		Transform[] children = hitObject.GetComponentsInChildren<Transform>(true);
		foreach (Transform child in children)
		{
			// 자식 개체의 이름을 확인하고 해당되는 이미지와 텍스트 활성화
			switch (child.name)
			{
				case "Island FireStation(Clone)":
					iconDisplay.sprite = buildingIconImages[1].GetComponent<Image>().sprite;
					textDisplay.text = "소방서"; 
					textSubDisplay.text = "불이야! 화재로부터 섬마을을 지켜요";
					return;
				case "Island IceRink(Clone)":
					iconDisplay.sprite = buildingIconImages[2].GetComponent<Image>().sprite;
					textDisplay.text = "아이스링크";
					textSubDisplay.text = "차가운 얼음판을 달리자!";
					return;
				case "Island MonkeyBucks(Clone)":
					iconDisplay.sprite = buildingIconImages[3].GetComponent<Image>().sprite;
					textDisplay.text = "몽키벅스";
					textSubDisplay.text = "카페인 솔솔~ 커피 프린숭 1호점";
					return;
				case "Island RooftopGarden(Clone)":
					iconDisplay.sprite = buildingIconImages[4].GetComponent<Image>().sprite;
					textDisplay.text = "옥상정원";
					textSubDisplay.text = "따사로운 햇살에서 뒹굴뒹굴";
					return;
				case "Island Jobseeking(Clone)":
					iconDisplay.sprite = buildingIconImages[5].GetComponent<Image>().sprite;
					textDisplay.text = "알바몽";
					textSubDisplay.text = "직원 고용했숭!";
					return;
				// 추가 건물에 대한 케이스를 여기에 추가
			}
		}
	}
}
