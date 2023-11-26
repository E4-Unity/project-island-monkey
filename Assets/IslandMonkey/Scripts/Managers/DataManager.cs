using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IslandMonkey
{
	public class DataManager
	{
		public interface ISavable<out T> where T : class
		{
			string FileName { get; }
			T Data { get; }
		}

		static string DefaultDataPath = Application.persistentDataPath;

		public static void SaveData<T>(ISavable<T> target) where T : class => SaveData(target, DefaultDataPath);

		public static void SaveData<T>(ISavable<T> target, string filePath) where T : class
		{
			if (target is null) return;

			var path = Path.Combine(filePath, target.FileName);

			var json = JsonUtility.ToJson(target.Data, true);
			File.WriteAllText(path, json);

#if UNITY_EDITOR
			Debug.Log("Data Saved At " + path);
#endif
		}

		public static T LoadData<T>(ISavable<T> target) where T : class => LoadData<T>(target, DefaultDataPath);

		public static T LoadData<T>(ISavable<T> target, string filePath) where T : class
		{
			if (target is null) return null;

			var path = Path.Combine(filePath, target.FileName);
			if (!File.Exists(path)) return null;

#if UNITY_EDITOR
			Debug.Log("Data Loaded From " + path);
#endif

			var json = File.ReadAllText(path);
			return JsonUtility.FromJson<T>(json);
		}

		public static void DeleteData<T>(ISavable<T> target, string filePath) where T : class
		{
			if (target is null) return;

			var path = Path.Combine(filePath, target.FileName);
			if (!File.Exists(path)) return;

			File.Delete(path);

#if UNITY_EDITOR
			Debug.Log("Data Deleted At " + path);
#endif
		}

		public static void DeleteAllData()
		{
			if (!Directory.Exists(DefaultDataPath)) return;

			var files = Directory.GetFiles(DefaultDataPath);

			foreach (var file in files)
			{
				File.Delete(Path.Combine(DefaultDataPath, file));
			}

#if UNITY_EDITOR
			Debug.Log("All Data Deleted At " + DefaultDataPath);
#endif
		}
	}
}
