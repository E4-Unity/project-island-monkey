using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{
	public AudioSource Main_BGM;
	public AudioSource Additional_BGM; // 추가 사운드 재생용 AudioSource

	[Serializable]
	public class SceneBGM
	{
		public string sceneName;
		public AudioClip bgm;
		public bool playAdditionalSound; // 추가 사운드 재생 여부
	}

	public List<SceneBGM> sceneBGMs;
	public AudioClip voyageWaveClip; // 인스펙터에서 할당할 Voyage_WAVE 클립

	[Serializable]
	public class SceneSoundEffect
	{
		public string soundName;
		public AudioClip clip;
	}

	public List<SceneSoundEffect> mainSceneSoundEffects; // 메인씬 효과음 리스트
	public List<SceneSoundEffect> voyageSceneSoundEffects; // 항해씬 효과음 리스트

	private Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();


	private void Awake()
	{
		DontDestroyOnLoad(gameObject); // 씬 전환 시 인스턴스 유지
		SceneManager.sceneLoaded += OnSceneLoaded;

		Additional_BGM.clip = voyageWaveClip; // Voyage_WAVE 사운드 설정

		InitializeSoundEffects(mainSceneSoundEffects);
		InitializeSoundEffects(voyageSceneSoundEffects);
	}

	private void InitializeSoundEffects(List<SceneSoundEffect> soundEffectList)
	{
		foreach (var soundEffect in soundEffectList)
		{
			if (!soundEffects.ContainsKey(soundEffect.soundName))
			{
				soundEffects[soundEffect.soundName] = soundEffect.clip;
			}
		}
	}


	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		foreach (var sceneBGM in sceneBGMs)
		{
			if (sceneBGM.sceneName == scene.name)
			{
				// 메인 배경음악 재생
				Main_BGM.clip = sceneBGM.bgm;
				Main_BGM.loop = true;
				Main_BGM.Play();

				// 추가 사운드 재생 여부 확인 및 처리
				if (sceneBGM.playAdditionalSound)
				{
					Additional_BGM.loop = true;
					Additional_BGM.Play();
				}
				else
				{
					Additional_BGM.Stop();
				}

				break;
			}
		}
	}
	public void PlaySoundEffect(string soundName)
	{
		if (soundEffects.TryGetValue(soundName, out AudioClip clip))
		{
			AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
		}
		else
		{
			Debug.LogWarning("이 사운드 이펙트에 오류: " + soundName);
		}
	}
}
