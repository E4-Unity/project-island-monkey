using UnityEngine;

namespace IslandMonkey
{
	public class SetBananahouseMonkey : MonoBehaviour
	{
		[SerializeField] BuildingMonkey buildingMonkeyPrefab; // BuildingMonkey 프리팹을 연결합니다.

		private void Start()
		{
			// BuildingMonkey 프리팹을 인스턴스화하고 SetBuilding 메서드를 호출하여 설정합니다.
			BuildingMonkey buildingMonkeyInstance = Instantiate(buildingMonkeyPrefab);
			BuildingMonkey.IBuilding bananahouse = GetComponent<BuildingMonkey.IBuilding>();
			if (bananahouse != null)
			{
				buildingMonkeyInstance.SetBuilding(bananahouse);
			}
			else
			{
				Debug.LogError("Bananahouse 또는 적절한 IBuilding 인터페이스를 가진 오브젝트를 찾을 수 없습니다.");
			}
		}
	}
}
