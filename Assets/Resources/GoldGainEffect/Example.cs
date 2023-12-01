using UnityEngine;

public class Example : MonoBehaviour
{
	public ItemAcquireFx prefabItem;
	public Transform target;
	private bool shouldCreateItemFx = false;

	public void ActivateItemFx()
	{
		shouldCreateItemFx = true;
	}

	void Update()
	{
		if (shouldCreateItemFx)
		{
			int randCount = Random.Range(1, 10);
			for (int i = 0; i < randCount; ++i)
			{
				var itemFx = GameObject.Instantiate<ItemAcquireFx>(prefabItem, this.transform);
				itemFx.Explosion(Input.mousePosition, target.position, 150.0f);
			}
			shouldCreateItemFx = false; // 재활성화를 위해 상태를 다시 false로 설정
		}
	}
}
