#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector(); // 기본 인스펙터를 그립니다.

		DataManager dataManager = (DataManager)target; // 현재 선택된 DataManager 오브젝트를 가져옵니다.

		// 저장 버튼
		if (GUILayout.Button("Save Buildings Data"))
		{
			dataManager.SaveBuildingData();
		}

		// 로드 버튼
		if (GUILayout.Button("Load Buildings Data"))
		{
			dataManager.LoadBuildingData();
		}

		// 삭제 버튼
		if (GUILayout.Button("Delete Buildings Data"))
		{
			dataManager.DeleteBuildingData();
		}
	}
}
#endif
