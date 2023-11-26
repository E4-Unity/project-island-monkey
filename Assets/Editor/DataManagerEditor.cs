#if UNITY_EDITOR
using IslandMonkey;
using UnityEditor;

[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor
{
	[MenuItem("DataManager/Delete All")]
	public static void DeleteAll()
	{
		DataManager.DeleteAllData();
	}
}
#endif
