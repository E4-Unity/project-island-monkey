using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmVolume : MonoBehaviour
{
    public AudioSource audioSource;  // 오디오 소스
    public Slider volumeSlider;     // 볼륨을 조절할 슬라이더

    // Start is called before the first frame update
    void Start()
    {
        // 초기 볼륨 설정
        volumeSlider.value = audioSource.volume;

        // 슬라이더 이벤트 추가
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    // 슬라이더 값이 변경될 때 호출되는 함수
    public void ChangeVolume(float newVolume)
    {
        audioSource.volume = newVolume;
    }
}
