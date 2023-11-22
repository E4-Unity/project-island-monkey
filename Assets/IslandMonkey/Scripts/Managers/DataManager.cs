using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	public static DataManager Instance { get; private set; }

	private const string BuildingDataFileName = "buildingData.json";
	private string BuildingDataPath => Path.Combine(Application.persistentDataPath, BuildingDataFileName);

	public List<BuildingData> Buildings { get; private set; } = new List<BuildingData>();

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void SaveBuildingData()
	{
		string json = JsonUtility.ToJson(new SerializableList<BuildingData>(Buildings), true);
		File.WriteAllText(BuildingDataPath, json);
		Debug.Log("Building data saved.");
	}

	public void LoadBuildingData()
	{
		if (File.Exists(BuildingDataPath))
		{
			string json = File.ReadAllText(BuildingDataPath);
			SerializableList<BuildingData> data = JsonUtility.FromJson<SerializableList<BuildingData>>(json);
			Buildings = data.list;
			Debug.Log("Building data loaded.");
		}
		else
		{
			Debug.LogWarning("Building data file not found.");
		}
	}

	public void DeleteBuildingData()
	{
		if (File.Exists(BuildingDataPath))
		{
			File.Delete(BuildingDataPath);
			Buildings.Clear();
			Debug.Log("Building data deleted.");
		}
		else
		{
			Debug.LogWarning("Building data file not found.");
		}
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
}
