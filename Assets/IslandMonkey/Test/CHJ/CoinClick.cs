using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems; // 이벤트 처리를 위한 네임스페이스

public class CoinClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject coinPrefab; // 코인 프리팹
    public Transform targetPosition; // 코인이 이동할 목표 위치
    public int numberOfCoins = 10; // 생성할 코인의 수
    public float speed = 1.0f; // 코인 이동 속도

    // 클릭 이벤트가 발생했을 때 호출될 메서드
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(SpawnAndMoveCoins());
    }

    // 코인을 생성하고 목표 위치까지 이동시키는 코루틴
    IEnumerator SpawnAndMoveCoins()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            // 코인 인스턴스 생성
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

            // 코인 이동
            StartCoroutine(MoveCoin(coin, targetPosition.position));
            yield return new WaitForSeconds(0.1f); // 다음 코인 생성 전에 잠시 대기
        }
    }

    // 코인을 목표 위치까지 이동시키는 코루틴
    IEnumerator MoveCoin(GameObject coin, Vector3 target)
    {
        while (Vector3.Distance(coin.transform.position, target) > 0.01f)
        {
            // 코인을 목표 위치까지 점진적으로 이동
            coin.transform.position = Vector3.MoveTowards(coin.transform.position, target, speed * Time.deltaTime);
            yield return null; // 다음 프레임까지 대기
        }

        Destroy(coin); // 목표 위치에 도달하면 코인 파괴
    }
}