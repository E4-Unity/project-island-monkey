using System;
using System.Collections.Generic;
using E4.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoSingleton<SoundManager>
{
	public AudioSource Main_BGM;
	public AudioSource Additional_BGM;
	public AudioClip voyageWaveClip;

	[Serializable]
	public class SceneBGM
	{
		public string sceneName;
		public AudioClip bgm;
		public bool playAdditionalSound;
	}

	public List<SceneBGM> sceneBGMs;


	[Serializable]
	public class SceneSoundEffect
	{
		public string soundName;
		public AudioClip clip;
	}

	public List<SceneSoundEffect> mainSceneSoundEffects;
	public List<SceneSoundEffect> voyageSceneSoundEffects;

	private Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();

	/* MonoSingleton */
	protected override void InitializeComponent()
	{
		base.InitializeComponent();

		SceneManager.sceneLoaded += OnSceneLoaded;

		Main_BGM = GetComponent<AudioSource>();
		Additional_BGM = gameObject.AddComponent<AudioSource>();
		Additional_BGM.clip = voyageWaveClip;

		InitializeSoundEffects(mainSceneSoundEffects);
		InitializeSoundEffects(voyageSceneSoundEffects);

		// 볼륨 설정 초기화
		Main_BGM.volume = PlayerPrefs.GetFloat("MainBGMVolume", 1f);
		Additional_BGM.volume = PlayerPrefs.GetFloat("AdditionalBGMVolume", 1f);
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
				Main_BGM.clip = sceneBGM.bgm;
				Main_BGM.loop = true;
				Main_BGM.Play();

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
			Debug.LogWarning("사운드 이펙트를 찾을 수 없습니다: " + soundName);
		}
	}

	// 메인 BGM 볼륨 조절 메소드
	public void SetMainBGMVolume(float volume)
	{
		Main_BGM.volume = volume;
		PlayerPrefs.SetFloat("MainBGMVolume", volume);
		PlayerPrefs.Save();
	}

	// 추가 BGM 볼륨 조절 메소드
	public void SetAdditionalBGMVolume(float volume)
	{
		Additional_BGM.volume = volume;
		PlayerPrefs.SetFloat("AdditionalBGMVolume", volume);
		PlayerPrefs.Save();
	}
}
