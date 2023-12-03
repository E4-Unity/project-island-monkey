#if UNITY_EDITOR
using IslandMonkey;
using UnityEditor;

[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor
{
	[MenuItem("DataManager/Delete/All")]
	public static void DeleteAll()
	{
		DataManager.DeleteAllData();
	}

	[MenuItem("DataManager/Delete/BuildingData")]
	public static void DeleteBuildingData()
	{
		DataManager.DeleteData(BuildingManager.SaveFileName);
	}

	[MenuItem("DataManager/Delete/GoodsData")]
	public static void DeleteGoodsData()
	{
		DataManager.DeleteData(GoodsManager.SaveFileName);
	}

	[MenuItem("DataManager/Delete/VoyageData")]
	public static void DeleteVoyageData()
	{
		DataManager.DeleteData(VoyageDataManager.SaveFileName);
	}

	[MenuItem("DataManager/Delete/MonkeyBankData")]
	public static void DeleteMonkeyBankData()
	{
		DataManager.DeleteData(MonkeyBank.SaveFileName);
	}
}
#endif
