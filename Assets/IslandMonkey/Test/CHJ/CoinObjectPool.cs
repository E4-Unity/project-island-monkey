using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class CoinObjectPool : MonoBehaviour
{
    public GameObject coinPrefab; // 코인 프리팹

    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(coinPrefab),  // 새 오브젝트 생성
            actionOnGet: (obj) => obj.SetActive(true),  // 오브젝트를 가져올 때 실행
            actionOnRelease: (obj) => obj.SetActive(false),  // 오브젝트를 반환할 때 실행
            actionOnDestroy: Destroy,  // 오브젝트를 파괴할 때 실행
            defaultCapacity: 10,  // 기본 용량
            maxSize: 20  // 최대 용량
        );
    }

    // 풀에서 오브젝트를 가져오는 메서드
    public GameObject GetFromPool()
    {
        return pool.Get();
    }

    // 오브젝트를 풀로 반환하는 메서드
    public void ReturnToPool(GameObject obj)
    {
        pool.Release(obj);
    }
}