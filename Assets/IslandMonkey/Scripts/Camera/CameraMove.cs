using UnityEngine;

public class CameraMove : MonoBehaviour
{
	[SerializeField] private float zoomSpeed = 0.05f;
	[SerializeField] private float minZoom = 5f;
	[SerializeField] private float maxZoom = 15f;
	[SerializeField] private Vector2 dragLimitX = new Vector2(-6f, 1f);
	[SerializeField] private Vector2 dragLimitY = new Vector2(6f, 7f);

	public bool canMoveCamera = true;

	private Camera cam;
	private Vector3 dragOrigin;

	private void Awake()
	{
		cam = Camera.main;
	}

	void Update()
	{
		if (canMoveCamera)
		{
			HandleInput();
		}
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

		// PC에서는 마우스 스크롤 휠로 줌 처리
#if UNITY_EDITOR || UNITY_STANDALONE
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		HandleZoom(scroll);
#endif

		// 안드로이드에서는 멀티터치 핀치 줌으로 줌 처리
#if UNITY_ANDROID
		if (Input.touchCount == 2)
		{
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			HandleZoom(deltaMagnitudeDiff);
		}
#endif
	}

	private void HandleZoom(float deltaMagnitudeDiff)
	{
		float newOrthoSize = Mathf.Clamp(cam.orthographicSize + deltaMagnitudeDiff * zoomSpeed, minZoom, maxZoom);
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newOrthoSize, Time.deltaTime * zoomSpeed * 5f);
	}

	private Vector3 ClampCameraPosition(Vector3 targetPosition)
	{
		float clampedX = Mathf.Clamp(targetPosition.x, dragLimitX.x, dragLimitX.y);
		float clampedY = Mathf.Clamp(targetPosition.y, dragLimitY.x, dragLimitY.y);

		return new Vector3(clampedX, clampedY, targetPosition.z);
	}
}
