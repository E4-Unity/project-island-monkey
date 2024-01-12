using IslandMonkey;
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
	[SerializeField] private Button upgradeButton;

	private const string BuildingSlot = "BuildingSlot";
	private const string BuildingSlotClone = "BuildingSlot(Clone)";
	private const string IslandMonkeyBankClone = "Island MonkeyBank(Clone)";
	private const string IslandLuckyDrawClone = "Island LuckyDraw(Clone)";
	private const string Icon_building = "icon_building";

	[SerializeField] private GoodsFactoryConfig bananaHouseConfig;
	[SerializeField] private GoodsFactoryConfig fireStationConfig;
	[SerializeField] private GoodsFactoryConfig iceLinkConfig;
	[SerializeField] private GoodsFactoryConfig monkeyBucksConfig;
	[SerializeField] private GoodsFactoryConfig rooftopGardenConfig;
	[SerializeField] private GoodsFactoryConfig jobSeekingConfig;

	private GoodsManager goodsManager;

	// 각 건물 이름과 연결된 GoodsFactoryConfig의 딕셔너리
	private Dictionary<string, GoodsFactoryConfig> goodsFactoryConfig;
	// 현재 선택된 건물의 이름
	private string currentSelectedBuildingName;

	// 딕셔너리를 초기화하고 각 GoodsFactoryConfig 인스턴스를 추가
	void Awake()
	{

		goodsFactoryConfig = new Dictionary<string, GoodsFactoryConfig>
		{
			{ "Island BananaHouse", bananaHouseConfig },
			{ "Island FireStation", fireStationConfig },
			{ "Island IceRink", iceLinkConfig },
			{ "Island Monkeybucks", monkeyBucksConfig },
			{ "Island RooftopGarden", rooftopGardenConfig },
			{ "Island JopSeeking", jobSeekingConfig }
		};

		goodsManager = GlobalGameManager.Instance.GetGoodsManager(); // GoodsManager의 인스턴스를 가져옵니다.

		upgradeButton.onClick.AddListener(UpgradeBuildingSelected); // Upgrade 버튼에 이벤트 리스너 추가
	}

	// UI 초기화: 첫 번째 건물 아이콘으로 아이콘 디스플레이 설정
	void Start()
	{
		if (buildingIconImages.Count > 0 && buildingIconImages[0].activeInHierarchy)
		{
			Image initialImage = buildingIconImages[0].GetComponent<Image>();
			if (initialImage != null)
			{
				iconDisplay.sprite = initialImage.sprite;
			}
		}
	}

	// 매 프레임마다 UI 포인터 체크 및 마우스/터치 입력에 따른 Raycast 처리
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

	// 업그레이드 버튼 클릭 시 선택된 건물의 GoodsFactoryConfig를 찾아 LevelUp 메소드 호출
	private void UpgradeBuildingSelected()
	{
		// 현재 선택된 건물의 GoodsFactoryConfig를 가져와서 업그레이드 로직 수행
		if (goodsFactoryConfig.TryGetValue(currentSelectedBuildingName, out GoodsFactoryConfig config))
		{
			config.LevelUp();
		}
		else
		{
			Debug.LogError($"GoodsFactoryConfig for {currentSelectedBuildingName} not found.");
		}
	}

	// 현재 포인터(마우스/터치)가 UI 요소 위에 있는지 확인
	private bool IsPointerOverUI(int fingerId = -1)
	{
		return EventSystem.current.IsPointerOverGameObject(fingerId);
	}

	// Raycast를 사용하여 히트된 게임 오브젝트 처리
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

	// 히트된 오브젝트에 따라 다른 UI 구성요소 활성화 (특별 건물 처리)
	private void HandleRaycastHit(GameObject hitObject)
	{
		switch (hitObject.name)
		{
			case var name when name == upgradeBuilding.name:
				upgradeBuildingPanel.SetActive(true);
				string buildingName = hitObject.name.Replace("(Clone)", ""); // 클론된 오브젝트의 이름 정리
				currentSelectedBuildingName = buildingName; // 현재 선택된 건물 이름 저장
				break;
			case IslandLuckyDrawClone:
				DrawMachinePanel.SetActive(true);
				break;
			case IslandMonkeyBankClone:
				MonkeyBankPopupPanel.SetActive(true);
				break;
			case BuildingSlotClone:
				if (name != IslandMonkeyBankClone && name != IslandLuckyDrawClone)
					ActivateUpgrade(hitObject);
				break;
			case Icon_building:
				upgradeBuildingPanel.SetActive(true);
				ActivateBuildingImageWithTag(hitObject);
				break;
			default:
				DeactivateAllPanels();
				break;
		}
	}

	// 히트된 건물에 대한 업그레이드 UI 활성화
	private void ActivateUpgrade(GameObject hitObject)
	{
		SetActiveForChildWithName(hitObject, "Upgrade", true);
		ActivateBuildingImageWithTag(hitObject);
	}

	// 지정된 이름을 가진 자식 게임 오브젝트의 활성/비활성 설정
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

	// 모든 UI 패널 비활성화
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

	// 클릭된 건물에 따라 해당 이미지와 텍스트를 UI에 표시
	private void ActivateBuildingImageWithTag(GameObject hitObject)
	{
		// 모든 이미지 비활성화
		foreach (var icon in buildingIconImages)
		{
			icon.SetActive(false);
		}

		// 이전에 추가된 리스너들 제거
		upgradeButton.onClick.RemoveAllListeners();

		// 클릭된 오브젝트의 자식 중에서 특정 이름을 가진 개체를 찾아서 처리
		Transform[] children = hitObject.GetComponentsInChildren<Transform>(true);

		foreach (Transform child in children)
		{
			// 자식 개체의 이름을 확인하고 해당되는 이미지와 텍스트 활성화
			switch (child.name)
			{
				case var name when name.Contains("Island BananaHouse(Clone)"):
					iconDisplay.sprite = buildingIconImages[0].GetComponent<Image>().sprite;
					textDisplay.text = "바나나하우스";
					textSubDisplay.text = "숭숭이의 안락한 보금자리";
					currentSelectedBuildingName = RemoveCloneFromName(name);
					break;
				case var name when name.Contains("Island FireStation(Clone)"):
					iconDisplay.sprite = buildingIconImages[1].GetComponent<Image>().sprite;
					textDisplay.text = "소방서";
					textSubDisplay.text = "불이야! 화재로부터 섬마을을 지켜요";
					currentSelectedBuildingName = RemoveCloneFromName(name);
					break;
				case var name when name.Contains("Island IceRink(Clone)"):
					iconDisplay.sprite = buildingIconImages[2].GetComponent<Image>().sprite;
					textDisplay.text = "아이스링크";
					textSubDisplay.text = "차가운 얼음판을 달리자!";
					currentSelectedBuildingName = RemoveCloneFromName(name);
					break;
				case var name when name.Contains("Island Monkeybucks(Clone)"):
					iconDisplay.sprite = buildingIconImages[3].GetComponent<Image>().sprite;
					textDisplay.text = "몽키벅스";
					textSubDisplay.text = "카페인 솔솔~ 커피 프린숭 1호점";
					currentSelectedBuildingName = RemoveCloneFromName(name);
					break;
				case var name when name.Contains("Island RooftopGarden(Clone)"):
					iconDisplay.sprite = buildingIconImages[4].GetComponent<Image>().sprite;
					textDisplay.text = "옥상정원";
					textSubDisplay.text = "따사로운 햇살에서 뒹굴뒹굴";
					currentSelectedBuildingName = RemoveCloneFromName(name);
					break;
				case var name when name.Contains("Island Jobseeking(Clone)"):
					iconDisplay.sprite = buildingIconImages[5].GetComponent<Image>().sprite;
					textDisplay.text = "알바몽";
					textSubDisplay.text = "직원 고용했숭!";
					currentSelectedBuildingName = RemoveCloneFromName(name);
					break;
			}
		}

		// 이전에 추가된 리스너들 제거
		upgradeButton.onClick.RemoveAllListeners();

			// 선택된 건물의 GoodsFactoryConfig 찾기
			if (goodsFactoryConfig.TryGetValue(currentSelectedBuildingName, out GoodsFactoryConfig config))
			{
				// 해당 건물에 대한 GoodsFactoryConfig가 있으면 업그레이드 버튼 활성화
				upgradeButton.onClick.AddListener(() => {
					if (config != null)
					{
						config.LevelUp();
						goodsManager.SpendGoods(GoodsType.Gold,1000);
						Debug.Log(" 레벨업 성공");
					}
					else
					{
						Debug.LogError("자료형이 없음");
					}
				});
				upgradeButton.interactable = true; // 버튼 활성화
			}
			else
			{
				// 오류 메시지 출력 및 버튼 비활성화
				Debug.LogError($"GoodsFactoryConfig {currentSelectedBuildingName} 찾을 수 없음 ");
				upgradeButton.interactable = false;
			}
		}
	private string RemoveCloneFromName(string objectName)
	{
		return objectName.Replace("(Clone)", "").Trim();
	}

}


