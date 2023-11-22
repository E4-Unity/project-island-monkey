using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using IslandMonkey;

// 건물 정보를 저장할 수 있는 직렬화 가능한 클래스
[System.Serializable]
public class BuildingData
{
	public int buildingIndex; // 건물의 인덱스
	public Vector3 position; // 건물의 위치
	public bool isCompleted; // 건물이 완성되었는지 여부
							 // 필요한 추가 정보를 여기에 추가
}


public class BuildingBtn : MonoBehaviour
{
	[SerializeField] private List<Button> buildingButtons; // 건물 버튼 리스트
	[SerializeField] private List<GameObject> finList; // 완료된 건물 UI 이미지 리스트
	[SerializeField] private List<int> payGoldList; // 건물 건설에 필요한 골드 리스트
	[SerializeField] private GameObject monkeyPrefab; // 원숭이 프리팹
	private GameObject spawnedMonkey; // 활성화될 원숭이 오브젝트

	public HexagonalPlacementManager placementManager; // 헥사곤 배치 매니저

	[SerializeField] private GameObject getAnimalPanel; // 동물 획득 UI 패널
	[SerializeField] private GameObject buildingPanel; // 건물 UI 패널

	private List<BuildingData> buildings = new List<BuildingData>(); // 건물 정보 리스트

	void Start()
	{
		LoadBuildingData(); // 저장된 건물 데이터 불러오기
		spawnedMonkey = Instantiate(monkeyPrefab, new Vector3(0, -0.025f, -1), Quaternion.Euler(0, 180, 0)); // 원숭이 생성 및 회전
		spawnedMonkey.SetActive(false); // 처음에는 원숭이를 비활성화

		for (int i = 0; i < buildingButtons.Count; i++)
		{
			int index = i;
			buildingButtons[i].onClick.AddListener(() => OnBuildingButtonClicked(index));
		}
	}

	private void OnBuildingButtonClicked(int buttonIndex)
	{
		if (buttonIndex >= 0 && buttonIndex < payGoldList.Count)
		{
			int payGold = payGoldList[buttonIndex];
			GoodsManager goodsManager = GlobalGameManager.instance.GetGoodsManager();

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

		// 원숭이 오브젝트를 활성화합니다.
		spawnedMonkey.SetActive(true);

		// 원숭이 스킨 컨트롤러에 접근합니다.
		var skinController = spawnedMonkey.GetComponent<MonkeySkinController>();
		if (skinController != null)
		{
			// 원숭이 스킨을 변경합니다.
			switch (buttonIndex)
			{
				case 0:
					skinController.ChangeSkin(MonkeyType.Basic);
					break;
				case 4:
					skinController.ChangeSkin(MonkeyType.Barista);
					break;
				case 6:
					skinController.ChangeSkin(MonkeyType.BuildingOwner);
					break;
			}
		}
		else
		{
			Debug.LogError("Spawned monkey does not have a MonkeySkinController component attached.");
		}

		

		yield return new WaitForSeconds(10f); // 연출 지연

		getAnimalPanel.SetActive(false);
		spawnedMonkey.SetActive(false); // 원숭이 비활성화

		SpawnBuildingAndSaveData(buttonIndex); // 건물 건설 및 데이터 저장
		SceneManager.LoadScene("SampleScene"); // 샘플 씬 로드
	}

	private void SpawnBuildingWithoutSequence(int buttonIndex)
	{
		buildingPanel.SetActive(false);
		SpawnBuildingAndSaveData(buttonIndex);
	}

	private void SpawnBuildingAndSaveData(int buttonIndex)
	{
		if (placementManager != null)
		{
			placementManager.SpawnBuilding(buttonIndex);
			Vector3 buildingPosition = placementManager.GetLastSpawnedBuildingPosition();

			// 현재 인덱스에 해당하는 기존의 데이터를 찾아서 업데이트하거나 새로 추가합니다.
			var existingData = buildings.Find(data => data.buildingIndex == buttonIndex);
			if (existingData != null)
			{
				existingData.position = buildingPosition; // 위치 업데이트
				existingData.isCompleted = true; // 완성 상태 업데이트
			}
			else
			{
				buildings.Add(new BuildingData()
				{
					buildingIndex = buttonIndex,
					position = buildingPosition,
					isCompleted = true
				});
			}
			UpdateBuildingUI(buttonIndex);
			SaveBuildingData(); // 데이터 JSON으로 저장
		}
	}

	private void UpdateBuildingUI(int buttonIndex)
	{
		buildingButtons[buttonIndex].gameObject.SetActive(false); // 해당 건물 버튼 비활성화
		finList[buttonIndex].SetActive(true); // 완료된 건물 UI 활성화
	}

	// BuildingBtn에서 이제 DataManager의 인스턴스를 사용하여 데이터를 처리합니다.
	private void SaveBuildingData()
	{
		DataManager.Instance.SaveBuildingData();
	}

	private void LoadBuildingData()
	{
		DataManager.Instance.LoadBuildingData();
		buildings = DataManager.Instance.Buildings; // DataManager에서 로드한 데이터로 buildings 리스트 갱신

		foreach (var building in buildings)
		{
			// 건물 오브젝트를 생성합니다.
			GameObject buildingPrefab = placementManager.GetBuildingPrefab(building.buildingIndex);
			if (buildingPrefab != null)
			{
				Instantiate(buildingPrefab, building.position, Quaternion.identity);
			}

			// 건물의 UI 상태를 복원합니다.
			buildingButtons[building.buildingIndex].gameObject.SetActive(!building.isCompleted);
			if (building.isCompleted && building.buildingIndex < finList.Count)
			{
				finList[building.buildingIndex].SetActive(true);
			}
		}
	}

	private void DeleteBuildingData()
	{
		DataManager.Instance.DeleteBuildingData();
		// Buildings 리스트를 클리어합니다.
		
		string filePath = Path.Combine(Application.persistentDataPath, "buildingData.json");
		if (File.Exists(filePath))
		{
			File.Delete(filePath);
			Debug.Log("건물 데이터가 삭제되었습니다.");
		}
		else
		{
			Debug.LogWarning("삭제할 건물 데이터가 없습니다!");
		}

		// 데이터 파일의 존재 여부와 상관없이 UI와 건물 데이터를 리셋합니다.
		buildings.Clear();
		ResetUI();
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

	[System.Serializable]
	private class SerializableList<T>
	{
		public List<T> list;
		public SerializableList(List<T> list)
		{
			this.list = list;
		}
	}



	bool CanSpendGold(in int amount) => GameManager.instance.CanSpend(GoodsType.Gold, amount);
	void SpendGold(in int amount) => GameManager.instance.SpendGoods(GoodsType.Gold, amount);
}
