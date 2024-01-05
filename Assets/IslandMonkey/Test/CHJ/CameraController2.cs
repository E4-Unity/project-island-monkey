using UnityEngine;
using Cinemachine;

public class CameraController2 : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera virtualCamera;
	[SerializeField] private float dragSpeed = 1f;
	[SerializeField] private float zoomSpeed = 1f;
	[SerializeField] private float minZoom = 1f;
	[SerializeField] private float maxZoom = 2.5f;
	[SerializeField] private Vector2 dragLimitX = new Vector2(-6f, 10f);
	[SerializeField] private Vector2 dragLimitY = new Vector2(-6f, 8.5f);

	private Vector3 dragOrigin;
	private float initialOrthographicSize;
	private Camera cam;
	private bool isDragging = false;
	private Vector3 lastMousePosition;

	private void Awake()
	{
		cam = Camera.main;
		initialOrthographicSize = virtualCamera.m_Lens.OrthographicSize;
	}



	private void Update()
	{
		HandleZoom();

		if (Input.GetMouseButtonDown(0))
		{
			// 마우스 버튼을 처음 누르면 드래그 시작
			dragOrigin = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));
			isDragging = true;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			// 마우스 버튼을 뗄 때 드래그 중단
			isDragging = false;
		}

		if (isDragging && Input.GetMouseButton(0))
		{
			HandleDrag();
		}
	}

	private void StartDrag()
	{
		isDragging = true;
		dragOrigin = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
	}

	private void HandleDrag()
	{
		Vector3 currentMousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));
		Vector3 difference = currentMousePosition - dragOrigin;
		dragOrigin = currentMousePosition; // 드래그 원점을 현재 마우스 위치로 계속 업데이트합니다.

		Vector3 newPosition = virtualCamera.transform.position - difference * dragSpeed;
		virtualCamera.transform.position = ClampCameraPosition(newPosition);
	}

	private void HandleInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			dragOrigin = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
		}

		if (Input.GetMouseButton(0))
		{
			Vector3 currentMousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
			Vector3 difference = currentMousePosition - dragOrigin;
			dragOrigin = currentMousePosition; // Update drag origin to the current position

			Vector3 newPosition = virtualCamera.transform.position - difference * dragSpeed;
			virtualCamera.transform.position = ClampCameraPosition(newPosition);
		}
	}

	private void HandleZoom()
	{
#if UNITY_EDITOR || UNITY_STANDALONE
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (Mathf.Abs(scroll) > 0f)
		{
			Debug.Log($"Scroll detected: {scroll}"); // 스크롤 감지 로그
			float newSize = virtualCamera.m_Lens.OrthographicSize - scroll * zoomSpeed;
			virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
			Debug.Log($"New Orthographic Size: {virtualCamera.m_Lens.OrthographicSize}"); // 새 Orthographic Size 로그
		}
#endif

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
