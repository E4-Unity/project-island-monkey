using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using IslandMonkey;

public class BuildingBtn : MonoBehaviour
{
	[SerializeField] private List<Button> buildingButtons; // 건물 버튼 리스트
	[SerializeField] private List<GameObject> finList; // 완료된 건물 UI 이미지 리스트
	[SerializeField] private List<int> payGoldList; // 건물 건설에 필요한 골드 리스트
	[SerializeField] private ShowcaseMonkey showcaseMonkey; // 연출용 원숭이 프리팹

	[SerializeField] private GameObject getAnimalPanel; // 동물 획득 UI 패널
	[SerializeField] private GameObject buildingPanel; // 건물 UI 패널

	// 캐릭터 등장 연출
	CutsceneController cutsceneController;

	// 컴포넌트
	VoyageDataManager voyageDataManager;
	BuildingManager buildingManager;
	GoodsManager goodsManager;
	HexagonalPlacementManager placementManager;

	void Start()
	{
		// 컴포넌트 할당
		goodsManager = GlobalGameManager.Instance.GetGoodsManager();
		voyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();
		buildingManager = IslandGameManager.Instance.GetBuildingManager();
		placementManager = IslandGameManager.Instance.GetPlacementManager();

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
		if (buildingManager.IsBuildingAlreadyExist(buttonIndex))
		{
			Debug.LogWarning("이미 건설된 건물입니다 : " + buttonIndex);
			return;
		}

		if (buttonIndex >= 0 && buttonIndex < payGoldList.Count)
		{
			int payGold = payGoldList[buttonIndex];

			// 소지 금액 확인
			if (goodsManager.CanSpend(GoodsType.Gold, payGold))
			{
				// 건설 비용 지불
				goodsManager.SpendGoods(GoodsType.Gold, payGold);

				// 건설 패널 비활성화
				buildingPanel.SetActive(false);

				if (buttonIndex == 0 || buttonIndex == 4 || buttonIndex == 6)
				{
					StartCoroutine(BuildingSequence(buttonIndex)); // 연출 시작
				}
				else
				{
					RequestSpawnBuilding(buttonIndex); // 연출 없이 즉시 건설
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

		/* 연출용 원숭이 초기화 */
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

		// 연출용 원숭이를 활성화 후 초기화합니다.
		showcaseMonkey.gameObject.SetActive(true);
		InitMonkey(showcaseMonkey, selectedType);

		// 건설 없이 건물 데이터만 저장
		RequestSpawnBuilding(buttonIndex, false);

		yield return new WaitForSeconds(10f); // 연출 지연

		// TODO BuildingSpawnManager 에서 처리
		// 원숭이 타입
		voyageDataManager.MonkeyType = selectedType;

		SceneManager.LoadScene("VoyageTest"); // 샘플 씬 로드
	}

	private void RequestSpawnBuilding(int buttonIndex, bool spawnImmediately = true)
	{
		// 유효성 검사
		if (placementManager is null) return;

		// 건설되지 않은 경우에만 실행
		if (buildingManager.IsBuildingAlreadyExist(buttonIndex)) return;

		// 건설 요청
		placementManager.RequestSpawnBuilding(buttonIndex, spawnImmediately);

		UpdateBuildingUI(buttonIndex);
	}

	private void UpdateBuildingUI(int buttonIndex)
	{
		buildingButtons[buttonIndex].gameObject.SetActive(false); // 해당 건물 버튼 비활성화
		finList[buttonIndex].SetActive(true); // 완료된 건물 UI 활성화
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
}
