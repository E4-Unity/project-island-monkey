using UnityEngine;
using UnityEngine.EventSystems;

public class TestUpgradeUI : MonoBehaviour
{
	[SerializeField] private GameObject upgradePool;
	[SerializeField] private GameObject upgradeBuilding;
	[SerializeField] private GameObject upgradeMonkey;
	[SerializeField] private GameObject upgradeBuildingPanel;

	[SerializeField] private GameObject MonkeyBankPopupPanel;
	[SerializeField] private GameObject DrawMachinePanel;

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
			else if (hitObject.name == "Island LuckyDraw")
			{
				DrawMachinePanel.SetActive(true);
			}
			else
			{
				// 아무것도 클릭되지 않았을 때 모든 패널을 비활성화합니다.
				upgradePool.SetActive(false);
				upgradeBuildingPanel.SetActive(false);
				MonkeyBankPopupPanel.SetActive(false);
				DrawMachinePanel.SetActive(false);
			}
		}
		else
		{
			Debug.Log("No hit"); // 레이캐스트가 아무것도 맞추지 않았을 때 로그로 출력
								 // 레이캐스트가 아무것도 맞추지 않았을 때 모든 패널을 비활성화합니다.
			upgradePool.SetActive(false);
			upgradeBuildingPanel.SetActive(false);
			MonkeyBankPopupPanel.SetActive(false);
			DrawMachinePanel.SetActive(false);
		}
	}
}
