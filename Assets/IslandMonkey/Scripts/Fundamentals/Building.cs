using DG.Tweening; // DOTween 애니메이션 라이브러리
using TMPro; // 텍스트 메시 프로 사용
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems; // 이벤트 시스템 사용

public class Building : MonoBehaviour
{

	public GameObject model; // 건물 모델
	public UnityEvent onBuildEvent; // 건물이 지어졌을 때 발생할 이벤트

	public GameObject iconBuilding; // 건물 아이콘
	public GameObject iconMonkey; // 원숭이 아이콘
	public GameObject buildingUpgradePanel; // 건물 업그레이드 패널

	public bool isPlaced = false; // 건물이 배치되었는지 여부


	protected virtual void Start()
	{
		iconBuilding.SetActive(false); // 건물 아이콘 비활성화
		iconMonkey.SetActive(false); // 원숭이 아이콘 비활성화
	}



	// 건물 모델을 압박하는 애니메이션
	public void SqueezeModel()
	{
		model.transform.DOShakeScale(0.2f, Vector3.one * 0.02f);
	}

	// 건물 배치 로직
	public void SetPlace(Place place)
	{
		// 건물의 새 위치를 설정하는 로직
		isPlaced = true; // 건물이 배치되었다고 상태 변경
	}

	// 골드 획득 애니메이션
	public void AnimateGoldAcquisition(Vector3 startPos)
	{
		// 골드 획득 애니메이션 로직
	}

	// 아이콘 활성화
	public virtual void ActivateIcons()
	{
		if (iconBuilding != null) iconBuilding.SetActive(true);
		if (iconMonkey != null) iconMonkey.SetActive(true);
	}

	// 아이콘 비활성화
	public virtual void DeactivateIcons()
	{
		if (iconBuilding != null) iconBuilding.SetActive(false);
		if (iconMonkey != null) iconMonkey.SetActive(false);
	}

	// 추가로 필요한 메서드들을 여기에 구현합니다.
}
