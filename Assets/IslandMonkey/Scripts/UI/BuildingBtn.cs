using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using IslandMonkey;

// 건물 정보를 저장할 수 있는 직렬화 가능한 클래스
[System.Serializable]
public class BuildingData
{
	public int buildingIndex; // 건물의 인덱스
	public Vector3 position; // 건물의 위치
	public bool isCompleted; // 건물이 완성되었는지 여부
	public int monkeyType; // 몽키 타입 정보
	public bool hasMonkey; // 원숭이 생성 여부를 나타내는 새로운 필드
						   // 필요한 추가 정보를 여기에 추가
}


public class BuildingBtn : MonoBehaviour, DataManager.ISavable
{
	[SerializeField] private List<Button> buildingButtons; // 건물 버튼 리스트
	[SerializeField] private List<GameObject> finList; // 완료된 건물 UI 이미지 리스트
	[SerializeField] private List<int> payGoldList; // 건물 건설에 필요한 골드 리스트
	[SerializeField] private ShowcaseMonkey showcaseMonkey; // 연출용 원숭이 프리팹
	[SerializeField] private GameObject buildingMonkeyPrefab; // 원숭이 프리팹
	private GameObject buildingMonkey;

	public HexagonalPlacementManager placementManager; // 헥사곤 배치 매니저

	[SerializeField] private GameObject getAnimalPanel; // 동물 획득 UI 패널
	[SerializeField] private GameObject buildingPanel; // 건물 UI 패널

	List<BuildingData> buildings; // 건물 정보 리스트

	// 캐릭터 등장 연출
	CutsceneController cutsceneController;

	// 컴포넌트
	VoyageDataManager voyageDataManager;

	void Start()
	{
		// 컴포넌트 할당
		voyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();

		LoadBuildingData(); // 저장된 건물 데이터 불러오기

		// 이벤트 바인딩
		cutsceneController = getAnimalPanel.GetComponent<CutsceneController>();
		if (showcaseMonkey && cutsceneController)
		{
			cutsceneController.OnCutSceneEnd += OnCutSceneEnd_Event;
		}

		for (int i = 0; i < buildingButtons.Count; i++)
		{
			int index = i;
			buildingButtons[i].onClick.AddListener(() => OnBuildingButtonClicked(index));
		}
	}

	// TODO 나중에 index 캐싱
	bool IsBuildingAlreadyExist(int index)
	{
		var existingData = buildings.Find(data => data.buildingIndex == index);
		return existingData is not null;
	}

	void OnCutSceneEnd_Event()
	{
		showcaseMonkey.ToggleAnimation();
	}

	void InitMonkey(Monkey monkey, MonkeyType monkeyType)
	{
		if (monkey is null) return;

		monkey.ChangeSkin(monkeyType);

#if UNITY_EDITOR
		Debug.Log(monkey.name + " 스킨 변경 완료");
#endif
	}

	private void OnBuildingButtonClicked(int buttonIndex)
	{
		// 이미 존재하는 건물은 건설하지 않음
		if (IsBuildingAlreadyExist(buttonIndex))
		{
			Debug.LogWarning("이미 건설된 건물입니다 : " + buttonIndex);
			return;
		}

		if (buttonIndex >= 0 && buttonIndex < payGoldList.Count)
		{
			int payGold = payGoldList[buttonIndex];
			GoodsManager goodsManager = GlobalGameManager.Instance.GetGoodsManager();

			if (goodsManager.CanSpend(GoodsType.Gold, payGold))
			{
				goodsManager.SpendGoods(GoodsType.Gold, payGold);
				if (buttonIndex == 0 || buttonIndex == 4 || buttonIndex == 6)
				{
					StartCoroutine(BuildingSequence(buttonIndex)); // 연출 시작
				}
				else
				{
					SpawnBuildingWithoutSequence(buttonIndex); // 연출 없이 건물 건설 및 데이터 저장
				}
			}
			else
			{
				Debug.Log("건설에 필요한 골드가 부족합니다.");
			}
		}
		else
		{
			Debug.LogWarning("버튼 인덱스가 payGoldList 범위를 벗어났습니다.");
		}
	}

	IEnumerator BuildingSequence(int buttonIndex)
	{
		getAnimalPanel.SetActive(true);
		buildingPanel.SetActive(false);

		// 원숭이 초기화
		// 원숭이 스킨을 변경하고 저장합니다.
		MonkeyType selectedType = MonkeyType.Basic; // 기본값 설정
		switch (buttonIndex)
		{
			case 0:
				selectedType = MonkeyType.Basic;
				break;
			case 4:
				selectedType = MonkeyType.Barista;
				break;
			case 6:
				selectedType = MonkeyType.Boss;
				break;
		}

		Debug.Log("스킨 변경 시도: " + selectedType.ToString());

		// 연출용 원숭이를 활성화 후 초기화합니다.
		showcaseMonkey.gameObject.SetActive(true);
		InitMonkey(showcaseMonkey, selectedType);

		// 건물에 상주할 원숭이 오브젝트를 생성하고 위치를 설정합니다.
		buildingMonkey = Instantiate(buildingMonkeyPrefab, placementManager.GetLastSpawnedBuildingPosition(), Quaternion.identity);
		InitMonkey(buildingMonkey.GetComponent<Monkey>(), selectedType);
		buildingMonkey.SetActive(false);

		SpawnBuildingAndSaveData(buttonIndex, (int)selectedType); // 건물 건설 및 데이터 저장

		yield return new WaitForSeconds(10f); // 연출 지연

		// 항해 씬에 넘길 데이터
		// 건설 시작 시간 기록
		var now = DateTime.Now.ToLocalTime();
		var span = now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
		voyageDataManager.BuildingTimeStamp = (int)span.TotalSeconds;

		// 원숭이 타입
		voyageDataManager.MonkeyType = selectedType;

		SceneManager.LoadScene("VoyageTest"); // 샘플 씬 로드
	}

