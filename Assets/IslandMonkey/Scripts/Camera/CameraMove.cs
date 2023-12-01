using UnityEngine;

public class CameraMove : MonoBehaviour
{
	[SerializeField] private float dragSpeed = 0.01f;
	[SerializeField] private float zoomSpeed = 0.05f;
	[SerializeField] private float minZoom = 5f;
	[SerializeField] private float maxZoom = 15f;
	[SerializeField] private Vector2 dragLimitX = new Vector2(-6f, 1f);
	[SerializeField] private Vector2 dragLimitY = new Vector2(6f, 7f);

	private Vector3 dragOrigin;
	private Camera cam;
	private float lastTouchDistance;

	private void Awake()
	{
		cam = Camera.main;
	}

	void Update()
	{
		HandleInput();
		HandleZoom();
	}

	private void HandleInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
		}

		if (Input.GetMouseButton(0))
		{
			Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
			cam.transform.position = ClampCameraPosition(cam.transform.position + difference);
		}
	}

	private void HandleZoom()
	{
		// PC에서는 마우스 스크롤 휠로 줌 처리
#if UNITY_EDITOR || UNITY_STANDALONE
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * (zoomSpeed * 1000), minZoom, maxZoom);
#endif

		// 안드로이드에서는 멀티터치 핀치 줌으로 줌 처리
#if UNITY_ANDROID
		if (Input.touchCount == 2)
		{
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			// 각 터치의 이전 프레임에서의 위치를 찾습니다.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// 각 프레임 간의 벡터(거리)의 크기를 구합니다.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// 두 벡터 간의 거리 차이를 구합니다.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			// 카메라 줌을 변경합니다.
			cam.orthographicSize += deltaMagnitudeDiff * zoomSpeed;
			cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
		}
#endif
	}

	private Vector3 ClampCameraPosition(Vector3 targetPosition)
	{
		float clampedX = Mathf.Clamp(targetPosition.x, dragLimitX.x, dragLimitX.y);
		float clampedY = Mathf.Clamp(targetPosition.y, dragLimitY.x, dragLimitY.y);

		return new Vector3(clampedX, clampedY, targetPosition.z);
	}
}
