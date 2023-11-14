using System.Collections;
using UnityEngine;

public class GoldAnimation : MonoBehaviour
{
    public CoinObjectPool pool; // 오브젝트 풀의 참조
    public RectTransform targetPosition; // 타겟 위치가 UI 요소일 경우 RectTransform

    public void StartGoldAnimation()
    {
        StartCoroutine(SpawnAndMoveGold());
    }

    private IEnumerator SpawnAndMoveGold()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject gold = pool.GetFromPool(); // 오브젝트 풀에서 골드 오브젝트를 가져옵니다.
            gold.SetActive(true);

            // 골드 오브젝트의 RectTransform을 가져옵니다.
            RectTransform goldRect = gold.GetComponent<RectTransform>();
            goldRect.SetParent(targetPosition.parent, false); // 타겟과 동일한 부모 설정, worldPositionStays를 false로 설정
            goldRect.localScale = Vector3.one; // 스케일을 1로 설정
            goldRect.anchoredPosition = Vector2.zero; // 캔버스의 정 중앙에서 시작

            // 골드를 타겟 위치로 이동시킵니다.
            StartCoroutine(MoveGold(goldRect, targetPosition.anchoredPosition, gold));
            yield return new WaitForSeconds(0.1f); // 다음 골드 생성 전에 잠시 대기
        }
    }

    private IEnumerator MoveGold(RectTransform goldRect, Vector2 targetAnchoredPos, GameObject gold)
    {
        float timeToMove = 1.0f;
        float elapsedTime = 0;

        Vector2 startingPos = goldRect.anchoredPosition;

        while (elapsedTime < timeToMove)
        {
            goldRect.anchoredPosition = Vector2.Lerp(startingPos, targetAnchoredPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        goldRect.anchoredPosition = targetAnchoredPos;

        pool.ReturnToPool(gold); // 오브젝트 풀로 반환
    }
}