	private void SpawnBuildingWithoutSequence(int buttonIndex)
	{
		buildingPanel.SetActive(false); // TODO 필요한가?
		SpawnBuildingAndSaveData(buttonIndex);
	}

	private void SpawnBuildingAndSaveData(int buttonIndex, int monkeyType = -1)
	{
		// 유효성 검사
		if (placementManager is null) return;

		// 건설되지 않은 경우에만 실행
		if (IsBuildingAlreadyExist(buttonIndex)) return;

		placementManager.SpawnBuilding(buttonIndex);
		Vector3 buildingPosition = placementManager.GetLastSpawnedBuildingPosition();

		var newBuildingData = new BuildingData()
		{
			buildingIndex = buttonIndex,
			position = buildingPosition,
			isCompleted = true
		};

		if (monkeyType != -1)
		{
			newBuildingData.hasMonkey = true;
			newBuildingData.monkeyType = monkeyType;
		}

		buildings.Add(newBuildingData);
		SaveBuildingData(); // 데이터 JSON으로 저장

		UpdateBuildingUI(buttonIndex);
	}

	private void UpdateBuildingUI(int buttonIndex)
	{
		buildingButtons[buttonIndex].gameObject.SetActive(false); // 해당 건물 버튼 비활성화
		finList[buttonIndex].SetActive(true); // 완료된 건물 UI 활성화
	}

	// BuildingBtn에서 이제 DataManager의 인스턴스를 사용하여 데이터를 처리합니다.
	private void SaveBuildingData()
	{
		DataManager.SaveData(this);
	}

	private void LoadBuildingData()
	{
		var data = DataManager.LoadData<SerializableList<BuildingData>>(this);
		buildings = data is null ? new List<BuildingData>() : data.list;

		foreach (var building in buildings)
		{
			// 건물이 이미 씬에 존재하는지 확인합니다.
			if (!IsBuildingSpawned(building))
			{
				GameObject buildingPrefab = placementManager.GetBuildingPrefab(building.buildingIndex);
				if (buildingPrefab != null)
				{
					Instantiate(buildingPrefab, building.position, Quaternion.identity);

					// 원숭이 생성 여부를 확인합니다.
					if (building.hasMonkey)
					{
						var monkeyInstance = Instantiate(buildingMonkeyPrefab, building.position, Quaternion.identity);
						var skinController = monkeyInstance.GetComponent<MonkeySkinController>();
						if (skinController != null)
						{
							skinController.ChangeSkin((MonkeyType)building.monkeyType);
						}
					}
				}
			}
		}

	}

	// 건물이 이미 씬에 존재하는지 확인하는 메서드입니다.
	private bool IsBuildingSpawned(BuildingData buildingData)
	{
		foreach (var existingBuilding in FindObjectsOfType<Building>())
		{
			if ((existingBuilding.transform.position - buildingData.position).sqrMagnitude < 0.1f)
			{
				return true; // 이미 존재하는 건물이 있음
			}
		}
		return false; // 씬에 건물이 존재하지 않음
	}

	private void ResetUI()
	{
		// 모든 건물 버튼을 활성화하고, 완료된 건물 UI를 비활성화합니다.
		for (int i = 0; i < buildingButtons.Count; i++)
		{
			buildingButtons[i].gameObject.SetActive(true);
			if (i < finList.Count)
			{
				finList[i].SetActive(false);
			}
		}

		// PlayerPrefs에서 저장된 건물 완성 정보를 삭제합니다.
		for (int i = 0; i < buildingButtons.Count; i++)
		{
			PlayerPrefs.DeleteKey("BuildingCompleted_" + i);
		}
		PlayerPrefs.Save();
	}

	bool CanSpendGold(in int amount) => GameManager.Instance.CanSpend(GoodsType.Gold, amount);
	void SpendGold(in int amount) => GameManager.Instance.SpendGoods(GoodsType.Gold, amount);

	/* ISavable 인터페이스 구현 */
	[System.Serializable]
	private class SerializableList<T>
	{
		public List<T> list;
		public SerializableList(List<T> list)
		{
			this.list = list;
		}
	}

	public string FileName => "buildingData.json";

	public object Data => new SerializableList<BuildingData>(buildings);
}
