using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float maxX = 5f;
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxZ = 11f;
    [SerializeField] private float minZ = 5f;
    [SerializeField] private float zoomSpeed = 10000000f;
    [SerializeField] private float panSpeed = 20f;

    private Camera camera;

    void OnDrawGizmos()
    {
        float height = transform.position.y;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(minX, height, maxZ), new Vector3(maxX, height, maxZ));
        Gizmos.DrawLine(new Vector3(maxX, height, maxZ), new Vector3(maxX, height, minZ));
        Gizmos.DrawLine(new Vector3(maxX, height, minZ), new Vector3(minX, height, minZ));
        Gizmos.DrawLine(new Vector3(minX, height, maxZ), new Vector3(minX, height,minZ));
  
    }

    private void Start()
    {
        camera = GetComponent<Camera>();

        /*// 마우스 드래그를 통한 카메라 이동
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButton(0))
            .Subscribe(_ =>
            {
                Vector3 pos = transform.position;

                pos.x -= Input.GetAxis("Mouse X") * panSpeed * camera.orthographicSize * Time.deltaTime;
                pos.z -= Input.GetAxis("Mouse Y") * panSpeed * camera.orthographicSize * Time.deltaTime;

                // 카메라 이동 범위 제한
                pos.x = Mathf.Clamp(pos.x, minX, maxX);
                pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

                transform.position = pos;
            })
            .AddTo(this);

        // 줌
        this.UpdateAsObservable()
            .Where(_ => Input.touchCount > 1 || Input.mouseScrollDelta.y != 0f) // 터치 또는 마우스 휠 입력 확인
            .Subscribe(_ =>
            {
                float zoomInput = Input.mouseScrollDelta.y; // 마우스 휠 입력 값
                if (Input.touchCount > 1)
                {
                    // 터치 입력 처리
                    // 터치 저장
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    // 위치 저장
                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    // 거리 계산
                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    // 줌 땡기기
                    zoomInput = prevTouchDeltaMag - touchDeltaMag;
                }

                // zoomInput 크기를 조절 (예: 2배 증가)
                zoomInput = zoomInput * 2.0f;

                // 카메라 사이즈
                float newSize = camera.orthographicSize - (zoomInput * zoomSpeed);
                newSize = Mathf.Max(newSize, 1f);
                newSize = Mathf.Min(newSize, 6f);
                camera.orthographicSize = newSize;
            })
            .AddTo(this);*/
		//  UniRx 제거하고 다른 기능 하숑하기 
    }
}
