using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Panel : MonoBehaviour
{
	public UnityEvent onEnableEvent;
	public UnityEvent onDisableEvent;

	// 이 메소드는 패널의 활성화 상태를 토글합니다.
	public virtual void React()
	{
		gameObject.SetActive(!gameObject.activeSelf);
	}

	// 패널이 활성화될 때 호출됩니다.
	void OnEnable()
	{
		onEnableEvent.Invoke();
		ToggleCameraControl(false); // 카메라 움직임을 막습니다.
	}

	// 패널이 비활성화될 때 호출됩니다.
	void OnDisable()
	{
		onDisableEvent.Invoke();
		ToggleCameraControl(true); // 카메라 움직임을 다시 허용합니다.
	}

	// 카메라의 움직임을 토글하는 메소드입니다.
	void ToggleCameraControl(bool enable)
	{
		CameraMove cameraMove = Camera.main?.GetComponent<CameraMove>();
		if (cameraMove is null) return;

		cameraMove.enabled = enable;
		cameraMove.canMoveCamera = enable;
	}
}
