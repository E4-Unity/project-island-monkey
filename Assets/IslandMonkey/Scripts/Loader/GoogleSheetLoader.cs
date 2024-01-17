using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;


[Serializable]
public class KeyValue
{
	public string key;
	public string value;
}

[Serializable]
public class RowData
{
	public List<KeyValue> data;
}

public class GoogleSheetLoader : MonoBehaviour
{
	[SerializeField]
	private string spreadsheetId = "1_R4-o-P6swv44UDCL7eU7D_YrijLA_OV5EGtDQvHnpA";

	[SerializeField]
	private string sheetName = "facility_list";

	[SerializeField]
	private string apiKey = "AIzaSyAvMJ301rf4TgoZ2hT0H3CfLCrPpW_9ioE";

	[SerializeField]
	private List<RowData> dataList = new List<RowData>();

	[SerializeField]
	private List<string> keyList = new List<string>();

	void Start()
	{
		StartCoroutine(LoadGoogleSheetData());
	}

	public IEnumerator LoadGoogleSheetData()
	{
		string url = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}/values/{sheetName}?key={apiKey}";
		UnityWebRequest www = UnityWebRequest.Get(url);

		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("Failed to load Google Spreadsheet data: " + www.error);
		}
		else
		{
			// Parse the JSON response
			JObject response = JObject.Parse(www.downloadHandler.text);
			JArray rows = response["values"] as JArray;
			JArray headerRow = rows[0] as JArray;

			// Iterate through the header row to initialize dictionary keys
			foreach (var key in headerRow)
			{
				keyList.Add(key.ToString());
			}
			for (int i = 1; i < rows.Count; i++)
			{
				JArray rowData = rows[i] as JArray;
				var row = new RowData();
				row.data = new List<KeyValue>();  // 초기화 부분

				for (int j = 0; j < headerRow.Count; j++)
				{
					var keyVal = new KeyValue();
					keyVal.key = keyList[j];
					keyVal.value = rowData[j].ToString();
					row.data.Add(keyVal);
				}
				dataList.Add(row);
			}
		}
	}

	public List<RowData> GetRowDataList()
	{
		return dataList;
	}
}
