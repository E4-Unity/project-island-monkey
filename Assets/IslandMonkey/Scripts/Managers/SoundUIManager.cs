using UnityEngine;
using UnityEngine.UI;

public class SoundUIManager : MonoBehaviour
{
	public Slider mainBGMSlider;
	public Slider additionalBGMSlider;
	private SoundManager soundManager;

	void Start()
	{
		soundManager = SoundManager.instance; // SoundManager의 인스턴스를 가져옴

		// 슬라이더 초기화
		mainBGMSlider.value = PlayerPrefs.GetFloat("MainBGMVolume", 1f);
		additionalBGMSlider.value = PlayerPrefs.GetFloat("AdditionalBGMVolume", 1f);

		// 슬라이더의 OnValueChanged 이벤트에 메소드 연결
		mainBGMSlider.onValueChanged.AddListener(HandleMainBGMVolumeChange);
		additionalBGMSlider.onValueChanged.AddListener(HandleAdditionalBGMVolumeChange);
	}

	// 메인 BGM 볼륨 조절 메소드
	private void HandleMainBGMVolumeChange(float volume)
	{
		soundManager.SetMainBGMVolume(volume);
	}

	// 추가 BGM 볼륨 조절 메소드
	private void HandleAdditionalBGMVolumeChange(float volume)
	{
		soundManager.SetAdditionalBGMVolume(volume);
	}
}
