using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	[SerializeField] private float dragSpeed = 0.01f;
	[SerializeField] private float zoomSpeed = 1f;
	[SerializeField] private float minZoom = 5f;
	[SerializeField] private float maxZoom = 15f;
	[SerializeField] private Vector2 dragLimitX = new Vector2(-5f, 5f);
	[SerializeField] private Vector2 dragLimitY = new Vector2(-5f, 5f);

	private Vector3 dragOrigin;

	void Update()
	{
		// PC에서의 마우스 드래그
		if (Input.GetMouseButtonDown(0))
		{
			dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
		if (Input.GetMouseButton(0))
		{
			Vector3 difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Camera.main.transform.position = ClampCameraPosition(Camera.main.transform.position + difference);
		}

		// 모바일 터치 드래그
		if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Moved)
			{
				Vector3 touchDeltaPosition = (Vector3)touch.deltaPosition * dragSpeed;
				Camera.main.transform.position = ClampCameraPosition(Camera.main.transform.position - touchDeltaPosition);
			}
		}

		// 스크롤에 의한 카메라 줌 처리 (PC)
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
	}

	private Vector3 ClampCameraPosition(Vector3 targetPosition)
	{
		float clampedX = Mathf.Clamp(targetPosition.x, dragLimitX.x, dragLimitX.y);
		float clampedY = Mathf.Clamp(targetPosition.y, dragLimitY.x, dragLimitY.y);

		return new Vector3(clampedX, clampedY, targetPosition.z);
	}
}
