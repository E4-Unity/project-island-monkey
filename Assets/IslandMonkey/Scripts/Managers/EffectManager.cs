using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
	[Serializable]
	public class SceneEffect
	{
		public string effectName;
		public GameObject effectPrefab; // 이펙트 프리팹
	}

	public List<SceneEffect> mainSceneEffects; // 메인씬 이펙트 리스트
	public List<SceneEffect> voyageSceneEffects; // 항해씬 이펙트 리스트

	private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

	private void Awake()
	{
		InitializeEffects(mainSceneEffects);
		InitializeEffects(voyageSceneEffects);
	}

	private void InitializeEffects(List<SceneEffect> effectList)
	{
		foreach (var effect in effectList)
		{
			if (!effects.ContainsKey(effect.effectName))
			{
				effects[effect.effectName] = effect.effectPrefab;
			}
		}
	}

	public void PlayEffect(string effectName, Vector3 position)
	{
		if (effects.TryGetValue(effectName, out GameObject effectPrefab))
		{
			Instantiate(effectPrefab, position, Quaternion.identity);
		}
		else
		{
			Debug.LogWarning("이 이펙트에 오류: " + effectName);
		}
	}
}
