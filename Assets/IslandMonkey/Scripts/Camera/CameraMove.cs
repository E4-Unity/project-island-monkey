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

	private Vector3 oldPosition;

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			oldPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		}

		if (Input.GetMouseButton(0))
		{
			Vector3 newPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
			Vector3 direction = oldPosition - newPosition;

			// 드래그 한계
			float newX = Mathf.Clamp(transform.position.x + direction.x * dragSpeed, dragLimitX.x, dragLimitX.y);
			float newY = Mathf.Clamp(transform.position.y + direction.y * dragSpeed, dragLimitY.x, dragLimitY.y);

			transform.position = new Vector3(newX, newY, transform.position.z);

			oldPosition = newPosition;
		}

		// 스크롤에 의한 카메라 줌 처리
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		Camera.main.orthographicSize -= scroll * zoomSpeed;
		// 줌 한계
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
	}
}
