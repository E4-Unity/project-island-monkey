using UnityEngine;

public class MonkeyBank : MonoBehaviour
{
	public static MonkeyBank Instance { get; private set; } // Singleton 인스턴스
	public int monkeybankCurrentGold = 0;
	public int monkeybankLimit = 3000;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void AddToBank(int amount)
	{
		if (monkeybankCurrentGold + amount <= monkeybankLimit)
		{
			monkeybankCurrentGold += amount;
			// 추가 로직: UI 업데이트, 사운드 재생 등
		}
		else
		{
			Debug.Log("몽키뱅크 꽉 참!");
		}
	}
}
