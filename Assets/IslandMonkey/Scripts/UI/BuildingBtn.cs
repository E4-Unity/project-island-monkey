using System.Collections;
using System.Collections.Generic;
using IslandMonkey;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBtn : MonoBehaviour
{
	[SerializeField] private List<Button> buildingButtons;
	[SerializeField] private List<int> payGoldList;
	[SerializeField] private List<GameObject> monkeyPrefabs; // 원숭이 프리팹 리스트
	private GameObject spawnedMonkey; // 스폰된 원숭이를 저장할 변수

	public HexagonalPlacementManager placementManager;

	[SerializeField] private GameObject getAnimalPanel; // 연출을 적용할 UI 패널에 대한 참조
	[SerializeField] private GameObject buildingPanel; // 연출을 적용할 UI 패널에 대한 참조


	void Start()
	{
		// 모든 버튼에 대해 OnClick 이벤트에 리스너를 추가
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

			var gameManager = GameManager.instance;
			bool hasEnoughGold = CanSpendGold(in payGold);
			SpendGold(in payGold);

			if (hasEnoughGold && placementManager != null)
			{
				StartCoroutine(BuildingSequence(buttonIndex)); // 건설 연출 시작
			}
			else
			{
				// 골드가 부족하면 여기서 건물 건설을 하지 않음
			}
		}
		else
		{
			Debug.LogWarning("Button index is out of range for payGoldList");
		}
	}

	// 건물 건설 연출 코루틴
	IEnumerator BuildingSequence(int buttonIndex)
	{
		getAnimalPanel.SetActive(true);
		buildingPanel.SetActive(false);

		// 원숭이 스폰 로직
		if (buttonIndex >= 0 && buttonIndex < monkeyPrefabs.Count)
		{
			// 이전에 스폰된 원숭이가 있다면 제거
			if (spawnedMonkey != null)
				Destroy(spawnedMonkey);

			// 원숭이 프리팹 스폰
			spawnedMonkey = Instantiate(monkeyPrefabs[buttonIndex], new Vector3(0, 0.025f, -1), Quaternion.identity);
		}

		yield return new WaitForSeconds(10f); // 연출 지연

		getAnimalPanel.SetActive(false);

		// 건물 건설
		placementManager.SpawnBuilding();

		// 원숭이 제거
		if (spawnedMonkey != null)
			Destroy(spawnedMonkey);
	}

	bool CanSpendGold(in int amount) => GameManager.instance.CanSpend(GoodsType.Gold, amount);
	void SpendGold(in int amount) => GameManager.instance.SpendGoods(GoodsType.Gold, amount);
}
