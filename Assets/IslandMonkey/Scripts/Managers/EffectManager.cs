using System;
using System.Collections.Generic;
using E4.Utilities;
using UnityEngine;

public class EffectManager : MonoSingleton<EffectManager>
{
	// 이펙트 유형을 정의하는 enum
	public enum EffectType
	{
		Smoke_01_2D,
		Smoke_02_2D,
		Smoke_10_2D,
		Smoke_01,
		Smoke_02,
		Smoke_05,
		Smoke_08,
		Smoke_09,
		FX_Desktop_Element_Smoke_01,
		Flash_star_ellow_green,
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

	/* MonoSingleton */
	protected override void InitializeComponent()
	{
		base.InitializeComponent();

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

	public void PlayUIEffect(EffectManager.EffectType effectType, Vector3 position)
	{
		if (effects.TryGetValue(effectType, out GameObject effectPrefab))
		{
			GameObject effectInstance = Instantiate(effectPrefab, position, Quaternion.identity);
			effectInstance.layer = LayerMask.NameToLayer("UI"); // "UI" 레이어로 변경
		}
		else
		{
			Debug.LogWarning("이 이펙트에 오류: " + effectType);
		}
	}

}
