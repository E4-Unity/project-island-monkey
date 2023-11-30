using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
	// 이펙트 유형을 정의하는 enum
	public enum EffectType
	{
		Smoke_02,
		FX_Desktop_Element_Smoke_01,
		Smoke_01,
		Smoke_10,
		Smoke_08,
		FX_Mobile_Element_Buff_Base_00,
		Flash_star_ellow_green,
		Smoke_09,
		Smoke_05,
		BuildEffect
	}

	[Serializable]
	public class SceneEffect
	{
		public EffectType effectType; // enum 타입으로 변경
		public GameObject effectPrefab; // 이펙트 프리팹
	}

	public List<SceneEffect> mainSceneEffects; // 메인씬 이펙트 리스트
	public List<SceneEffect> voyageSceneEffects; // 항해씬 이펙트 리스트

	private Dictionary<EffectType, GameObject> effects = new Dictionary<EffectType, GameObject>();

	private void Awake()
	{
		InitializeEffects(mainSceneEffects);
		InitializeEffects(voyageSceneEffects);
	}

	private void InitializeEffects(List<SceneEffect> effectList)
	{
		foreach (var effect in effectList)
		{
			if(!effect.effectPrefab) continue;

			effects.TryAdd(effect.effectType, effect.effectPrefab);
		}
	}

	public void PlayEffect(EffectType effectType, Vector3 position)
	{
		if (effects.TryGetValue(effectType, out GameObject effectPrefab))
		{
			Instantiate(effectPrefab, position, Quaternion.identity);
		}
		else
		{
#if UNITY_EDITOR
			Debug.LogWarning("이 이펙트에 오류: " + effectType);
#endif
		}
	}
}
