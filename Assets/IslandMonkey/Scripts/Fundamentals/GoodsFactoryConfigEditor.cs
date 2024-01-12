#if UNITY_EDITOR
using IslandMonkey;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GoodsFactoryConfig))]
public class GoodsFactoryConfigEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GoodsFactoryConfig myScript = (GoodsFactoryConfig)target;

		if (GUILayout.Button("Restore Defaults Level"))
		{
			myScript.RestoreDefaultsLevel();
		}
	}
}

#endif
