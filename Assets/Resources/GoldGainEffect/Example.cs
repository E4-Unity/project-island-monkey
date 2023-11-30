using UnityEngine;

public class Example : MonoBehaviour
{
    public ItemAcquireFx prefabItem;
    public Transform target;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            int randCount = Random.Range(1, 10);
            for (int i = 0; i < randCount; ++i)
            {
                var itemFx = GameObject.Instantiate<ItemAcquireFx>(prefabItem, this.transform);
                itemFx.Explosion(Input.mousePosition, target.position, 150.0f);
            }
        }
    }
}
